namespace Vantage.Animation2D.Commands
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.OsbTypes;

    public class ParameterCommand : Command<OsbParameter>
    {
        public ParameterCommand(int easing, float startTime, float endTime, OsbParameter startValue)
            : base("P", easing, startTime, endTime, startValue, startValue)
        {
        }
    }
}