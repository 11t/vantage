namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class MoveYCommand : Command<OsbDecimal>
    {
        public MoveYCommand(int easing, double startTime, double endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("MY", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}