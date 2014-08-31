namespace Vantage.Animation3D.Animation
{
    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.Keyframes;

    public class ColorProperty : AnimatableProperty<Keyframe<OsbColor>, OsbColor>
    {
        public ColorProperty(OsbColor initialValue)
            : base(initialValue)
        {
        }

        public ColorProperty()
            : this(OsbColor.White)
        {
        }
    }
}