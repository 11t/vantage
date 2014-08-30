namespace Vantage.Animation3D.Animation.EasingCurves
{
    public class BasicEasingCurve
    {
        public static readonly StepEasingCurve Step = new StepEasingCurve();
        public static readonly LinearEasingCurve Linear = new LinearEasingCurve();
        public static readonly OsuEaseInEasingCurve OsuEaseIn = new OsuEaseInEasingCurve();
        public static readonly OsuEaseOutEasingCurve OsuEaseOut = new OsuEaseOutEasingCurve();

        public static IEasingCurve FromEasingParameter(int easing)
        {
            switch (easing)
            {
                case 0:
                    return Linear;
                case 1:
                    return OsuEaseOut;
                case 2:
                    return OsuEaseIn;
                default:
                    return Step;
            }
        }
    }
}