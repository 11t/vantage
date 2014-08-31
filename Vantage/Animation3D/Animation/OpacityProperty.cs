namespace Vantage.Animation3D.Animation
{
    using Vantage.Animation3D.Animation.Keyframes;

    public class OpacityProperty : AnimatableProperty<Keyframe<double>, double>
    {
        public OpacityProperty(double initialValue)
            : base(initialValue)
        {
        }

        public OpacityProperty()
            : this(1)
        {
        }
    }
}