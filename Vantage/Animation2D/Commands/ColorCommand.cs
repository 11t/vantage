namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.OsbTypes;

    public class ColorCommand : Command<OsbColor>
    {
        public ColorCommand(int easing, float startTime, float endTime, OsbColor startValue, OsbColor endValue)
            : base("C", easing, startTime, endTime, startValue, endValue)
        {
        }
    }
}