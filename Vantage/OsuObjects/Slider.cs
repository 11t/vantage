namespace Vantage.OsuObjects
{
    using System;
    using System.Collections.Generic;

    public enum SliderCurveType
    {
        L,
        P,
        B
    }

    public class Slider : HitObject
    {
        public Slider()
            : base(HitObject.SliderBit)
        {
            this.CurveType = SliderCurveType.L;
            this.AnchorPoints = new List<AnchorPoint>();
            this.HitsoundParameters = new List<HitsoundParameter>();
        }

        public SliderCurveType CurveType { get; set; }

        public IList<AnchorPoint> AnchorPoints { get; private set; }

        public IList<HitsoundParameter> HitsoundParameters { get; private set; }

        public int RepeatAmount { get; set; }

        public double Length { get; set; }

        public static Slider FromOsuString(string osuString)
        {
            Slider slider = new Slider();
            slider.ReadFromOsuString(osuString);
            return slider;
        }

        protected override void ReadFromOsuString(string osuString)
        {
            base.ReadFromOsuString(osuString);
            string[] data = osuString.Trim().Split(',');
            string[] anchorPointsData = data[5].Split('|');
            this.CurveType = (SliderCurveType)Enum.Parse(typeof(SliderCurveType), anchorPointsData[0]);

            int lastX = -1;
            int lastY = -1;
            AnchorPoint lastAnchorPoint = null;
            for (int i = 1; i < anchorPointsData.Length; i++)
            {
                string[] coordinateData = anchorPointsData[i].Split(':');
                int x = int.Parse(coordinateData[0]);
                int y = int.Parse(coordinateData[1]);

                if (x == lastX && y == lastY && lastAnchorPoint != null)
                {
                    lastAnchorPoint.Type = AnchorPointType.Sharp;
                }
                else
                {
                    AnchorPoint anchorPoint = new AnchorPoint(x, y, AnchorPointType.Smooth);
                    this.AnchorPoints.Add(anchorPoint);
                    lastX = x;
                    lastY = y;
                    lastAnchorPoint = anchorPoint;
                }
            }

            this.RepeatAmount = int.Parse(data[6]);
            this.Length = double.Parse(data[7]);
            if (data.Length > 8)
            {
                string[] hitsoundAdditionsData = data[8].Split('|');
                string[] hitsoundSamplesetTypesData = data[9].Split('|');
                for (int j = 0; j < hitsoundAdditionsData.Length; j++)
                {
                    var hitsoundParameter = new HitsoundParameter();
                    this.HitsoundParameter.Additionals = int.Parse(hitsoundAdditionsData[j]);
                    string[] hitsoundSamplesetTypes = hitsoundSamplesetTypesData[j].Split(':');
                    hitsoundParameter.BaseSamplesetType = (HitsoundSamplesetType)int.Parse(hitsoundSamplesetTypes[0]);
                    hitsoundParameter.AdditionalSamplesetType =
                        (HitsoundSamplesetType)int.Parse(hitsoundSamplesetTypes[1]);
                    this.HitsoundParameters.Add(hitsoundParameter);
                }
            }
            else
            {
                for (int j = 0; j < this.RepeatAmount + 1; j++)
                {
                    var hitsoundParameter = new HitsoundParameter
                                                {
                                                    Additionals = 0,
                                                    BaseSamplesetType = HitsoundSamplesetType.Auto,
                                                    AdditionalSamplesetType = HitsoundSamplesetType.Auto
                                                };
                    this.HitsoundParameters.Add(hitsoundParameter);
                }
            }
        }
    }
}
