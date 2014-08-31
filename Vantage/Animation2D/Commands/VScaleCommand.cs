namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class VScaleCommand : Command<OsbScale>
    {
        public VScaleCommand(int easing, double startTime, double endTime, OsbScale startValue, OsbScale endValue)
            : base("V", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}