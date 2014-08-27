namespace Vantage.Animation3D.Animation
{
    using SharpDX;

    using Vantage.Animation3D.Animation.Keyframes;

    public class ScaleProperty : AnimatableProperty<Keyframe<Vector3>, Vector3>
    {
        public ScaleProperty(Vector3 initialValue)
            : base(initialValue)
        {
        }

        public ScaleProperty()
            : this(Vector3.One)
        {
        }
    }
}