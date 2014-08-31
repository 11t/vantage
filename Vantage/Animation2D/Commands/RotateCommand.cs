namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class RotateCommand : Command<OsbDecimal>
    {
        public RotateCommand(int easing, double startTime, double endTime, OsbDecimal startValue, OsbDecimal endValue)
            : base("R", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}