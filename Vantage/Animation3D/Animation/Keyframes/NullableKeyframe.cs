namespace Vantage.Animation3D.Animation.Keyframes
{
    using Vantage.Animation3D.Animation.EasingCurves;

    public class NullableKeyframe<TNullable> : Keyframe<TNullable?> where TNullable : struct
    {
        public NullableKeyframe(double time, TNullable? value, IEasingCurve easingCurve)
            : base(time, value, easingCurve)
        {
        }

        public NullableKeyframe(double time, TNullable? value)
            : base(time, value)
        {
        }

        internal override TNullable? Interpolate(TNullable? start, TNullable? end, double amount)
        {
            if (start == null)
            {
                return null;
            }

            if (end == null)
            {
                return start;
            }

            dynamic a = start.Value;
            dynamic b = end.Value;
            float floatAmount = (float)amount;
            return (a * (1 - floatAmount)) + (b * floatAmount);
        }
    }
}
