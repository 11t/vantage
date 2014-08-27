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

        /*
        public BeatPattern(double measureLength)
        {
            this.MeasureLength = (float)measureLength;
            this.RelativeBeats = new List<float>();
        }

        public BeatPattern()
            : this(4)
        {
        }

        public float MeasureLength { get; set; }

        public IList<float> RelativeBeats { get; set; }

        public double AbsoluteBeatAtIndex(int index)
        {
            double sum = 0;
            for (int i = 0; i < index; i++)
            {
                sum += this.RelativeBeats[i];
            }
            return sum;
        }

        public void AddRepeat(IList<double> beatList, int count)
        {
            for (int i = 0; i < count; i++)
            {
                foreach (double beat in beatList)
                {
                    this.RelativeBeats.Add((float)beat);
                }
            }
        }

        public int BeatIndexForMeasure(int measureIndex)
        {
            int k = 0;
            double beatSum = 0;
            for (int i = 0; i < measureIndex; i++)
            {
                for (int j = k; j < this.RelativeBeats.Count; j++)
                {
                    beatSum += this.RelativeBeats[j];
                    if (beatSum > this.MeasureLength)
                    {
                        beatSum -= this.MeasureLength;
                        k = j;
                        break;
                    }
                }
            }

            return k;
        }

        public int LastIndexForMeasureStartingAtBeatIndex(int beatIndex)
        {
            float beatSum = 0;
            for (int i = beatIndex; i < this.RelativeBeats.Count; i++)
            {
                beatSum += this.RelativeBeats[i];
                if (beatSum > this.MeasureLength)
                {
                    return i;
                }
            }

            return beatIndex;
        }

        public void ReplaceMeasure(int measureIndex, IList<double> beatList)
        {
            int i = this.BeatIndexForMeasure(measureIndex);
            int j = this.LastIndexForMeasureStartingAtBeatIndex(i);

            for (int k = i; k < j; k++)
            {
                this.RelativeBeats.RemoveAt(i);
            }

            foreach (double beat in beatList)
            {
                this.RelativeBeats.Insert(i++, (float)beat);
            }
        }

        public IEnumerable<int> BeatIndicesForMeasure(int measureIndex)
        {
            int i = this.BeatIndexForMeasure(measureIndex);
            int j = this.LastIndexForMeasureStartingAtBeatIndex(i);
            for (int k = 0; k < j - i; k++)
            {
                yield return k;
            }
        }

        public IEnumerable<float> BeatsForMeasure(int measureIndex)
        {
            int i = this.BeatIndexForMeasure(measureIndex);
            int j = this.LastIndexForMeasureStartingAtBeatIndex(i);
            for (int k = 0; k < j - i; k++)
            {
                yield return this.RelativeBeats[k];
            }
        }*/
    }
}
