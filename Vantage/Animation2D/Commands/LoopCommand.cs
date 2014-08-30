namespace Vantage.Animation2D.Commands 
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class LoopCommand : CommandGroup 
    {
        public LoopCommand(float startTime, int loopCount) 
        {
            this.StartTime = startTime;
            this.LoopCount = loopCount;
        }

        public override float EndTime 
        {
            get 
            {
                return this.StartTime + (this.GetCommandsEndTime() * this.LoopCount);
            }

            set 
            {
                this.LoopCount = (int)Math.Floor((value - this.StartTime) / this.GetCommandsEndTime());
            }
        }

        public int LoopCount { get; set; }

        protected override string GetOsbStringHeader() 
        {
            string startTimeString = ((int)this.StartTime).ToString(CultureInfo.InvariantCulture);
            string loopCountString = ((int)this.LoopCount).ToString(CultureInfo.InvariantCulture);

            string[] headerArray = { "L", startTimeString, loopCountString };
            return string.Join(",", headerArray);
        }
    }
}
