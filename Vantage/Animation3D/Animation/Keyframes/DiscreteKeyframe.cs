namespace Vantage.Animation3D.Animation.Keyframes
{
    public class DiscreteKeyframe<T> : IKeyframe<T>
    {
        public DiscreteKeyframe(float time, T value)
        {
            this.Time = time;
            this.Value = value;
        }

        public float Time { get; set; }

        public T Value { get; set; }

        public T ValueAtTime(IKeyframe<T> next, float time)
        {
            return this.Value;
        }
    }
}
