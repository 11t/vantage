namespace Vantage.OsuObjects
{
    using System.Linq;

    public abstract class HitObject : IHitObject
    {
        protected const int HitCircleBit = 0;

        protected const int SliderBit = 1;

        protected const int SpinnerBit = 3;

        private const int NewComboBit = 2;

        // 1110000
        private const int ComboColorShiftAmountMask = 112;

        private const int ComboColorShiftAmountMaskBitShift = 4;

        protected HitObject(
            double time,
            double x,
            double y,
            int typeBit,
            bool newCombo,
            int comboColorShiftAmount,
            HitsoundParameter hitsoundParameter,
            IControlPoint controlPoint)
        {
            this.Time = time;
            this.X = x;
            this.Y = y;
            this.TypeEncoding = 0.SetBit(typeBit);
            this.NewCombo = newCombo;
            this.ComboColorShiftAmount = comboColorShiftAmount;
            this.HitsoundParameter = hitsoundParameter;
            this.ControlPoint = controlPoint;
        }

        protected HitObject(int typeBit)
            : this(0, 0, 0, typeBit, false, 0, new HitsoundParameter(), null)
        {
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Time { get; set; }

        public bool NewCombo
        {
            get
            {
                return this.TypeEncoding.CheckBit(NewComboBit);
            }

            set
            {
                this.TypeEncoding = this.TypeEncoding.SetClearBit(NewComboBit, value);
            }
        }

        // Additional combo color shifting. 0 means that the combo color is shifted to the next combo color.
        // Only valid if NewCombo is true.
        public int ComboColorShiftAmount
        {
            get
            {
                return (this.TypeEncoding & ComboColorShiftAmountMask) >> ComboColorShiftAmountMaskBitShift;
            }

            set
            {
                int typeEncodingCleared = this.TypeEncoding & ~ComboColorShiftAmountMask;
                int valueMasked = value & (ComboColorShiftAmountMask >> ComboColorShiftAmountMaskBitShift);
                this.TypeEncoding = typeEncodingCleared | (valueMasked << ComboColorShiftAmountMaskBitShift);
            }
        }

        public HitsoundParameter HitsoundParameter { get; set; }

        public IControlPoint ControlPoint { get; set; }

        protected int TypeEncoding { get; set; }

        protected virtual void ReadFromOsuString(string osuString)
        {
            string[] data = osuString.Trim().Split(',');
            this.X = int.Parse(data[0]);
            this.Y = int.Parse(data[1]);
            this.Time = int.Parse(data[2]);
            this.TypeEncoding = int.Parse(data[3]);
            if (data.Last().Contains(':'))
            {
                this.HitsoundParameter = HitsoundParameter.FromOsuString(data.Last());
            }

            this.HitsoundParameter.Additionals = int.Parse(data[4]);
        }
    }
}
