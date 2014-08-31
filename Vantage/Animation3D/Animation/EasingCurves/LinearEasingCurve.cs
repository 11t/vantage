namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class LinearEasingCurve : IEasingCurve
    {
        public double Evaluate(double x)
        {
            return x;
        }
    }
}