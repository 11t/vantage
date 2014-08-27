namespace Vantage
{
    using System.Collections.Generic;
    using System.Linq;

    public class BeatPattern
    {
        public BeatPattern(double beatsPerMeasure)
        {
            this.BeatsPerMeasure = beatsPerMeasure;
            this.Measures = new List<BeatMeasure>();
        }

        public BeatPattern()
            : this(4)
        {
        }

        public double BeatsPerMeasure { get; set; }

        public float TotalBeats
        {
            get
            {
                return (float)(this.Measures.Count * this.BeatsPerMeasure);
            }
        }

        public IList<BeatMeasure> Measures { get; private set; }

        public IEnumerable<float> AbsoluteBeats()
        {
            double beatSum = this.Measures[0].Delay;
            foreach (var measure in this.Measures)
            {
                foreach (double beatDuration in measure.BeatDurations)
                {
                    yield return (float)beatSum;
                    beatSum += beatDuration;
                }
            }
        }

        public IEnumerable<float> AbsoluteBeatsForMeasure(int measureIndex)
        {
            double beatOffset = measureIndex * this.BeatsPerMeasure;
            BeatMeasure measure = this.Measures[measureIndex];
            return measure.AbsoluteBeats().Select(absoluteBeat => (float)(absoluteBeat + beatOffset));
        }

        public void RepeatAddMeasure(BeatMeasure measure, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Measures.Add(measure);
            }
        }
    }
}
