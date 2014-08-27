namespace Vantage.Animation3D.Layers
{
    using System;

    using SharpDX;

    using Vantage.Animation3D.Animation.EasingCurves;

    public interface ICamera
    {
    }

    public class Camera : Layer, ICamera
    {
        #region Fields

        private readonly Vector2 _resolution;
        private readonly float _aspectRatio;

        private float _horizontalFieldOfView;
        private float _verticalFieldOfView;
        private float _focalLength;
        private float _nearPlaneDistance;
        private float _farPlaneDistance;
        private float _nearPlaneWidth;

        #endregion

        public Camera(float width, float height)
        {
            this._resolution = new Vector2(width, height);
            this._aspectRatio = width / height;
            this.HorizontalFieldOfView = 60.0f;
            this.NearPlaneDistance = 10.0f;
            this.FarPlaneDistance = 2000.0f;
            /*
            _forward = Vector3.ForwardRH;
            _up = Vector3.Up;
            _right = Vector3.Right;
            */
        }

        #region Properties

        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public Matrix ViewProjection { get; private set; }

        /*
        public Vector3 Forward
        {
            get
            {
                return _forward;
            }
            set
            {
                _forward = Vector3.Normalize(value);
            }
        }

        public Vector3 Up
        {
            get
            {
                return _up;
            }
            set
            {
                _up = Vector3.Normalize(value);
            }
        }

        public Vector3 Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = Vector3.Normalize(value);
            }
        }
        */
        
        public Vector3 Target
        {
            get
            {
                return this.Forward + this.WorldPosition;
            }
        }

        public Vector2 Resolution
        {
            get { return this._resolution; }
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
            get { return this._aspectRatio; }
        }

        public float HorizontalFieldOfView
        {
            get
            {
                return this._horizontalFieldOfView;
            }

            set
            {
                this._horizontalFieldOfView = value;
                this._verticalFieldOfView = VerticalFromHorizontalFieldOfView(this.HorizontalFieldOfView, this.AspectRatio);
                this._focalLength = FocalLengthFromFieldOfView(this.HorizontalFieldOfView);
                this._nearPlaneWidth = this.Resolution.X * this.FocalLength * 0.5f;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float VerticalFieldOfView
        {
            get { return this._verticalFieldOfView; }
        }

        public float NearPlaneDistance
        {
            get
            {
                return this._nearPlaneDistance;
            }

            set
            {
                this._nearPlaneDistance = value;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float FarPlaneDistance
        {
            get
            {
                return this._farPlaneDistance;
            }

            set
            {
                this._farPlaneDistance = value;
                this.ProjectionNeedsUpdate = true;
            }
        }

        public float FocalLength
        {
            get { return this._focalLength; }
        }

        public float NearPlaneWidth
        {
            get { return this._nearPlaneWidth; }
        }

        public bool ViewNeedsUpdate { get; set; }

        public bool ProjectionNeedsUpdate { get; set; }

        #endregion

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
        public void SetTarget(float time, Vector3 target)
        {
            this.Target.InsertKeyframe(time, target);
        }

        public void SetTarget(float time, Vector3 target, IEasingCurve easingCurve)
        {
            this.Target.InsertKeyframe(time, target, easingCurve);
        }

        public void SetTarget(float time, float x, float y, float z)
        {
            this.Target.InsertKeyframe(time, x, y, z);
        }

        public void SetTarget(float time, float x, float y, float z, IEasingCurve easingCurve)
        {
            this.Target.InsertKeyframe(easingCurve, time, x, y, z);
        }
        */

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

        public override void UpdateToTime(float time)
        {
            base.UpdateToTime(time);

            /*
            bool updated = false;
            if (this.ViewNeedsUpdate)
            {
                this.UpdateView();
                updated = true;
            }

            if (this.ProjectionNeedsUpdate)
            {
                this.UpdateProjection();
                updated = true;
            }

            if (updated)
            {
                this.ViewProjection = this.View * this.Projection;
            }
            */
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
