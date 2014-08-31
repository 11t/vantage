namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class ScaleCommand : Command<OsbDecimal>
    {
        public ScaleCommand(int easing, double startTime, double endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("S", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}