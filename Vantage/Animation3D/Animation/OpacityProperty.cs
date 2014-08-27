namespace Vantage.Animation3D.Animation
{
    using Vantage.Animation3D.Animation.Keyframes;

    public class OpacityProperty : AnimatableProperty<Keyframe<float>, float>
    {
        public OpacityProperty(float initialValue)
            : base(initialValue)
        {
        }

        public OpacityProperty()
            : this(1)
        {
        }
    }
}