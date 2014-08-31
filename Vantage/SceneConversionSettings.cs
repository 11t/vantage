namespace Vantage
{
    public class SceneConversionSettings
    {
        public SceneConversionSettings()
        {
            this.TimePrecision = 1;

            this.MoveThreshold = 0;
            this.RotateThreshold = 0;
            this.ScaleThreshold = 0;
            this.ColorThreshold = 0;
            this.FadeThreshold = 0;

            this.DefaultTextLetterSpacing = 0;
            this.DefaultTextSpaceWidth = 30;
        }

        public float TimePrecision { get; set; }

        public float MoveThreshold { get; set; }

        public float RotateThreshold { get; set; }

        public float ScaleThreshold { get; set; }

        public float ColorThreshold { get; set; }

        public float FadeThreshold { get; set; }

        public int DefaultTextLetterSpacing { get; set; }

        public int DefaultTextSpaceWidth { get; set; }
    }
}
