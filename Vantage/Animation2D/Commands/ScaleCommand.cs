namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.OsbTypes;

    public class ScaleCommand : Command<OsbDecimal>
    {
        public ScaleCommand(int easing, float startTime, float endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("S", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}