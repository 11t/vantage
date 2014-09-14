namespace Vantage.OsuObjects
{
    public class Spinner : HitObject
    {
        public Spinner()
            : base(HitObject.SpinnerBit)
        {
            this.EndTime = 0;
        }

        public double EndTime { get; set; }

        public static Spinner FromOsuString(string osuString)
        {
            Spinner spinner = new Spinner();
            spinner.ReadFromOsuString(osuString);
            return spinner;
        }

        protected override void ReadFromOsuString(string osuString)
        {
            base.ReadFromOsuString(osuString);
            this.EndTime = int.Parse(osuString.Trim().Split(',')[5]);
        }
    }
}
