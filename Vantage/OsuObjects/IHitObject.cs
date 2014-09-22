namespace Vantage.OsuObjects
{
    using Vantage.Animation2D.OsbTypes;

    public interface IHitObject
    {
        OsuBeatmap Beatmap { get; set; }

        double X { get; set; }
        
        double Y { get; set; }

        double Time { get; set; }

        bool NewCombo { get; set; }
        
        int ComboColorShiftAmount { get; set; }

        HitsoundParameter HitsoundParameter { get; set; }

        IControlPoint ControlPoint { get; set; }

        OsbColor Color { get; set; }
    }
}
