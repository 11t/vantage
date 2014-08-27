namespace Vantage.Animation3D.Layers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using SharpDX;

    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    public class Layer : ILayer
    {
        private ILayer parent;

        private bool additive;
        private bool horizontalFlip;
        private bool verticalFlip;

        public Layer()
        {
            this.Children = new List<ILayer>();
            this.Position = new PositionProperty();
            this.Rotation = new RotationProperty();
            this.Scale = new ScaleProperty();
            this.Color = new ColorProperty();
            this.Opacity = new OpacityProperty();

            this.Forward = Vector3.ForwardRH;
            this.Up = Vector3.Up;
            this.Right = Vector3.Right;

            this.additive = false;
            this.horizontalFlip = false;
            this.verticalFlip = false;
        }

        public float CurrentTime { get; private set; }

        public ILayer Root
        {
            get
            {
                if (this.Parent == null)
                {
                    return this;
                }

                return this.Parent.Root;
            }
        }

        public ILayer Parent
        {
            get
            {
                return this.parent;
            }

            [SuppressMessage("StyleCop.CSharp.LayoutRules",
                "SA1503:CurlyBracketsMustNotBeOmitted",
                Justification = "Reviewed. Suppression is OK here.")]
            set
            {
                if (this.parent != null)
                {
                    this.parent.Children.Remove(this);
                }

                if (value != null)
                {
                    value.Children.Add(this);

                    // Only change if new parent's parameters are true
                    if (value.Additive) this.Additive = true;
                    if (value.HorizontalFlip) this.HorizontalFlip = true;
                    if (value.VerticalFlip) this.VerticalFlip = true;
                }

                this.parent = value;
            }
        }

        public IList<ILayer> Children { get; private set; }

        public IAnimatableProperty<Keyframe<Vector3>, Vector3> Position { get; private set; }

        public IAnimatableProperty<QuaternionKeyframe, Quaternion> Rotation { get; private set; }

        public IAnimatableProperty<Keyframe<Vector3>, Vector3> Scale { get; private set; }

        public IAnimatableProperty<Keyframe<Vector3>, Vector3> Color { get; private set; }

        public IAnimatableProperty<Keyframe<float>, float> Opacity { get; private set; }

        public Vector3 Forward { get; private set; }

        public Vector3 Up { get; private set; }

        public Vector3 Right { get; private set; }

        public Vector3 WorldPosition { get; private set; }

        public Quaternion WorldRotation { get; private set; }

        public Vector3 WorldScale { get; private set; }

        public Vector3 WorldColor { get; private set; }

        public float WorldOpacity { get; private set; }

        public Vector3 LocalPosition
        {
            get
            {
                return this.Position.CurrentValue;
            }
        }

        public Quaternion LocalRotation
        {
            get
            {
                return this.Rotation.CurrentValue;
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return this.Scale.CurrentValue;
            }
        }

        public Vector3 LocalColor
        {
            get
            {
                return this.Color.CurrentValue;
            }
        }

        public float LocalOpacity
        {
            get
            {
                return this.Opacity.CurrentValue;
            }
        }

        public Matrix LocalToWorld { get; private set; }

        public Matrix LocalToParent { get; private set; }

        public virtual bool Additive
        {
            get
            {
                return this.additive;
            }

            set
            {
                this.additive = value;
                foreach (ILayer child in this.Children)
                {
                    child.Additive = value;
                }
            }
        }

        public virtual bool HorizontalFlip
        {
            get
            {
                return this.horizontalFlip;
            }

            set
            {
                this.horizontalFlip = value;
                foreach (ILayer child in this.Children)
                {
                    child.HorizontalFlip = value;
                }
            }
        }

        public virtual bool VerticalFlip
        {
            get
            {
                return this.verticalFlip;
            }

            set
            {
                this.verticalFlip = value;
                foreach (ILayer child in this.Children)
                {
                    child.VerticalFlip = value;
                }
            }
        }

        public bool DebugTrack { get; set; }

        /* PED: Use object initializers for atomicity.
         * Ensures that objects are never partially initialized; useful in multithreaded environment.
         */
        public T NewChild<T>() where T : ILayer, new()
        {
            var child = new T { Parent = this };
            return child;
        }

        public Layer NewLayer()
        {
            var layer = new Layer { Parent = this };
            return layer;
        }

        public Sprite3D NewSprite(string imageName)
        {
            var sprite = new Sprite3D(imageName) { Parent = this };
            return sprite;
        }

        public virtual void UpdateToTime(float time)
        {
            this.CurrentTime = time;
            this.Position.UpdateToTime(time);
            this.Rotation.UpdateToTime(time);
            this.Scale.UpdateToTime(time);
            this.Color.UpdateToTime(time);
            this.Opacity.UpdateToTime(time);

            var translationMatrix = Matrix.Translation(this.LocalPosition);
            //// var rotationMatrix = Matrix.RotationYawPitchRoll(this.LocalRotation.Y, this.LocalRotation.X, this.LocalRotation.Z);
            var rotationMatrix = Matrix.RotationQuaternion(this.LocalRotation);
            var scaleMatrix = Matrix.Scaling(this.LocalScale);
            this.LocalToParent = scaleMatrix * rotationMatrix * translationMatrix;
            if (this.Parent == null)
            {
                this.WorldPosition = this.LocalPosition;
                this.WorldRotation = this.LocalRotation;
                this.WorldScale = this.LocalScale;
                this.WorldOpacity = this.LocalOpacity;
                this.WorldColor = this.LocalColor;
                this.LocalToWorld = this.LocalToParent;
                
                this.Forward = Vector3.TransformNormal(Vector3.ForwardRH, rotationMatrix);
                this.Up = Vector3.TransformNormal(Vector3.Up, rotationMatrix);
                this.Right = Vector3.TransformNormal(Vector3.Right, rotationMatrix);
            }
            else
            {
                this.WorldPosition = Vector3.TransformCoordinate(this.LocalPosition, this.Parent.LocalToWorld);
                this.WorldRotation = Quaternion.Normalize(this.Parent.WorldRotation * this.LocalRotation);
                this.WorldScale = this.Parent.WorldScale * this.LocalScale;
                this.WorldOpacity = this.Parent.WorldOpacity * this.LocalOpacity;
                this.WorldColor = this.Parent.WorldColor * this.LocalColor;
                this.LocalToWorld = this.LocalToParent * this.Parent.LocalToWorld;
                
                this.Forward = Vector3.TransformNormal(this.Parent.Forward, rotationMatrix);
                this.Up = Vector3.TransformNormal(this.Parent.Up, rotationMatrix);
                this.Right = Vector3.TransformNormal(this.Parent.Right, rotationMatrix);
            }

            //// if (DebugTrack) System.Diagnostics.Debug.WriteLine(this.WorldPosition.ToString());
            foreach (var child in this.Children)
            {
                child.UpdateToTime(time);
            }
        }

        public void SetPosition(float time, Vector3 position)
        {
            this.Position.InsertKeyframe(time, position);
        }

        public void SetPosition(float time, Vector3 position, IEasingCurve easingCurve)
        {
            this.Position.InsertKeyframe(time, position, easingCurve);
        }

        public void SetPosition(float time, float x, float y, float z)
        {
            this.Position.InsertKeyframe(time, x, y, z);
        }

        public void SetPosition(float time, float x, float y, float z, IEasingCurve easingCurve)
        {
            this.Position.InsertKeyframe(easingCurve, time, x, y, z);
        }

        public void SetAngles(float time, float x, float y, float z)
        {
            this.SetAngles(time, x, y, z, BasicEasingCurve.Linear);
        }

        public void SetAngles(float time, float x, float y, float z, IEasingCurve easingCurve)
        {
            x = MathUtil.DegreesToRadians(x);
            y = MathUtil.DegreesToRadians(y);
            z = MathUtil.DegreesToRadians(z);
            Quaternion rotationQuaternion = Quaternion.RotationYawPitchRoll(y, x, z);
            this.SetRotation(time, rotationQuaternion, easingCurve);
        }

        public void SetRotation(float time, Quaternion rotationQuaternion, IEasingCurve easingCurve)
        {
            this.Rotation.InsertKeyframe(time, Quaternion.Normalize(rotationQuaternion), easingCurve);
        }

        public void SetRotation(float time, Quaternion rotationQuaternion)
        {
            this.SetRotation(time, rotationQuaternion, BasicEasingCurve.Linear);
        }

        public void SetScale(float time, float x, float y, float z)
        {
            this.Scale.InsertKeyframe(time, x, y, z);
        }

        public void SetScale(float time, float s)
        {
            this.SetScale(time, s, s, s);
        }

        public void SetColor(float time, float r, float g, float b)
        {
            this.Color.InsertKeyframe(time, r, g, b);
        }

        public void SetColor(float time, OsbColor color)
        {
            this.SetColor(time, color.R, color.G, color.B);
        }

        public void SetOpacity(float time, float opacity)
        {
            this.Opacity.InsertKeyframe(time, opacity);
        }

        public void SetOpacity(float time, float opacity, IEasingCurve easingCurve)
        {
            this.Opacity.InsertKeyframe(time, opacity, easingCurve);
        }

        public void OpacityTransition(
            float startTime,
            float endTime,
            float startOpacity,
            float endOpacity,
            IEasingCurve easingCurve)
        {
            this.SetOpacity(startTime, startOpacity, easingCurve);
            this.SetOpacity(endTime, endOpacity);
        }

        public void OpacityTransition(float startTime, float endTime, float startOpacity, float endOpacity)
        {
            this.OpacityTransition(startTime, endTime, startOpacity, endOpacity, BasicEasingCurve.Linear);
        }
    }
}
