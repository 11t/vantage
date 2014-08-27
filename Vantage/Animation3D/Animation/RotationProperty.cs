namespace Vantage.Animation3D.Animation
{
    using SharpDX;

    using Vantage.Animation3D.Animation.Keyframes;

    public class RotationProperty : AnimatableProperty<QuaternionKeyframe, Quaternion>
    {
        public RotationProperty(Quaternion initialValue)
            : base(initialValue)
        {
        }

        public RotationProperty()
            : this(Quaternion.Identity)
        {
        }
    }
}