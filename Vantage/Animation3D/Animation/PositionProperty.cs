namespace Vantage.Animation3D.Animation
{
    using SharpDX;

    using Vantage.Animation3D.Animation.Keyframes;

    public class PositionProperty : AnimatableProperty<Keyframe<Vector3>, Vector3>
    {
        public PositionProperty(Vector3 initialValue)
            : base(initialValue)
        {
        }

        public PositionProperty()
            : this(Vector3.Zero)
        {
        }
    }
}