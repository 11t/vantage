namespace Vantage.Animation2D.OsbTypes
{
    using System;

    using SharpDX;

    public struct OsbScale : IOsbType, IEquatable<OsbScale>
    {
        private readonly OsbDecimal x;
        private readonly OsbDecimal y;

        public OsbScale(OsbDecimal x, OsbDecimal y)
        {
            this.x = x;
            this.y = y;
        }

        public OsbScale(Vector2 vector)
            : this(vector.X, vector.Y)
        {
        }

        public OsbDecimal X
        {
            get
            {
                return this.x;
            }
        }

        public OsbDecimal Y
        {
            get
            {
                return this.y;
            }
        }

        public static implicit operator OsbScale(Vector2 vector)
        {
            return new OsbScale(vector);
        }

        public static bool operator ==(OsbScale left, OsbScale right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OsbScale left, OsbScale right)
        {
            return !left.Equals(right);
        }

        public bool Equals(OsbScale other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is OsbScale && this.Equals((OsbScale)obj);
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
            return this.X.ToOsbString() + ',' + this.Y.ToOsbString();
        }

        public float DistanceFrom(object obj)
        {
            OsbScale other = (OsbScale)obj;
            float diffX = this.X.DistanceFrom(other.X);
            float diffY = this.Y.DistanceFrom(other.Y);
            return (float)Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }
    }
}