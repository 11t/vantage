namespace Vantage.Animation2D.Commands
{
    using System;
    using System.Globalization;

    using Vantage.Animation2D.OsbTypes;

    public class MoveCommand : Command<OsbPosition>
    {
        public MoveCommand(int easing, double startTime, double endTime, OsbPosition startValue, OsbPosition endValue)
            : base("M", easing, startTime, endTime, startValue, endValue)
        {
        }

        /*
        public override string ToOsbString()
        {
            string startTimeString = ((int)this.StartTime).ToString(CultureInfo.InvariantCulture);
            string endTimeString = ((int)this.EndTime).ToString(CultureInfo.InvariantCulture);
            string startX = Math.Round(this.StartValue.X, 4).ToString();
            var startY = Math.Round(this.StartValue.Y, 4).ToString();
            var endX = Math.Round(this.EndValue.X, 4).ToString();
            var endY = Math.Round(this.EndValue.Y, 4).ToString();

            string[] stringArray =
                {
                    "MX", this.Easing.ToString(CultureInfo.InvariantCulture),
                    startTimeString, endTimeString, startX, endX
                };
            string result = string.Join(",", stringArray);

            string[] stringArray2 = { "MY", this.Easing.ToString(), startTimeString, endTimeString, startY, endY };
            var result2 = string.Join(",", stringArray2);

            return result + "\n " + result2;
        }
        */
    }
}