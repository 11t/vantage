namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.OsbTypes;

    public class MoveCommand : Command<OsbPosition>
    {
        public MoveCommand(int easing, float startTime, float endTime, OsbPosition startValue, OsbPosition endValue)
            : base("M", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}