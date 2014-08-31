namespace Vantage.Animation3D.Animation.Keyframes
{
    using SharpDX;

    using Vantage.Animation3D.Animation.EasingCurves;

    public class QuaternionKeyframe : Keyframe<Quaternion>
    {
        public QuaternionKeyframe(double time, Quaternion value, IEasingCurve easingCurve)
            : base(time, value, easingCurve)
        {
        }

        public QuaternionKeyframe(double time, Quaternion value)
            : base(time, value)
        {
        }

        internal override Quaternion Interpolate(Quaternion start, Quaternion end, double amount)
        {
            return Quaternion.Slerp(start, end, (float)amount);
        }
    }
}