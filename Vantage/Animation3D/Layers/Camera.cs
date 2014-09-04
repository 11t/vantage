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
        private readonly double aspectRatio;

        private double horizontalFieldOfView;
        private double verticalFieldOfView;
        private double focalLength;
        private double nearPlaneDistance;
        private double farPlaneDistance;
        private double nearPlaneWidth;

        public Camera(double width, double height)
        {
            this.resolution = new Vector2((float)width, (float)height);
            this.aspectRatio = width / height;
            this.HorizontalFieldOfView = 60;
            this.NearPlaneDistance = 10;
            this.FarPlaneDistance = 2000;
            this.TargetProperty = new AnimatableProperty<NullableKeyframe<Vector3>, Vector3?>(null);
        }

        public AnimatableProperty<NullableKeyframe<Vector3>, Vector3?> TargetProperty { get; private set; } 

        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public Matrix ViewProjection { get; private set; }
        
        public Vector3 Target
        {
            get
            {
                if (this.TargetProperty.CurrentValue != null)
                {
                    return this.TargetProperty.CurrentValue.Value;
                }

                return this.Forward + this.WorldPosition;
            }
        }

        public Vector2 Resolution
        {
            get { return this.resolution; }
        }

        public double Width
        {
            get { return this.Resolution.X; }
        }

        public double Height
        {
            get { return this.Resolution.Y; }
        }

        public double AspectRatio
        {
            get { return this.aspectRatio; }
        }

        public double HorizontalFieldOfView
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

        public double VerticalFieldOfView
        {
            get { return this.verticalFieldOfView; }
        }

        public double NearPlaneDistance
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

        public double FarPlaneDistance
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

        public double FocalLength
        {
            get { return this.focalLength; }
        }

        public double NearPlaneWidth
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
                MathUtil.DegreesToRadians((float)this.VerticalFieldOfView),
                (float)this.AspectRatio,
                (float)this.NearPlaneDistance,
                (float)this.FarPlaneDistance);
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

        public void SetTarget(double time, Vector3 target)
        {
            this.SetTarget(time, target, BasicEasingCurve.Linear);
        }

        public void SetTarget(double time, Vector3 target, IEasingCurve easingCurve)
        {
            this.TargetProperty.InsertKeyframe(time, target, easingCurve);
        }

        public void SetTarget(double time, double x, double y, double z, IEasingCurve easingCurve)
        {
            this.SetTarget(time, new Vector3((float)x, (float)y, (float)z), easingCurve);
        }

        public void SetTarget(double time, double x, double y, double z)
        {
            this.SetTarget(time, x, y, z, BasicEasingCurve.Linear);
        }

        public override void UpdateToTime(double time)
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

        private static double FocalLengthFromFieldOfView(double degrees)
        {
            return 1.0 / Math.Tan(Math3D.DegreesToRadians(degrees / 2.0));
        }

        private static double VerticalFromHorizontalFieldOfView(double degrees, double aspectRatio)
        {
            double horizontalRadians = Math3D.DegreesToRadians(degrees);
            double verticalRadians = Math.Atan(Math.Tan(horizontalRadians * 0.5) / aspectRatio) * 2.0;
            return Math3D.RadiansToDegrees(verticalRadians);
        }
    }
}
