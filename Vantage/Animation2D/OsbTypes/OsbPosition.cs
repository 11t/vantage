namespace Vantage.Animation2D.OsbTypes
{
    using System;
    using System.Globalization;

    using SharpDX;

    public struct OsbPosition : IOsbType, IEquatable<OsbPosition>
    {
        private readonly double x;
        private readonly double y;

        public OsbPosition(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public OsbPosition(Vector2 vector)
            : this(vector.X, vector.Y)
        {
        }

        public double X
        {
            get
            {
                return this.x;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
        }

        public static bool operator ==(OsbPosition left, OsbPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OsbPosition left, OsbPosition right)
        {
            return !left.Equals(right);
        }

        public static implicit operator Vector2(OsbPosition position)
        {
            return new Vector2((float)position.X, (float)position.Y);
        }

        public static implicit operator OsbPosition(Vector2 vector)
        {
            return new OsbPosition(vector.X, vector.Y);
        }

        public static float Distance(OsbPosition a, OsbPosition b)
        {
            double diffX = a.X - b.X;
            double diffY = a.Y - b.Y;
            return (float)Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

        public bool Equals(OsbPosition other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is OsbPosition && this.Equals((OsbPosition)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.x.GetHashCode() * 397) ^ this.y.GetHashCode();
            }
        }

        public string ToOsbString()
        {
            int intX = (int)Math.Round(this.X);
            int intY = (int)Math.Round(this.Y);
            return intX.ToString(CultureInfo.InvariantCulture) + "," + intY;
        }

        public float DistanceFrom(object obj)
        {
            return Distance(this, (OsbPosition)obj);
        }
    }
}