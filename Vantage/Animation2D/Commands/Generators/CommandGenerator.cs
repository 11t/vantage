namespace Vantage.Animation2D.Commands.Generators
{
    using Vantage.Animation2D;
    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public abstract class CommandGenerator<TValue>
        where TValue : IOsbType
    {
        protected CommandGenerator(double allowedError)
        {
            this.AllowedError = allowedError;
            this.IssuedCommand = false;
        }

        public double Time { get; set; }

        public TValue Value { get; set; }

        public bool Visible { get; set; }

        public double AllowedError { get; set; }

        public bool IssuedCommand { get; set; }

        public virtual void Set(double time, TValue value, bool visible)
        {
            this.Time = time;
            this.Value = value;
            this.Visible = visible;
        }

        public abstract ICommand CreateCommand(double time, TValue value);

        public virtual ICommand Generate(double time, TValue value, bool visible)
        {
            ICommand command = default(ICommand);
            float distance = value.DistanceFrom(this.Value);
            if (distance > this.AllowedError)
            {
                if (visible || this.Visible)
                {
                    // If was visible and is now not, or was not visible and now is,
                    // or was visible and is still visible, perform a Move.
                    command = this.CreateCommand(time, value);
                    this.IssuedCommand = true;

                    // prevVisible = visible;
                }

                this.Value = value;
                this.Time = time;

                // Unnecessary: if this point is reached, then prevVisible == visible == false
                // prevVisible = visible;
            }
            else
            {
                // if the distance is within the tolerable screen error limit
                // if (position != prevPosition)
                if (distance != 0)
                {
                    // *Some* motion has occurred, but was within the error limit
                    // Depend on next states to approximate movement
                }
                else
                {
                    // Unnecessary: already true
                    // prevPosition = position;
                    this.Time = time;
                }
            }

            this.Visible = visible;
            return command;
        }
    }
}
