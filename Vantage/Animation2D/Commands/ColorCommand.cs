namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class ColorCommand : Command<OsbColor>
    {
        public ColorCommand(int easing, double startTime, double endTime, OsbColor startValue, OsbColor endValue)
            : base("C", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}