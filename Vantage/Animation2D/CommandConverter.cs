namespace Vantage.Animation2D
{
    using Vantage.Animation2D.Commands.Generators;

    public static class CommandConverter
    {
        /// <summary>
        /// The generator used for creating Color commands from Sprite2DState objects.
        /// </summary>
        public static readonly ColorCommandGenerator ColorGenerator =
            new ColorCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ColorThreshold);

        /// <summary>
        /// The generator used for creating Fade commands from Sprite2DState objects.
        /// </summary>
        public static readonly FadeCommandGenerator FadeGenerator =
            new FadeCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.FadeThreshold);

        /// <summary>
        /// The generator used for creating Move commands from Sprite2DState objects.
        /// </summary>
        public static readonly MoveCommandGenerator MoveGenerator =
            new MoveCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold);

        /// <summary>
        /// The generator used for creating Rotate commands from Sprite2DState objects.
        /// </summary>
        public static readonly RotateCommandGenerator RotateGenerator =
            new RotateCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.RotateThreshold);

        /// <summary>
        /// The generator used for creating Scale and VScale commands from Sprite2DState objects.
        /// </summary>
        public static readonly ScaleCommandGenerator ScaleGenerator =
            new ScaleCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ScaleThreshold);
    }
}
