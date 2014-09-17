namespace Vantage.Animation2D.Commands.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    public class MoveXCommandGenerator : CommandGenerator<OsbDecimal>
    {
        public MoveXCommandGenerator(double allowedError)
            : base(allowedError)
        {
            this.PreviousTimes = new List<double>();
            this.PreviousValues = new List<OsbDecimal>();
        }

        public IList<double> PreviousTimes { get; set; }

        public IList<OsbDecimal> PreviousValues { get; set; }

        protected void ClearPrevious()
        {
            this.PreviousTimes.Clear();
            this.PreviousValues.Clear();
        }

        public override void Set(double time, OsbDecimal value, bool visible)
        {
            this.PreviousTimes.Add(time);
            this.PreviousValues.Add(value);
            this.Time = time;
            this.Value = value;
            this.Visible = visible;
        }

        public override ICommand CreateCommand(double time, OsbDecimal value)
        {
            return new MoveXCommand(0, this.Time, time, this.Value, value);
        }

        public override ICommand Generate(double time, OsbDecimal value, bool visible)
        {
            ICommand command = default(ICommand);

            if (!visible && this.Visible)
            {
                command = this.CreateCommand(time, value);
                this.Set(time, value, false);
                this.IssuedCommand = true;
                return command;
            }

            if (visible && !this.Visible)
            {
                command = this.CreateCommand(time, value);
                this.Set(time, value, true);
                this.IssuedCommand = true;
                return command;
            }

            return base.Generate(time, value, visible);
            
            double previousTime = this.PreviousTimes[this.PreviousTimes.Count - 1];
            OsbDecimal previousValue = this.PreviousValues[this.PreviousValues.Count - 1];

            this.PreviousTimes.Add(time);
            this.PreviousValues.Add(value);

            Tuple<double, double> slopeInterceptTuple = Math3D.LinearLeastSquares(
                this.PreviousTimes,
                this.PreviousValues);

            double sumSquaresError = Math3D.SumSquaresError(
                this.PreviousTimes,
                this.PreviousValues,
                slopeInterceptTuple.Item1,
                slopeInterceptTuple.Item2);

            Debug.WriteLine(time + "," + sumSquaresError + "," + slopeInterceptTuple.Item1 + "," + slopeInterceptTuple.Item2);

            if (sumSquaresError > this.AllowedError)
            {
                if (visible || this.Visible)
                {
                    // If was visible and is now not, or was not visible and now is,
                    // or was visible and is still visible, perform a Move.
                    command = this.CreateCommand(previousTime, previousValue);
                    this.IssuedCommand = true;

                    this.ClearPrevious();
                    this.PreviousTimes.Add(time);
                    this.PreviousValues.Add(value);

                    // prevVisible = visible;
                }

                this.Value = previousValue;
                this.Time = previousTime;

                // Unnecessary: if this point is reached, then prevVisible == visible == false
                // prevVisible = visible;
            }

            this.Visible = visible;
            return command;
        }
    }
}