namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class FadeCommand : Command<OsbDecimal>
    {
        public FadeCommand(int easing, double startTime, double endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("F", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}