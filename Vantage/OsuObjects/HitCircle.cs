namespace Vantage.OsuObjects
{
    public class HitCircle : HitObject
    {
        public HitCircle()
            : base(HitObject.HitCircleBit)
        {
        }

        public static HitCircle FromOsuString(string osuString)
        {
            HitCircle hitCircle = new HitCircle();
            hitCircle.ReadFromOsuString(osuString);
            return hitCircle;
        }
    }
}
