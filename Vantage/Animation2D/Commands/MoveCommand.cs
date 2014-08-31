namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class MoveCommand : Command<OsbPosition>
    {
        public MoveCommand(int easing, double startTime, double endTime, OsbPosition startValue, OsbPosition endValue)
            : base("M", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}