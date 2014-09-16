namespace Vantage.OsuObjects
{
    using System;

    public class ControlPoint : IControlPoint
    {
        public ControlPoint(
            double time,
            double velocity,
            int timeSignature,
            HitsoundSamplesetType hitsoundSamplesetType,
            int hitsoundSampleIndex,
            double hitsoundVolume,
            int type,
            bool kiai,
            IControlPoint parent)
        {
            this.Time = time;
            this.Velocity = velocity;
            this.TimeSignature = timeSignature;
            this.HitsoundSamplesetType = hitsoundSamplesetType;
            this.HitsoundSamplesetIndex = hitsoundSampleIndex;
            this.HitsoundVolume = hitsoundVolume;
            this.Type = type;
            this.Kiai = kiai;
            this.Parent = parent;
        }

        public double Time { get; set; }

        public double Velocity { get; set; }

        public int TimeSignature { get; set; }

        public HitsoundSamplesetType HitsoundSamplesetType { get; set; }

        public int HitsoundSamplesetIndex { get; set; }

        public double HitsoundVolume { get; set; }

        public int Type { get; set; }

        public bool Kiai { get; set; }

        public IControlPoint Parent { get; set; }

        public bool IsTimingPoint
        {
            get
            {
                return this.Type == 1;
            }
        }

        public double BeatDuration
        {
            get
            {
                return this.IsTimingPoint ? this.Velocity : this.Parent.Velocity;
            }
        }

        public double Bpm
        {
            get
            {
                return 60000.0 / this.BeatDuration;
            }
        }

        public static ControlPoint FromOsuString(string osuString)
        {
            string[] data = osuString.Trim().Split(',');
            double time = double.Parse(data[0]);
            double velocity = double.Parse(data[1]);
            int timeSignature = int.Parse(data[2]);
            var hitsoundSamplesetType = (HitsoundSamplesetType)int.Parse(data[3]);
            int hitsoundSamplesetIndex = int.Parse(data[4]);
            double hitsoundVolume = double.Parse(data[5]);
            int type = int.Parse(data[6]);
            bool kiai = Convert.ToBoolean(int.Parse(data[7]));
            return new ControlPoint(
                time,
                velocity,
                timeSignature,
                hitsoundSamplesetType,
                hitsoundSamplesetIndex,
                hitsoundVolume,
                type,
                kiai,
                null);
        }
    }
}
