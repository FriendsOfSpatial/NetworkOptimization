using System;

namespace NetworkOptimization
{
    public static class Decode
    {
        /// <summary>
        /// Decode 12 bytes into a 3D vector of floats.
        /// </summary>
        public static float[] Vector3f(Byte[] encodedComponents)
        {
            float x = BitConverter.ToSingle(encodedComponents, 0);
            float y = BitConverter.ToSingle(encodedComponents, 4);
            float z = BitConverter.ToSingle(encodedComponents, 8);

            return new float[3] { x, y, z };
        }
        
        /// <summary>
        /// Decode 7 bytes into a quaternion using smallest three technique.
        /// </summary>
        public static float[] Quaternion(Byte[] encodedQuaternion)
        {
            byte largestComponentIndex = encodedQuaternion[0];

            short[] decodedComponents = new short[3]
            {
                BitConverter.ToInt16(encodedQuaternion, 1),
                BitConverter.ToInt16(encodedQuaternion, 3),
                BitConverter.ToInt16(encodedQuaternion, 5),
            };
            
            double[] unquantizedComponents = new double[3]
            {
                Helper.UnquantizeQuaternionComponent(decodedComponents[0]),
                Helper.UnquantizeQuaternionComponent(decodedComponents[1]),
                Helper.UnquantizeQuaternionComponent(decodedComponents[2]),
            };
            
            float[] result = new float[4];
            byte shiftedIndex = 0;
            for (byte i = 0; i < 4; i++)
            {
                if (i == largestComponentIndex)
                {
                    result[i] = (float)Math.Sqrt(1d - (unquantizedComponents[0] * unquantizedComponents[0]) - (unquantizedComponents[1] * unquantizedComponents[1]) - (unquantizedComponents[2] * unquantizedComponents[2]));
                }
                else
                {
                    result[i] = (float)unquantizedComponents[shiftedIndex];
                    shiftedIndex++;
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Decode 6 bytes into a 3D vector representing velocity.
        /// </summary>
        public static float[] Velocity(Byte[] encodedComponents, float maxVelocity)
        {
            float x = BitConverter.ToInt16(encodedComponents, 0) * maxVelocity / Int16.MaxValue;
            float y = BitConverter.ToInt16(encodedComponents, 2) * maxVelocity / Int16.MaxValue;
            float z = BitConverter.ToInt16(encodedComponents, 4) * maxVelocity / Int16.MaxValue;
            
            return new float[3] { x, y, z };
        }
        
        /// <summary>
        /// Decode 6 bytes into a Vector3 representing a position relative to the ship.
        /// </summary>
        public static float[] RelativePosition(Byte[] encodedComponents)
        {
            float x = BitConverter.ToInt16(encodedComponents, 0);
            float y = BitConverter.ToInt16(encodedComponents, 2);
            float z = BitConverter.ToInt16(encodedComponents, 4);

            return new float[] { x, y, z };
        }
    }
}
