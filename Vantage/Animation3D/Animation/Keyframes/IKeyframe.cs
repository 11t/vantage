namespace Vantage.Animation3D.Animation.Keyframes
{
    public interface IKeyframe<T>
    {
        double Time { get; set; }

        T Value { get; set; }

        T ValueAtTime(IKeyframe<T> next, double time);
    }
}