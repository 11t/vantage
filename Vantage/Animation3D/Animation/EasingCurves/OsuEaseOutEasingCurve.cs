namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class OsuEaseOutEasingCurve : IEasingCurve
    {
        public float Evaluate(float x)
        {
            float a = 1 - x;
            return -(a * a) + 1;
        }
    }
}
