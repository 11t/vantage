namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class MoveXCommand : Command<OsbDecimal>
    {
        public MoveXCommand(int easing, float startTime, float endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("MX", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}