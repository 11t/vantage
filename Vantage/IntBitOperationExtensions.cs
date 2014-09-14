namespace Vantage
{
    using System;

    public static class IntBitOperationExtensions
    {
        public static int SetBit(this int number, int bit)
        {
            return number | (1 << bit);
        }

        public static int ClearBit(this int number, int bit)
        {
            return number & ~(1 << bit);
        }

        public static int ToggleBit(this int number, int bit)
        {
            return number ^ (1 << bit);
        }

        public static int SetClearBit(this int number, int bit, bool value)
        {
            if (value)
            {
                return number.SetBit(bit);
            }

            return number.ClearBit(bit);
        }

        public static bool CheckBit(this int number, int bit)
        {
            return Convert.ToBoolean(number & (1 << bit));
        }
    }
}
