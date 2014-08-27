namespace Vantage
{
    public class SceneConversionSettings
    {
        public SceneConversionSettings()
        {
            this.MoveThreshold = 8.0f;
            this.RotateThreshold = 0.01f;
            this.ScaleThreshold = 0.00f;
            this.ColorThreshold = 1.0f;
            this.FadeThreshold = 0;
            this.DefaultTextLetterSpacing = 0;
            this.DefaultTextSpaceWidth = 30;
        }

        public float MoveThreshold { get; set; }

        public float RotateThreshold { get; set; }

        public float ScaleThreshold { get; set; }

        public float ColorThreshold { get; set; }

        public float FadeThreshold { get; set; }

        public int DefaultTextLetterSpacing { get; set; }

        public int DefaultTextSpaceWidth { get; set; }
    }
}
