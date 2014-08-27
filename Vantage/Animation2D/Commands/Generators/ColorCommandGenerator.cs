namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class ColorCommandGenerator : CommandGenerator<OsbColor>
    {
        public ColorCommandGenerator(float allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(float time, OsbColor value)
        {
            return new ColorCommand(0, this.Time, time, this.Value, value);
        }
    }
}