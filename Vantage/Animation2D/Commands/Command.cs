namespace Vantage.Animation2D.Commands
{
    using System.Globalization;

    using Vantage.Animation2D.OsbTypes;

    public abstract class Command<T> : ICommand
        where T : IOsbType
    {
        protected Command(string identifier, int easing, double startTime, double endTime, T startValue, T endValue)
        {
            this.Identifier = identifier;
            this.Easing = easing;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.StartValue = startValue;
            this.EndValue = endValue;
        }

        public string Identifier { get; set; }

        public int Easing { get; set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public T StartValue { get; set; }

        public T EndValue { get; set; }

        public virtual string ToOsbString()
        {
            string startTimeString = ((int)this.StartTime).ToString(CultureInfo.InvariantCulture);
            string endTimeString = ((int)this.EndTime).ToString(CultureInfo.InvariantCulture);
            string startValueString = this.StartValue.ToOsbString();
            string endValueString = this.EndValue.ToOsbString();
            if (startTimeString == endTimeString)
            {
                endTimeString = string.Empty;
            }

            string[] stringArray =
                {
                    this.Identifier, this.Easing.ToString(CultureInfo.InvariantCulture),
                    startTimeString, endTimeString, startValueString
                };
            string result = string.Join(",", stringArray);
            if (startValueString != endValueString)
            {
                result += "," + endValueString;
            }

            return result;
        }
    }
}
