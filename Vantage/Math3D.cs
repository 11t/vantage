namespace Vantage
{
    using System;

    using SharpDX;

    public static class Math3D
    {
        public const double Pi2 = 2 * Math.PI;

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
    }
}
