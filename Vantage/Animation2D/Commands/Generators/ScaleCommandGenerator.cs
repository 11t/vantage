namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class ScaleCommandGenerator : CommandGenerator<OsbScale>
    {
        public ScaleCommandGenerator(double allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(double time, OsbScale value)
        {
            if (this.Value.X == this.Value.Y && value.X == value.Y)
            {
                return new ScaleCommand(0, this.Time, time, this.Value.X, value.X);
            }

            return new VScaleCommand(0, this.Time, time, this.Value, value);
        }

        public override ICommand Generate(double time, OsbScale value, bool visible)
        {
            if (visible && !this.Visible)
            {
                ICommand command = this.CreateCommand(time, value);
                this.Set(time, value, true);
                this.IssuedCommand = true;
                return command;
            }

            return base.Generate(time, value, visible);
        }
    }
}