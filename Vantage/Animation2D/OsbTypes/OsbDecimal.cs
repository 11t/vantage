namespace Vantage.Animation2D.OsbTypes
{
    using System;
    using System.Globalization;

    public struct OsbDecimal : IOsbType, IEquatable<OsbDecimal>
    {
        public const int Precision = 4;
        public static double MaximumError = Math.Pow(10, -4);

        private readonly double value;

        public OsbDecimal(double value)
        {
            this.value = Math.Round(value, Precision);
        }

        public static bool operator ==(OsbDecimal left, OsbDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OsbDecimal left, OsbDecimal right)
        {
            return !left.Equals(right);
        }

        public static implicit operator OsbDecimal(double value)
        {
            return new OsbDecimal(value);
        }

        public static implicit operator double(OsbDecimal obj)
        {
            return obj.value;
        }

        public static OsbDecimal operator -(OsbDecimal left, OsbDecimal right)
        {
            return new OsbDecimal(left.value - right.value);
        }

        public static OsbDecimal operator +(OsbDecimal left, OsbDecimal right)
        {
            return new OsbDecimal(left.value + right.value);
        }

        public bool Equals(OsbDecimal other)
        {
            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is OsbDecimal && this.Equals((OsbDecimal)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public bool Approximately(OsbDecimal other)
        {
            return Math.Abs(this.value - other.value) < MaximumError;
        }

        public string ToOsbString()
        {
            int ivalue = (int)Math.Round(this.value);
            if (this.Approximately(ivalue))
            {
                return ivalue.ToString(CultureInfo.InvariantCulture);
            }

            return this.value.ToString(CultureInfo.InvariantCulture);
        }

        public float DistanceFrom(object obj)
        {
            return (float)Math.Abs(this.value - ((OsbDecimal)obj).value);
        }
    }
}