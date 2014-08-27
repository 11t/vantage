namespace Vantage.Animation3D.Animation
{
    using SharpDX;

    using Vantage.Animation3D.Animation.Keyframes;

    public class ColorProperty : AnimatableProperty<Keyframe<Vector3>, Vector3>
    {
        public ColorProperty(Vector3 initialValue)
            : base(initialValue)
        {
        }

        public ColorProperty()
            : this(Vector3.One)
        {
        }
    }
}