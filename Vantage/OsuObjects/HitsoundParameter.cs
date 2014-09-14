namespace Vantage.OsuObjects
{
    public class HitsoundParameter
    {
        private const int WhistleBit = 1;

        private const int FinishBit = 2;

        private const int ClapBit = 3;

        public HitsoundParameter(
            int additionals,
            HitsoundSamplesetType baseSamplesetType,
            HitsoundSamplesetType additionalSamplesetType,
            int samplesetIndex,
            double customSampleVolume,
            string customSampleFilename)
        {
            this.Additionals = additionals;
            this.BaseSamplesetType = baseSamplesetType;
            this.AdditionalSamplesetType = additionalSamplesetType;
            this.SamplesetIndex = samplesetIndex;
            this.CustomSampleVolume = customSampleVolume;
            this.CustomSampleFilename = customSampleFilename;
        }

        public HitsoundParameter()
            : this(0, HitsoundSamplesetType.Auto, HitsoundSamplesetType.Auto, 0, 0, null)
        {
        }

        public int Additionals { get; set; }

        public bool Whistle
        {
            get
            {
                return this.Additionals.CheckBit(WhistleBit);
            }

            set
            {
                this.Additionals = this.Additionals.SetClearBit(WhistleBit, value);
            }
        }

        public bool Finish
        {
            get
            {
                return this.Additionals.CheckBit(FinishBit);
            }

            set
            {
                this.Additionals = this.Additionals.SetClearBit(FinishBit, value);
            }
        }

        public bool Clap
        {
            get
            {
                return this.Additionals.CheckBit(ClapBit);
            }

            set
            {
                this.Additionals = this.Additionals.SetClearBit(ClapBit, value);
            }
        }

        public HitsoundSamplesetType BaseSamplesetType { get; set; }

        public HitsoundSamplesetType AdditionalSamplesetType { get; set; }

        public int SamplesetIndex { get; set; }

        public double CustomSampleVolume { get; set; }

        public string CustomSampleFilename { get; set; }

        public static HitsoundParameter FromOsuString(string osuString)
        {
            osuString = osuString.Trim();
            string[] data = osuString.Split(':');
            HitsoundSamplesetType baseSamplesetType = (HitsoundSamplesetType)int.Parse(data[0]);
            HitsoundSamplesetType additionalSamplesetType = (HitsoundSamplesetType)int.Parse(data[1]);
            int samplesetIndex = int.Parse(data[2]);
            double customSampleVolume = double.Parse(data[3]);
            string customSampleFilename = data[4];
            return new HitsoundParameter(
                0,
                baseSamplesetType,
                additionalSamplesetType,
                samplesetIndex,
                customSampleVolume,
                customSampleFilename);
        }
    }
}
