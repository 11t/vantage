// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OsbColor.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage.Animation2D.OsbTypes
{
    using System;
    using System.Drawing;
    using System.Globalization;

    using SharpDX;

    using Color = System.Drawing.Color;

    public struct OsbColor : IOsbType, IEquatable<OsbColor>
    {
        #region Static Fields

        public static OsbColor AliceBlue = FromHtml("#F0F8FF");

        public static OsbColor AntiqueWhite = FromHtml("#FAEBD7");

        public static OsbColor Aqua = FromHtml("#00FFFF");

        public static OsbColor Aquamarine = FromHtml("#7FFFD4");

        public static OsbColor Azure = FromHtml("#F0FFFF");

        public static OsbColor Beige = FromHtml("#F5F5DC");

        public static OsbColor Bisque = FromHtml("#FFE4C4");

        public static OsbColor Black = FromHtml("#000000");

        public static OsbColor BlanchedAlmond = FromHtml("#FFEBCD");

        public static OsbColor Blue = FromHtml("#0000FF");

        public static OsbColor BlueViolet = FromHtml("#8A2BE2");

        public static OsbColor Brown = FromHtml("#A52A2A");

        public static OsbColor BurlyWood = FromHtml("#DEB887");

        public static OsbColor CadetBlue = FromHtml("#5F9EA0");

        public static OsbColor DarkSlateBlue = FromHtml("#483D8B");

        public static OsbColor Fuchsia = FromHtml("#FF00FF");

        public static OsbColor Green = FromHtml("#008000");

        public static OsbColor Lime = FromHtml("#00FF00");

        public static OsbColor Purple = FromHtml("#800080");

        public static OsbColor Red = FromHtml("#FF0000");

        public static OsbColor White = FromHtml("#FFFFFF");

        public static OsbColor Yellow = FromHtml("#FFFF00");

        #endregion

        #region Fields

        private readonly double b;

        private readonly double g;

        private readonly double r;

        #endregion

        #region Constructors and Destructors

        public OsbColor(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public OsbColor(Vector3 vector)
            : this(vector.X, vector.Y, vector.Z)
        {
        }

        #endregion

        #region Public Properties

        public int B
        {
            get
            {
                return Clamp((int)(this.b * 255));
            }
        }

        public int G
        {
            get
            {
                return Clamp((int)(this.g * 255));
            }
        }

        public int R
        {
            get
            {
                return Clamp((int)(this.r * 255));
            }
        }

        #endregion

        #region Public Methods and Operators

        public static OsbColor FromHsb(double hue, double saturation, double brightness)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = (hue / 60) - Math.Floor(hue / 60);

            double v = brightness;
            double p = brightness * (1 - saturation);
            double q = brightness * (1 - (f * saturation));
            double t = brightness * (1 - ((1 - f) * saturation));

            if (hi == 0)
            {
                return new OsbColor(v, t, p);
            }

            if (hi == 1)
            {
                return new OsbColor(q, v, p);
            }

            if (hi == 2)
            {
                return new OsbColor(p, v, t);
            }

            if (hi == 3)
            {
                return new OsbColor(p, q, v);
            }

            if (hi == 4)
            {
                return new OsbColor(t, p, v);
            }

            return new OsbColor(v, p, q);
        }

        public static OsbColor FromHtml(string htmlColor)
        {
            Color sysColor = ColorTranslator.FromHtml(htmlColor);
            return new OsbColor(sysColor.R / 255.0f, sysColor.G / 255.0f, sysColor.B / 255.0f);
        }

        public static bool operator ==(OsbColor left, OsbColor right)
        {
            return left.Equals(right);
        }

        public static implicit operator OsbColor(Vector3 vector)
        {
            return new OsbColor(vector);
        }

        public static bool operator !=(OsbColor left, OsbColor right)
        {
            return !left.Equals(right);
        }

        public float DistanceFrom(object obj)
        {
            OsbColor other = (OsbColor)obj;
            float diffR = this.R - other.R;
            float diffG = this.G - other.G;
            float diffB = this.B - other.B;
            return (float)Math.Sqrt((diffR * diffR) + (diffG * diffG) + (diffB * diffB));
        }

        public bool Equals(OsbColor other)
        {
            return this.r.Equals(other.r) && this.g.Equals(other.g) && this.b.Equals(other.b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is OsbColor && this.Equals((OsbColor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.r.GetHashCode();
                hashCode = (hashCode * 397) ^ this.g.GetHashCode();
                hashCode = (hashCode * 397) ^ this.b.GetHashCode();
                return hashCode;
            }
        }

        public string ToOsbString()
        {
            return this.R.ToString(CultureInfo.InvariantCulture) + ',' + this.G + ',' + this.B;
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)this.r, (float)this.g, (float)this.b);
        }

        #endregion

        #region Methods

        private static int Clamp(int x)
        {
            if (x > 255)
            {
                x = 255;
            }

            if (x < 0)
            {
                x = 0;
            }

            return x;
        }

        #endregion
    }
}