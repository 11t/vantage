namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class OsuEaseInEasingCurve : IEasingCurve
    {
        public float Evaluate(float x)
        {
            return x * x;
        }
    }
}
