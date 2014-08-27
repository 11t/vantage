namespace Vantage.Animation3D.Animation.Keyframes
{
    public interface IKeyframe<T>
    {
        float Time { get; set; }

        T Value { get; set; }

        T ValueAtTime(IKeyframe<T> next, float time);
    }
}