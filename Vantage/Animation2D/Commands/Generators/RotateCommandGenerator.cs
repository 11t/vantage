namespace Vantage.Animation2D.Commands.Generators
{
    using System;

    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class RotateCommandGenerator : CommandGenerator<OsbDecimal>
    {
        public RotateCommandGenerator(double allowedError)
            : base(allowedError)
        {
        }

        public override ICommand CreateCommand(double time, OsbDecimal value)
        {
            var difference = value - this.Value;
            int sign = difference < 0 ? -1 : 1;
            if (Math.Abs(difference) > Math.PI)
            {
                sign = sign * -1;
            }

            double distance = Math3D.AngleDistance(this.Value, value);
            OsbDecimal endValue = this.Value + (sign * distance);
            return new RotateCommand(0, this.Time, time, this.Value, endValue);
        }

        public override ICommand Generate(double time, OsbDecimal value, bool visible)
        {
            if (!visible && this.Visible)
            {
                ICommand command = this.CreateCommand(time, value);
                this.Set(time, value, false);
                return command;
            }

            if (visible && !this.Visible)
            {
                ICommand command = this.CreateCommand(time, value);
                this.Set(time, value, true);
                return command;
            }

            return base.Generate(time, value, visible);
        }
    }
}