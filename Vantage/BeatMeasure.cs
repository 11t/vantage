namespace Vantage
{
    using System.Collections.Generic;
    using System.Linq;

    public class BeatMeasure
    {
        private readonly IList<double> beatDurations;
 
        public BeatMeasure(double delay, params double[] beatDurations)
        {
            this.Delay = delay;
            this.beatDurations = beatDurations.ToList();
        }

        public IEnumerable<double> BeatDurations
        {
            get
            {
                return this.beatDurations;
            }
        }

        public double Delay { get; set; }

        public IEnumerable<double> AbsoluteBeats()
        {
            double beatSum = this.Delay;
            foreach (double beatDuration in this.BeatDurations)
            {
                yield return beatSum;
                beatSum += beatDuration;
            }
        }
    }
}
