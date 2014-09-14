namespace Vantage.OsuObjects
{
    public enum AnchorPointType
    {
        Smooth,
        Sharp
    }

    public class AnchorPoint
    {
        public AnchorPoint(int x, int y, AnchorPointType type)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public AnchorPointType Type { get; set; }
    }
}
