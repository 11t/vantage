namespace Vantage.Animation3D.Animation.EasingCurves
{
    using System;

    using SharpDX;

    public class CubicBezierEasingCurve : IEasingCurve
    {
        public static readonly CubicBezierEasingCurve Linear = new CubicBezierEasingCurve(0, 0, 0, 0);
        public static readonly CubicBezierEasingCurve EaseInStrong = new CubicBezierEasingCurve(0.5, 0, 1, 0.5);
        public static readonly CubicBezierEasingCurve EaseOutStrong = new CubicBezierEasingCurve(0, 0.5, 0.5, 1);
        public static readonly CubicBezierEasingCurve EaseIn = new CubicBezierEasingCurve(0.5, 0, 1, 1);
        public static readonly CubicBezierEasingCurve EaseOut = new CubicBezierEasingCurve(0, 0, 0.5, 1);
        public static readonly CubicBezierEasingCurve EaseInOut = new CubicBezierEasingCurve(0.5, 0, 0.5, 1);

        public CubicBezierEasingCurve(Vector2 p1, Vector2 p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        public CubicBezierEasingCurve(double x1, double y1, double x2, double y2)
            : this(new Vector2((float)x1, (float)y1), new Vector2((float)x2, (float)y2))
        {
        }

        public Vector2 P1 { get; set; }

        public Vector2 P2 { get; set; }

        public double Evaluate(double x)
        {
            var t = this.FindT(x);
            var c = 3.0f * this.P1.Y;
            var d = 3.0f * this.P2.Y;
            var a = 1.0f - d + c;
            var b = d - (2.0f * c);
            return ((((a * t) + b) * t) + c) * t;
        }

        // Finds the t value that corresponds to the given x value on the curve
        private double FindT(double x)
        {
            var x1 = this.P1.X;
            var x2 = this.P2.X;
            var t = x;
            for (var i = 0; i < 4; i++)
            {
                var c = 3.0f * x1;
                var d = 3.0f * x2;
                var a = 1.0f - d + c;
                var b = d - (2.0f * c);
                var slope = (3.0f * a * t * t) + (2.0f * b * t) + c;
                if (Math.Abs(slope) < 0.0001)
                {
                    return t;
                }

                var currentX = (((((a * t) + b) * t) + c) * t) - x;
                t -= currentX / slope;
            }

            return t;
        }
    }
}
