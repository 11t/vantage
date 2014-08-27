namespace Vantage.Animation3D.Animation.Keyframes
{
    using SharpDX;

    using Vantage.Animation3D.Animation.EasingCurves;

    public class QuaternionKeyframe : Keyframe<Quaternion>
    {
        public QuaternionKeyframe(float time, Quaternion value, IEasingCurve easingCurve)
            : base(time, value, easingCurve)
        {
        }

        public QuaternionKeyframe(float time, Quaternion value)
            : base(time, value)
        {
        }

        internal override Quaternion Interpolate(Quaternion start, Quaternion end, float amount)
        {
            return Quaternion.Slerp(start, end, amount);
        }
    }
}