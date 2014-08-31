namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class MoveCommandGenerator : CommandGenerator<OsbPosition>
    {
        public MoveCommandGenerator(double allowedError)
            : base(allowedError)
        {
            this.DidMove = false;
        }

        public bool DidMove { get; set; }

        public override ICommand CreateCommand(double time, OsbPosition value)
        {
            return new MoveCommand(0, this.Time, time, this.Value, value);
        }

        public override ICommand Generate(double time, OsbPosition value, bool visible)
        {
            /*
            ICommand command = null;
            if (!visible && this.Visible && !DidMove)
            {
                command = this.CreateCommand(time, value);
                this.Time = time;
                this.Value = value;
                this.Visible = false;
                this.DidMove = false;
                return command;
            }
            command = base.Generate(time, value, visible);
            if (command != null)
            {
                DidMove = true;
            }
            return command;
            */
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