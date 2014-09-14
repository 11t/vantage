using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantage
{
    using SharpDX;

    public class BezierCurve
    {
        public BezierCurve()
        {
            this.Points = new List<Vector3>();
        }

        public BezierCurve(IEnumerable<Vector3> points)
            : this()
        {
            foreach (var point in points)
            {
                this.Points.Add(point);
            }
        }

        public IList<Vector3> Points { get; private set; }

        public Vector3 Evaluate(double t)
        {
            int n = 3;
            Vector3 sum = default(Vector3);
            for (int i = 0; i <= n; i++)
            {
                int combination = this.Combination(n, i);
                sum += combination * (float)Math.Pow(1 - t, n - 1) * (float)Math.Pow(t, i) * this.Points[i];
            }

            return sum;
        }

        private int Combination(int n, int k)
        {
            return this.Factorial(n) / (this.Factorial(k) * this.Factorial(n - k));
        }

        private int Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            return n * this.Factorial(n - 1);
        }
    }
}
