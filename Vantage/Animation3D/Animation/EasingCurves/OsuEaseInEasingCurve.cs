namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class OsuEaseInEasingCurve : IEasingCurve
    {
        public double Evaluate(double x)
        {
            return x * x;
        }
    }
}
