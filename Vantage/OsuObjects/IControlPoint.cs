namespace Vantage.OsuObjects
{
    public interface IControlPoint
    {
        double Time { get; set; }
        
        double Velocity { get; set; }
        
        int TimeSignature { get; set; }

        HitsoundSamplesetType HitsoundSamplesetType { get; set; }

        int HitsoundSamplesetIndex { get; set; }

        double HitsoundVolume { get; set; }

        int Type { get; set; }

        bool Kiai { get; set; }

        IControlPoint Parent { get; set; }

        bool IsTimingPoint { get; }

        double BeatDuration { get; }

        double Bpm { get; }
    }
}
