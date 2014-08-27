namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class LinearEasingCurve : IEasingCurve
    {
        public float Evaluate(float x)
        {
            return x;
        }
    }
}