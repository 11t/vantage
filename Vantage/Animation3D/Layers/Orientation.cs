namespace Vantage.Animation3D.Layers
{
    using System.Windows.Forms;

    using SharpDX;

    using Vantage.Animation3D.Animation;
    using Vantage.Animation3D.Animation.Keyframes;

    public interface IOrientation
    {
        Vector3 Forward { get; set; }

        Vector3 Up { get; set; }

        Vector3 Right { get; set; }

        ILayer TargetLayer { get; set; }
        
        Vector3 TargetPosition { get; set; }

        Quaternion Rotation { get; set; }
    }

    public class Orientation : IOrientation
    {
        private Quaternion? rotation;

        private Vector3? targetPosition;

        private ILayer targetLayer;

        public Orientation()
        {
            
        }

        public Vector3 Forward { get; set; }

        public Vector3 Up { get; set; }

        public Vector3 Right { get; set; }

        public ILayer TargetLayer { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Quaternion Rotation
        {
            get
            {
                if (this.TargetLayer != null)
                {
                    
                }

                if (this.targetPosition != null)
                {
                    
                }

                if (this.rotation != null)
                {
                    return this.rotation.Value;
                }

                return Quaternion.Identity;
            }

            set
            {
                this.rotation = value;
            }
        }

        public static Orientation Lerp(Orientation start, Orientation end, float amount)
        {
            //Vector3.Lerp(start.targetPosition, end.targetPosition, amount);
            return null;
        }
    }
}
