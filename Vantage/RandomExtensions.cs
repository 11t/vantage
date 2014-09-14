namespace Vantage
{
    using System;

    using SharpDX;

    public static class RandomExtensions
    {
        public static int NextSigned(this Random r, int min, int max)
        {
            int sign = r.Next(0, 2);
            int next = r.Next(min, max);
            if (sign != 0)
            {
                next *= -1;
            }

            return next;
        }

        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var randStandardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var randNormal = mu + (sigma * randStandardNormal);
            return randNormal;
        }

        public static Vector3 NextSpherePoint(this Random r, Vector3 center, double minRadius, double maxRadius)
        {
            var x1 = r.NextGaussian();
            var x2 = r.NextGaussian();
            var x3 = r.NextGaussian();
            var u = r.NextDouble(minRadius / maxRadius, 1);
            var d = (maxRadius * Math.Pow(u, 1.0 / 3.0)) / Math.Sqrt((x1 * x1) + (x2 * x2) + (x3 * x3));
            return new Vector3((float)(d * x1), (float)(d * x2), (float)(d * x3)) + center;
        }
    }
}
