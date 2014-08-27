namespace Vantage.Animation2D.OsbTypes
{
    public struct OsbParameter : IOsbType
    {
        public static readonly OsbParameter FlipHorizontal = new OsbParameter(ParameterType.FlipHorizontal);
        public static readonly OsbParameter FlipVertical = new OsbParameter(ParameterType.FlipVertical);
        public static readonly OsbParameter AdditiveBlending = new OsbParameter(ParameterType.AdditiveBlending);

        private readonly ParameterType type;

        private OsbParameter(ParameterType type)
        {
            this.type = type;
        }

        private enum ParameterType
        {
            FlipHorizontal,
            FlipVertical,
            AdditiveBlending
        }

        public string ToOsbString()
        {
            switch (this.type)
            {
                case ParameterType.FlipHorizontal:
                    return "H";
                case ParameterType.FlipVertical:
                    return "V";
                case ParameterType.AdditiveBlending:
                    return "A";
                default:
                    return string.Empty;
            }
        }

        public float DistanceFrom(object obj)
        {
            return 0;
        }
    }
}
