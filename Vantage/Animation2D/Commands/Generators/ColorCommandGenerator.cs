namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class ColorCommandGenerator : CommandGenerator<OsbColor>
    {
        public ColorCommandGenerator(double allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(double time, OsbColor value)
        {
            return new ColorCommand(0, this.Time, time, this.Value, value);
        }
    }
}