namespace Vantage
{
    using Vantage.Animation3D.Animation;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public class SceneConversionSettings
    {
        public SceneConversionSettings()
        {
            this.TimePrecision = 1;

            this.MoveThresholdProperty = new AnimatableProperty<Keyframe<double>, double>(0);
            this.RotateThreshold = 0;
            this.ScaleThreshold = 0;
            this.ColorThreshold = 0;
            this.FadeThreshold = 0;

            this.DefaultTextLetterSpacing = 0;
            this.DefaultTextSpaceWidth = 30;

            this.UseFloatForMove = false;
        }

        public bool UseFloatForMove { get; set; }

        public float TimePrecision { get; set; }

        public AnimatableProperty<Keyframe<double>, double> MoveThresholdProperty { get; private set; }

        public double MoveThreshold
        {
            get
            {
                return this.MoveThresholdProperty.CurrentValue;
            }
        }

        public float RotateThreshold { get; set; }

        public float ScaleThreshold { get; set; }

        public float ColorThreshold { get; set; }

        public float FadeThreshold { get; set; }

        public int DefaultTextLetterSpacing { get; set; }

        public int DefaultTextSpaceWidth { get; set; }

        public void SetMoveThreshold(double time, double value, IEasingCurve easingCurve)
        {
            this.MoveThresholdProperty.InsertKeyframe(time, value, easingCurve);
        }

        public void SetMoveThreshold(double time, double value)
        {
            this.SetMoveThreshold(time, value, BasicEasingCurve.Step);
        }

        public void UpdateToTime(double time)
        {
            this.MoveThresholdProperty.UpdateToTime(time);
        }
    }
}
