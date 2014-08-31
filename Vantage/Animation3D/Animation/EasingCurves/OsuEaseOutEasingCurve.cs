namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class OsuEaseOutEasingCurve : IEasingCurve
    {
        public double Evaluate(double x)
        {
            double a = 1 - x;
            return -(a * a) + 1;
        }
    }
}
