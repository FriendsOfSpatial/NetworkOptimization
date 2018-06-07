using System;

namespace NetworkOptimization
{
    public static class Helper
    {
        const double squareRootOfTwo = 1.414213562373095;
        const double quaternionSmallestThreeMaxValue = 1d / squareRootOfTwo;

        public static Byte QuantizeToByte(double value, double min, double max)
        {
            return (byte)((value - min) / (max - min) * byte.MaxValue);
        }

        public static UInt16 QuantizeToUInt16(double value, double min, double max)
        {
            return (ushort)((value - min) / (max - min) * ushort.MaxValue);
        }

        public static Int16 QuantizeToInt16(double value, double min, double max)
        {
            return (short)(((value - min) / (max - min) * ushort.MaxValue) + short.MinValue);
        }

        public static Int16 QuantizeUnitQuaternionComponent(double value)
        {
            return (short)((value / quaternionSmallestThreeMaxValue) * short.MaxValue);
        }

        public static Double UnquantizeQuaternionComponent(short value)
        {
            return (value * quaternionSmallestThreeMaxValue / short.MaxValue);
        }

        public static Byte[] Combine(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length, third.Length);
            return ret;
        }
    }
}
