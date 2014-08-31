namespace Vantage.Animation3D.Animation.Keyframes
{
    public class DiscreteKeyframe<T> : IKeyframe<T>
    {
        public DiscreteKeyframe(double time, T value)
        {
            this.Time = time;
            this.Value = value;
        }

        public double Time { get; set; }

        public T Value { get; set; }

        public T ValueAtTime(IKeyframe<T> next, double time)
        {
            return this.Value;
        }
    }
}
