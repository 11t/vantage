namespace Vantage.Animation3D.Layers
{
    using System;

    using SharpDX;

    using Vantage.Animation3D.Animation;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public interface ICamera
    {
    }

    public class Camera : Layer, ICamera
    {
        private readonly Vector2 resolution;
        private readonly float aspectRatio;

        private float horizontalFieldOfView;
        private float verticalFieldOfView;
        private float focalLength;
        private float nearPlaneDistance;
        private float farPlaneDistance;
        private float nearPlaneWidth;

        public Camera(float width, float height)
        {
            this.resolution = new Vector2(width, height);
            this.aspectRatio = width / height;
            this.HorizontalFieldOfView = 60.0f;
            this.NearPlaneDistance = 10.0f;
            this.FarPlaneDistance = 2000.0f;
            this.TargetProperty = new AnimatableProperty<Keyframe<Vector3>, Vector3>(Vector3.ForwardRH);
        }

        public AnimatableProperty<Keyframe<Vector3>, Vector3> TargetProperty { get; private set; } 

        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public Matrix ViewProjection { get; private set; }
        
        public Vector3 Target
        {
            get
            {
                if (this.WorldRotation != Quaternion.Identity)
                {
                    return this.Forward + this.WorldPosition;
                }

                return this.TargetProperty.CurrentValue;
            }
        }

        public Vector2 Resolution
        {
            get { return this.resolution; }
        }

        public float Width
        {
            get { return this.Resolution.X; }
        }

        public float Height
        {
            get { return this.Resolution.Y; }
        }

        public float AspectRatio
        {
            get { return this.aspectRatio; }
        }

        public float HorizontalFieldOfView
        {
            get
            {
                return this.horizontalFieldOfView;
            }

            set
            {
                this.horizontalFieldOfView = value;
                this.verticalFieldOfView = VerticalFromHorizontalFieldOfView(this.HorizontalFieldOfView, this.AspectRatio);
                this.focalLength = FocalLengthFromFieldOfView(this.HorizontalFieldOfView);
                this.nearPlaneWidth = this.Resolution.X * this.FocalLength * 0.5f;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float VerticalFieldOfView
        {
            get { return this.verticalFieldOfView; }
        }

        public float NearPlaneDistance
        {
            get
            {
                return this.nearPlaneDistance;
            }

            set
            {
                this.nearPlaneDistance = value;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float FarPlaneDistance
        {
            get
            {
                return this.farPlaneDistance;
            }

            set
            {
                this.farPlaneDistance = value;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float FocalLength
        {
            get { return this.focalLength; }
        }

        public float NearPlaneWidth
        {
            get { return this.nearPlaneWidth; }
        }

        public bool ViewNeedsUpdate { get; set; }

        public bool ProjectionNeedsUpdate { get; set; }

        public void UpdateView()
        {
            this.View = Matrix.LookAtRH(this.WorldPosition, this.Target, this.Up);
            this.ViewNeedsUpdate = false;
        }

        public void UpdateProjection()
        {
            this.Projection = Matrix.PerspectiveFovRH(
                MathUtil.DegreesToRadians(this.VerticalFieldOfView),
                this.AspectRatio,
                this.NearPlaneDistance,
                this.FarPlaneDistance);
            this.ProjectionNeedsUpdate = false;
        }

        /*
        public void SetTargetLayer(float time, ILayer targetLayer, IEasingCurve easingCurve)
        {
            Rotation.UpdateToTime(time);
            this.Root.UpdateToTime(this.Rotation.CurrentKeyframe != null ? this.Rotation.CurrentKeyframe.Time : time);

            Vector3 target = targetLayer.WorldPosition;
            Vector3 desiredForward = Vector3.Normalize(target - WorldPosition);
            Vector3 desiredUp = targetLayer.Up;

            Quaternion rotationFromCurrentToTarget = Math3D.RotationFromTo(this.Forward, desiredForward);
            Quaternion rotation = Quaternion.Normalize(this.LocalRotation * rotationFromCurrentToTarget);

            Vector3 currentUp = Vector3.TransformNormal(Vector3.Up, Matrix.RotationQuaternion(rotation));
            Quaternion rotationToUp = Math3D.RotationFromTo(currentUp, desiredUp);

            this.SetRotation(time, Quaternion.Normalize(rotation * rotationToUp), easingCurve);
        }

        public void SetTarget(float time, Vector3 target, IEasingCurve easingCurve)
        {
            Rotation.UpdateToTime(time);
            this.Root.UpdateToTime(this.Rotation.CurrentKeyframe != null ? this.Rotation.CurrentKeyframe.Time : time);

            Vector3 desiredForward = Vector3.Normalize(target - WorldPosition);

            Quaternion rotationFromCurrentToTarget = Math3D.RotationFromTo(this.Forward, desiredForward);
            Quaternion rotation = Quaternion.Normalize(this.LocalRotation * rotationFromCurrentToTarget);
            
            this.SetRotation(time, rotation, easingCurve);
        }

        public void SetTarget(float time, float x, float y, float z, IEasingCurve easingCurve)
        {
            this.SetTarget(time, new Vector3(x, y, z), easingCurve);
        }

        public void SetTarget(float time, float x, float y, float z)
        {
            this.SetTarget(time, x, y, z, BasicEasingCurve.Linear);
        }
        */

        public void SetTarget(float time, Vector3 target)
        {
            this.SetTarget(time, target, BasicEasingCurve.Linear);
        }

        public void SetTarget(float time, Vector3 target, IEasingCurve easingCurve)
        {
            this.TargetProperty.InsertKeyframe(time, target, easingCurve);
        }

        public void SetTarget(float time, float x, float y, float z, IEasingCurve easingCurve)
        {
            this.SetTarget(time, new Vector3(x, y, z), easingCurve);
        }

        public void SetTarget(float time, float x, float y, float z)
        {
            this.SetTarget(time, x, y, z, BasicEasingCurve.Linear);
        }

        public override void UpdateToTime(float time)
        {
            // TODO: UpdateProjection() does not always need to be called
            base.UpdateToTime(time);
            this.TargetProperty.UpdateToTime(time);
            this.UpdateView();
            this.UpdateProjection();
            this.ViewProjection = this.View * this.Projection;
        }

        public Vector2 NormalizedDeviceCoordinates(Vector3 worldPosition)
        {
            Vector4 worldCoordinates = new Vector4(this.WorldPosition, 1.0f);
            Vector4 clipCoordinates = Vector4.Transform(worldCoordinates, this.ViewProjection);
            Vector2 ndc = new Vector2(clipCoordinates.X, -clipCoordinates.Y) / clipCoordinates.W;
            return ndc;
        }

        private static float FocalLengthFromFieldOfView(float degrees)
        {
            return (float)(1.0f / Math.Tan(MathUtil.DegreesToRadians(degrees / 2.0f)));
        }

        private static float VerticalFromHorizontalFieldOfView(float degrees, float aspectRatio)
        {
            float horizontalRadians = MathUtil.DegreesToRadians(degrees);
            float verticalRadians = (float)Math.Atan(Math.Tan(horizontalRadians * 0.5) / aspectRatio) * 2.0f;
            return MathUtil.RadiansToDegrees(verticalRadians);
        }
    }
}
