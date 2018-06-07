using System;

namespace NetworkOptimization
{
    public static class Encode
    {
        /// <summary>
        /// Encode a 3D vector of float into 12 bytes.
        /// </summary>
        public static Byte[] Vector3f(float x, float y, float z)
        {
            byte[] bx = BitConverter.GetBytes(x);
            byte[] by = BitConverter.GetBytes(y);
            byte[] bz = BitConverter.GetBytes(z);

            return Helper.Combine(bx, by, bz);
        }
        
        /// <summary>
        /// Encode a quaternion into 7 bytes using smallest three technique.
        /// </summary>
        public static Byte[] Quaternion(float x, float y, float z, float w)
        {
            float[] rotation = new float[4] { x, y, z, w };

            float largestComponentValue = 0f;
            byte largestComponentIndex = 0;
            bool largestComponentSign = true;

            for (byte i = 0; i < 4; i++)
            {
                float value = rotation[i];

                if (Math.Abs(value) > Math.Abs(largestComponentValue))
                {
                    largestComponentValue = value;
                    largestComponentIndex = i;
                    largestComponentSign = value > 0;
                }
            }

            short[] quantizedComponents = new short[3];

            byte shiftedIndex = 0;
            for (byte i = 0; i < 4; i++)
            {
                float value = rotation[i];

                if (i != largestComponentIndex)
                {
                    if (!largestComponentSign)
                    {
                        value = -value;
                    }

                    quantizedComponents[shiftedIndex] = Helper.QuantizeUnitQuaternionComponent(value);
                    shiftedIndex++;
                }
            }
            
            byte[] ret = new byte[7];
            ret[0] = largestComponentIndex;
            Buffer.BlockCopy(BitConverter.GetBytes(quantizedComponents[0]), 0, ret, 1, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(quantizedComponents[1]), 0, ret, 3, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(quantizedComponents[2]), 0, ret, 5, 2);

            return ret;
        }
        
        /// <summary>
        /// Encode a 3D vector representing velocity into 6 bytes.
        /// </summary>
        public static Byte[] Velocity(float x, float y, float z, float maxVelocity)
        {
            short qx = Helper.QuantizeToInt16(x, -maxVelocity, maxVelocity);
            short qy = Helper.QuantizeToInt16(y, -maxVelocity, maxVelocity);
            short qz = Helper.QuantizeToInt16(z, -maxVelocity, maxVelocity);

            byte[] bx = BitConverter.GetBytes(qx);
            byte[] by = BitConverter.GetBytes(qy);
            byte[] bz = BitConverter.GetBytes(qz);

            return Helper.Combine(bx, by, bz);
        }
        
        /// <summary>
        /// Encode a Vector3 representing a position relative to the ship into 6 bytes.
        /// </summary>
        public static Byte[] RelativePosition(float x, float y, float z)
        {
            short qx = Helper.QuantizeToInt16(x, Int16.MinValue, Int16.MaxValue);
            short qy = Helper.QuantizeToInt16(y, Int16.MinValue, Int16.MaxValue);
            short qz = Helper.QuantizeToInt16(z, Int16.MinValue, Int16.MaxValue);

            byte[] bx = BitConverter.GetBytes(qx);
            byte[] by = BitConverter.GetBytes(qy);
            byte[] bz = BitConverter.GetBytes(qz);

            return Helper.Combine(bx, by, bz);
        }
    }
}
