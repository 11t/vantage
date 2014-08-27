namespace Vantage.Animation3D.Animation.EasingCurves
{
    using SharpDX;

    public class CubicBezierEasingCurve : IEasingCurve
    {
        public static readonly CubicBezierEasingCurve Linear = new CubicBezierEasingCurve(0, 0, 0, 0);
        public static readonly CubicBezierEasingCurve EaseIn = new CubicBezierEasingCurve(0.5f, 0, 1, 0.5f);
        public static readonly CubicBezierEasingCurve EaseOut = new CubicBezierEasingCurve(0, 0.5f, 0.5f, 1);

        public CubicBezierEasingCurve(Vector2 p1, Vector2 p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        public CubicBezierEasingCurve(float x1, float y1, float x2, float y2)
            : this(new Vector2(x1, y1), new Vector2(x2, y2))
        {
        }

        public Vector2 P1 { get; set; }

        public Vector2 P2 { get; set; }

        public float Evaluate(float x)
        {
            if (this.P1.X == this.P1.Y && this.P2.X == this.P2.Y)
            {
                return x;
            }
            var t = this.FindT(x);
            var C = 3.0f * this.P1.Y;
            var D = 3.0f * this.P2.Y;
            var A = 1.0f - D + C;
            var B = D - 2.0f * C;
            return ((A * t + B) * t + C) * t;
        }

        // Finds the t value that corresponds to the given x value on the curve
        private float FindT(float x)
        {
            var x1 = this.P1.X;
            var x2 = this.P2.X;
            var t = x;
            for (var i = 0; i < 4; i++)
            {
                var C = 3.0f * x1;
                var D = 3.0f * x2;
                var A = 1.0f - D + C;
                var B = D - 2.0f * C;
                var slope = 3.0f * A * t * t + 2.0f * B * t + C;
                if (slope == 0.0f)
                {
                    return t;
                }
                var currentX = ((A * t + B) * t + C) * t - x;
                t -= currentX / slope;
            }
            return t;
        }
    }
}
