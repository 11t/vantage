namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class MoveXCommand : Command<OsbDecimal>
    {
        public MoveXCommand(int easing, double startTime, double endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("MX", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}