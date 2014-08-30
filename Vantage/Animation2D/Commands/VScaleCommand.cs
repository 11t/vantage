namespace Vantage.Animation2D.Commands
{
    using System.Globalization;

    using Vantage.Animation2D.OsbTypes;

    public class VScaleCommand : Command<OsbScale>
    {
        public VScaleCommand(int easing, float startTime, float endTime, OsbScale startValue, OsbScale endValue)
            : base("V", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}