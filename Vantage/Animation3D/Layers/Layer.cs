﻿namespace Vantage.Animation3D.Layers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using SharpDX;

    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Animation.Keyframes;

    /// <summary>
    /// Represents a 3D layer with animatable position, rotation, scale, color, and opacity properties.
    /// </summary>
    public class Layer : ILayer
    {
        protected static readonly IEasingCurve DefaultEasingCurve = BasicEasingCurve.Linear;

        /// <summary>
        /// The parent Layer in the layer hierarchy. 
        /// </summary>
        private ILayer parent;

        /// <summary>
        /// Indicates whether the layer (and its children) should be rendered using additive blending.
        /// </summary>
        private bool additive;

        /// <summary>
        /// Indicates whether the layer (and its children) should be flipped horizontally.
        /// </summary>
        private bool horizontalFlip;

        /// <summary>
        /// Indicates whether the layer (and its children) should be flipped vertically.
        /// </summary>
        private bool verticalFlip;

        private bool lockScale;

        private bool lockRotation;

        private double nearFogDistanceMinimum;

        private double nearFogDistanceMaximum;

        private double farFogDistanceMinimum;

        private double farFogDistanceMaximum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
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
            this.lockScale = false;
            this.lockRotation = false;
        }

        /// <summary>
        /// Gets the current time to which this Layer was last updated.
        /// </summary>
        public double CurrentTime { get; private set; }

        /// <summary>
        /// Gets the root Layer in the Layer hierarchy.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the parent Layer.
        /// </summary>
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
                    if (value.LockScale) this.LockScale = true;
                    if (value.LockRotation) this.LockRotation = true;
                    this.FarFogDistanceMaximum = value.FarFogDistanceMaximum;
                    this.FarFogDistanceMinimum = value.FarFogDistanceMinimum;
                    this.NearFogDistanceMaximum = value.NearFogDistanceMaximum;
                    this.NearFogDistanceMinimum = value.NearFogDistanceMinimum;
                }

                this.parent = value;
            }
        }

        /// <summary>
        /// Gets the children Layers, for which this Layer is a parent.
        /// </summary>
        public IList<ILayer> Children { get; private set; }

        /// <summary>
        /// Gets the position property of this Layer, representing its position in 3D space, relative to its parent, at any given time. 
        /// </summary>
        public IAnimatableProperty<Keyframe<Vector3>, Vector3> Position { get; private set; }

        /// <summary>
        /// Gets the rotation property of this Layer, representing its rotation in 3D space, relative to its parent, at any given time.
        /// </summary>
        public IAnimatableProperty<QuaternionKeyframe, Quaternion> Rotation { get; private set; }

        /// <summary>
        /// Gets the scale property, representing this Layer's scaling in 3D space, relative to its parent's scale, at any given time.
        /// </summary>
        public IAnimatableProperty<Keyframe<Vector3>, Vector3> Scale { get; private set; }

        /// <summary>
        /// Gets the color property, representing this Layer's color, relative multiplicatively to its parent's color, at any given time.
        /// </summary>
        public IAnimatableProperty<Keyframe<OsbColor>, OsbColor> Color { get; private set; }

        /// <summary>
        /// Gets the opacity property, representing this Layer's opacity, relative to its parent's opacity, at any given time.
        /// </summary>
        public IAnimatableProperty<Keyframe<double>, double> Opacity { get; private set; }

        /// <summary>
        /// Gets the forward vector, the positive Z axis direction of this Layer relative to the world's coordinate system.
        /// </summary>
        public Vector3 Forward { get; private set; }

        /// <summary>
        /// Gets the up vector, the positive Y axis direction of this Layer relative to the world's coordinate system.
        /// </summary>
        public Vector3 Up { get; private set; }

        /// <summary>
        /// Gets the right vector, the positive X axis direction of this Layer relative to the world's coordinate system.
        /// </summary>
        public Vector3 Right { get; private set; }

        /// <summary>
        /// Gets the current position relative to the world. Represents the translation of the Layer from the origin (0, 0, 0) of world space.
        /// </summary>
        public Vector3 WorldPosition { get; private set; }

        /// <summary>
        /// Gets the current rotation relative to the world.
        /// </summary>
        public Quaternion WorldRotation { get; private set; }

        /// <summary>
        /// Gets the current scale relative to the world.
        /// </summary>
        public Vector3 WorldScale { get; private set; }

        /// <summary>
        /// Gets the current color relative to the world.
        /// </summary>
        public OsbColor WorldColor { get; private set; }

        /// <summary>
        /// Gets the current opacity relative to the world.
        /// </summary>
        public double WorldOpacity { get; private set; }

        /// <summary>
        /// Gets the position relative to the parent Layer. Represents this Layer's translation from the parent's origin (0, 0, 0).
        /// </summary>
        public Vector3 LocalPosition
        {
            get
            {
                return this.Position.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the rotation relative to the parent Layer.
        /// </summary>
        public Quaternion LocalRotation
        {
            get
            {
                return this.Rotation.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the scale relative to the parent Layer.
        /// </summary>
        public Vector3 LocalScale
        {
            get
            {
                return this.Scale.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the color relative to the parent Layer.
        /// </summary>
        public OsbColor LocalColor
        {
            get
            {
                return this.Color.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the opacity relative to the parent Layer.
        /// </summary>
        public double LocalOpacity
        {
            get
            {
                return this.Opacity.CurrentValue;
            }
        }

        /// <summary>
        /// Gets the matrix that transforms vectors in the coordinate system of the Layer to the coordinate system of the world.
        /// </summary>
        public Matrix LocalToWorld { get; private set; }

        /// <summary>
        /// Gets the matrix that transforms vectors in the coordinate system of the Layer to the coordinate system of the parent Layer.
        /// </summary>
        public Matrix LocalToParent { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Layer should be rendered using additive blending. If this value is set to true, all of the Additive properties of the child Layers of this Layer will also be set to true.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether to horizontally flip the Layer. If this value is set to true, the HorizontalFlip properties of each child Layer will also be set to true.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether to vertically flip the Layer. If this value is set to true, the VerticalFlip properties of each child Layer will also be set to true.
        /// </summary>
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

        public bool LockScale
        {
            get
            {
                return this.lockScale;
            }

            set
            {
                this.lockScale = value;
                foreach (ILayer child in this.Children)
                {
                    child.LockScale = value;
                }
            }
        }

        public bool LockRotation
        {
            get
            {
                return this.lockRotation;
            }

            set
            {
                this.lockRotation = value;
                foreach (ILayer child in this.Children)
                {
                    child.LockRotation = value;
                }
            }
        }

        public double FarFogDistanceMaximum
        {
            get
            {
                return this.farFogDistanceMaximum;
            }

            set
            {
                this.farFogDistanceMaximum = value;
                foreach (ILayer child in this.Children)
                {
                    child.FarFogDistanceMaximum = value;
                }
            }
        }

        public double FarFogDistanceMinimum
        {
            get
            {
                return this.farFogDistanceMinimum;
            }

            set
            {
                this.farFogDistanceMinimum = value;
                foreach (ILayer child in this.Children)
                {
                    child.FarFogDistanceMinimum = value;
                }
            }
        }

        public double NearFogDistanceMaximum
        {
            get
            {
                return this.nearFogDistanceMaximum;
            }

            set
            {
                this.nearFogDistanceMaximum = value;
                foreach (ILayer child in this.Children)
                {
                    child.NearFogDistanceMaximum = value;
                }
            }
        }

        public double NearFogDistanceMinimum
        {
            get
            {
                return this.nearFogDistanceMinimum;
            }

            set
            {
                this.nearFogDistanceMinimum = value;
                foreach (ILayer child in this.Children)
                {
                    child.NearFogDistanceMinimum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to track the Layer for debug purposes.
        /// </summary>
        public bool DebugTrack { get; set; }

        /// <summary>
        /// Creates a new Layer and attaches it as a child to this Layer.
        /// </summary>
        /// <typeparam name="TChild">
        /// The type of the child Layer.
        /// </typeparam>
        /// <returns>
        /// The type <see cref="TChild"/> newly created child Layer.
        /// </returns>
        public TChild NewChild<TChild>(params object[] args) where TChild : class, ILayer
        {
            TChild child = Activator.CreateInstance(typeof(TChild), args) as TChild;
            if (child != null)
            {
                child.Parent = this;
            }

            return child;
        }

        /// <summary>
        /// Creates a new child Layer.
        /// </summary>
        /// <returns>
        /// The new child <see cref="Layer"/>.
        /// </returns>
        public Layer NewLayer()
        {
            var layer = new Layer { Parent = this };
            return layer;
        }

        public Sprite3D NewSprite(string imageName, string layer, string origin)
        {
            var sprite = new Sprite3D(imageName, layer, origin) { Parent = this };
            return sprite;
        }

        /// <summary>
        /// Creates a new child Sprite3D layer.
        /// </summary>
        /// <param name="imageName">
        /// The image name of the sprite in the map directory.
        /// </param>
        /// <returns>
        /// The new child <see cref="Sprite3D"/> layer.
        /// </returns>
        public Sprite3D NewSprite(string imageName)
        {
            var sprite = new Sprite3D(imageName) { Parent = this };
            return sprite;
        }

        /// <summary>
        /// Updates the Layer and all children to the time specified. All AnimatableProperty objects are updated to the specified time, and the Layer's local and world properties are updated as well. 
        /// </summary>
        /// <param name="time">
        /// The time.
        /// </param>
        public virtual void UpdateToTime(double time)
        {
            this.CurrentTime = time;
            
            // Update all AnimatableProperty objects.
            this.Position.UpdateToTime(time);
            this.Rotation.UpdateToTime(time);
            this.Scale.UpdateToTime(time);
            this.Color.UpdateToTime(time);
            this.Opacity.UpdateToTime(time);

            // Calculate the local to parent transformation matrix.
            var translationMatrix = Matrix.Translation(this.LocalPosition);
            var rotationMatrix = Matrix.RotationQuaternion(this.LocalRotation);
            var scaleMatrix = Matrix.Scaling(this.LocalScale);
            this.LocalToParent = scaleMatrix * rotationMatrix * translationMatrix;
            
            // Update the Layer's 3D data relative to the world.
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

            // Update all children Layers.
            foreach (var child in this.Children)
            {
                child.UpdateToTime(time);
            }
        }

        public void SetPosition(double time, Vector3 position, IEasingCurve easingCurve)
        {
            this.Position.InsertKeyframe(time, position, easingCurve);
        }

        public void SetPosition(double time, Vector3 position)
        {
            this.SetPosition(time, position, DefaultEasingCurve);
        }

        public void SetPosition(double time, double x, double y, double z, IEasingCurve easingCurve)
        {
            this.SetPosition(time, new Vector3((float)x, (float)y, (float)z), easingCurve);
        }

        public void SetPosition(double time, double x, double y, double z)
        {
            this.SetPosition(time, x, y, z, DefaultEasingCurve);
        }

        public void SetAngles(double time, double x, double y, double z, IEasingCurve easingCurve)
        {
            float x2 = MathUtil.DegreesToRadians((float)x);
            float y2 = MathUtil.DegreesToRadians((float)y);
            float z2 = MathUtil.DegreesToRadians((float)z);
            Quaternion rotationQuaternion = Quaternion.RotationYawPitchRoll(y2, x2, z2);
            this.SetRotation(time, rotationQuaternion, easingCurve);
        }

        public void SetAngles(double time, double x, double y, double z)
        {
            this.SetAngles(time, x, y, z, DefaultEasingCurve);
        }

        public void SetAngles(double time, double z, IEasingCurve easingCurve)
        {
            this.SetAngles(time, 0, 0, z, easingCurve);
        }

        public void SetAngles(double time, double z)
        {
            this.SetAngles(time, z, DefaultEasingCurve);
        }

        public void SetRotation(double time, Quaternion rotationQuaternion, IEasingCurve easingCurve)
        {
            this.Rotation.InsertKeyframe(time, Quaternion.Normalize(rotationQuaternion), easingCurve);
        }

        public void SetRotation(double time, Quaternion rotationQuaternion)
        {
            this.SetRotation(time, rotationQuaternion, DefaultEasingCurve);
        }

        public void SetScale(double time, Vector3 scale, IEasingCurve easingCurve)
        {
            this.Scale.InsertKeyframe(time, scale, easingCurve);
        }

        public void SetScale(double time, Vector3 scale)
        {
            this.SetScale(time, scale, DefaultEasingCurve);
        }

        public void SetScale(double time, double x, double y, double z, IEasingCurve easingCurve)
        {
            this.SetScale(time, new Vector3((float)x, (float)y, (float)z), easingCurve);
        }

        public void SetScale(double time, double x, double y, double z)
        {
            this.SetScale(time, x, y, z, DefaultEasingCurve);
        }

        public void SetScale(double time, double scale, IEasingCurve easingCurve)
        {
            this.SetScale(time, scale, scale, scale, easingCurve);
        }

        public void SetScale(double time, double scale)
        {
            this.SetScale(time, scale, DefaultEasingCurve);
        }

        public void SetColor(double time, OsbColor color, IEasingCurve easingCurve)
        {
            this.Color.InsertKeyframe(time, color, easingCurve);
        }

        public void SetColor(double time, OsbColor color)
        {
            this.SetColor(time, color, DefaultEasingCurve);
        }

        public void SetColor(double time, double r, double g, double b, IEasingCurve easingCurve)
        {
            this.SetColor(time, new OsbColor(r, g, b), easingCurve);
        }

        public void SetColor(double time, double r, double g, double b)
        {
            this.SetColor(time, r, g, b, DefaultEasingCurve);
        }

        public void SetOpacity(double time, double opacity, IEasingCurve easingCurve)
        {
            this.Opacity.InsertKeyframe(time, opacity, easingCurve);
        }

        public void SetOpacity(double time, double opacity)
        {
            this.SetOpacity(time, opacity, DefaultEasingCurve);
        }

        public void Fade(int easing, double startTime, double endTime, double startOpacity, double endOpacity)
        {
            this.Fade(BasicEasingCurve.FromEasingParameter(easing), startTime, endTime, startOpacity, endOpacity);
        }

        public void Fade(IEasingCurve easingCurve, double startTime, double endTime, double startOpacity, double endOpacity)
        {
            this.SetOpacity(startTime, startOpacity, easingCurve);
            this.SetOpacity(endTime, endOpacity, BasicEasingCurve.Step);
        }
    }
}
