namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class FadeCommandGenerator : CommandGenerator<OsbDecimal>
    {
        public FadeCommandGenerator(float allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(float time, OsbDecimal value)
        {
            return new FadeCommand(0, this.Time, time, this.Value, value);
        }

        public override ICommand Generate(float time, OsbDecimal value, bool visible)
        {
            if (!visible && this.Visible)
            {
                ICommand command = this.CreateCommand(time, 0);
                this.Set(time, 0, false);
                return command;
            }

            if (visible && !this.Visible)
            {
                this.Value = 0;
                ICommand command = this.CreateCommand(time, value);
                this.Set(time, value, true);
                return command;
            }

            return base.Generate(time, value, visible);
        }
    }
}