namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class MoveYCommandGenerator : CommandGenerator<OsbDecimal>
    {
        public MoveYCommandGenerator(double allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(double time, OsbDecimal value)
        {
            return new MoveYCommand(0, this.Time, time, this.Value, value);
        }

        public override ICommand Generate(double time, OsbDecimal value, bool visible)
        {
            if (!visible && this.Visible)
            {
                ICommand command = this.CreateCommand(time, value);
                this.Set(time, value, false);
                this.IssuedCommand = true;
                return command;
            }

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