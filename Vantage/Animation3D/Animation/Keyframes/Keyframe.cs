namespace Vantage.Animation3D.Animation.Keyframes
{
    using Vantage.Animation3D.Animation.EasingCurves;

    public class Keyframe<T> : IKeyframe<T>
    {
        public Keyframe(float time, T value)
            : this(time, value, BasicEasingCurve.Linear)
        {
        }

        public Keyframe(float time, T value, IEasingCurve easingCurve)
        {
            this.Time = time;
            this.Value = value;
            this.EasingCurve = easingCurve;
        }

        public float Time { get; set; }

        public T Value { get; set; }

        public IEasingCurve EasingCurve { get; set; }

        public T ValueAtTime(IKeyframe<T> next, float time)
        {
            if (time <= this.Time || this.EasingCurve == null || next == null)
            {
                return this.Value;
            }

            var x = (time - this.Time) / (next.Time - this.Time);
            var y = this.EasingCurve.Evaluate(x);
            return this.Interpolate(this.Value, next.Value, y);
        }

        internal virtual T Interpolate(T start, T end, float amount)
        {
            dynamic a = start;
            dynamic b = end;
            return (a * (1 - amount)) + (b * amount);
        }
    }
}
