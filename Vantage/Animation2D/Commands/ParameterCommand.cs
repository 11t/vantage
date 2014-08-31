namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D.OsbTypes;

    public class ParameterCommand : Command<OsbParameter>
    {
        public ParameterCommand(int easing, double startTime, double endTime, OsbParameter startValue)
            : base("P", easing, startTime, endTime, startValue, startValue)
        {
        }
    }
}