namespace Vantage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms.VisualStyles;

    using SharpDX;

    public static class Math3D
    {
        public const double DoubleEpsilon = 0.00001;

        public const double Pi2 = 2 * Math.PI;

        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public static bool TimesAreEqual(double a, double b)
        {
            return Math.Abs(a - b) < StoryboardSettings.Instance.SceneConversionSettings.TimePrecision;
        }

        public static double AngleDistance(double a, double b)
        {
            double change = Math.Abs(a - b);
            double modChange = change % Pi2;
            double distance = Math.PI - Math.Abs(Math.PI - modChange);
            return distance;
        }

        public static bool IsClockwiseRotation(double start, double end)
        {
            var modStart = start % Pi2;
            var modEnd = end % Pi2;
            var difference = modEnd - modStart;
            bool clockwise = difference < 0;
            if (Math.Abs(difference) > Math.PI)
            {
                clockwise = !clockwise;
            }

            return clockwise;
        }

        public static Quaternion RotationFromTo(Vector3 fromVector, Vector3 toVector)
        {
            const float DotError = 0.001f;

            float dot = Vector3.Dot(fromVector, toVector);

            if (dot > 1 - DotError)
            {
                return Quaternion.Identity;
            }

            if (dot < -(1 - DotError))
            {
                return Quaternion.RotationAxis(Vector3.UnitY, (float)Math.PI);
            }

            Vector3 xyz = Vector3.Cross(fromVector, toVector);
            float w = (float)Math.Sqrt(fromVector.LengthSquared() * toVector.LengthSquared()) + dot;
            Quaternion rotationQuaternion = new Quaternion(xyz, w);
            rotationQuaternion.Normalize();
            return rotationQuaternion;
        }

        public static double SumSquaresError<T>(IList<double> times, IList<T> values, double slope, double intercept)
        {
            double sumSquaresError = 0;
            for (int i = 0; i < times.Count; i++)
            {
                dynamic value = values[i];
                double error = value - ((times[i] * slope) + intercept);
                sumSquaresError += error * error;
            }

            return sumSquaresError;
        }

        public static Tuple<double, double> LinearLeastSquares<T>(IList<double> times, IList<T> values)
        {
            double slope;
            double intercept;

            double x = 0;
            dynamic y = default(T);
            dynamic xy = default(T);
            double x2 = 0;
            double n = times.Count;

            for (int i = 0; i < times.Count; i++)
            {
                double time = times[i];
                dynamic value = values[i];
                x += time;
                y += value;
                xy += time * value;
                x2 += time * time;
            }

            double j = (times.Count * x2) - (x * x);
            if (Math.Abs(j) >= DoubleEpsilon)
            {
                slope = ((n * xy) - (x * y)) / j;
                intercept = ((y * x2) - (x * xy)) / j;
            }
            else
            {
                slope = 0;
                intercept = 0;
            }

            return new Tuple<double, double>(slope, intercept);
        }
    }
}
