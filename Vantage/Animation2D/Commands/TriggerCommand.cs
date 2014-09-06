namespace Vantage.Animation2D.Commands 
{
    using System.Collections.Generic;
    using System.Globalization;

    public class TriggerCommand : CommandGroup 
    {
        public TriggerCommand(string triggerName, double startTime, double endTime) 
        {
            this.TriggerName = triggerName;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public string TriggerName { get; set; }

        protected override string GetOsbStringHeader() 
        {
            string startTimeString = ((int)this.StartTime).ToString(CultureInfo.InvariantCulture);
            string endTimeString = ((int)this.EndTime).ToString(CultureInfo.InvariantCulture);

            string[] headerArray = { "T", this.TriggerName, startTimeString, endTimeString };
            return string.Join(",", headerArray);
        }
    }
}
