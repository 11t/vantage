namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class StepEasingCurve : IEasingCurve
    {
        public double Evaluate(double x)
        {
            if (x >= 1)
            {
                return 1;
            }

            return 0;
        }
    }
}