namespace Vantage.Animation3D.Layers
{
    using System;
    using System.Drawing;

    using SharpDX;

    using Vantage.Animation2D;

    public class Sprite3D : Layer
    {
        private readonly Sprite2D _representative;

        private string _imageName;
        private int _width;
        private int _height;

        public Sprite3D(string imageName, string layer, string origin)
            : base()
        {
            this.ImageName = imageName;
            this.Layer = layer;
            this.Origin = origin;

            this._representative = new Sprite2D(imageName, layer, origin);
        }

        public Sprite3D(string imageName)
            : this(imageName, "Foreground", "Centre")
        {
        }

        public string ImageName
        {
            get
            {
                return this._imageName;
            }

            set
            {
                if (value != null)
                {
                    Image image = StoryboardResourceManager.Instance.GetImage(value);
                    this._width = image.Width;
                    this._height = image.Height;
                }

                this._imageName = value;
            }
        }

        public string Layer { get; set; }

        public string Origin { get; set; }

        public int Width
        {
            get
            {
                return this._width;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
        }

        public Sprite2DState State { get; set; }

        public Sprite2D Representative
        {
            get
            {
                return this._representative;
            }
        }

        public override bool Additive
        {
            get
            {
                return base.Additive;
            }

            set
            {
                this.Representative.Additive = value;
                base.Additive = value;
            }
        }

        public override bool HorizontalFlip
        {
            get
            {
                return base.HorizontalFlip;
            }

            set
            {
                this.Representative.HorizontalFlip = value;
                base.HorizontalFlip = value;
            }
        }

        public override bool VerticalFlip
        {
            get
            {
                return base.VerticalFlip;
            }

            set
            {
                this.Representative.VerticalFlip = value;
                base.VerticalFlip = value;
            }
        }

        public void UpdateState(Camera mainCamera)
        {
            Vector2 resolution = mainCamera.Resolution;

            Vector4 worldCoordinates = new Vector4(this.WorldPosition, 1.0f);
            Vector4 clipCoordinates = Vector4.Transform(worldCoordinates, mainCamera.ViewProjection);
            Vector2 ndc = new Vector2(clipCoordinates.X, -clipCoordinates.Y) / clipCoordinates.W;

            Vector2 position = (ndc + Vector2.One) / 2.0f * Storyboard.ViewportSize;
            position.X -= 107;

            // float rotation = -this.WorldRotation.Z;
            float rotation = this.CalculateRoll(this.WorldRotation * Quaternion.Invert(mainCamera.WorldRotation));

            float defaultScale = Storyboard.ViewportSize.Y / resolution.Y;
            Vector2 scale = new Vector2(this.WorldScale.X, this.WorldScale.Y);
            scale = scale * mainCamera.NearPlaneWidth / clipCoordinates.W * defaultScale;

            this.State = new Sprite2DState(
                this.CurrentTime,
                position,
                rotation,
                scale,
                this.WorldColor,
                this.WorldOpacity,
                this.Additive,
                this.HorizontalFlip,
                this.VerticalFlip,
                this.Width,
                this.Height);
            if (DebugTrack)
            {
                System.Diagnostics.Debug.WriteLine(this.CurrentTime.ToString(), mainCamera.ViewProjection.ToString());
            }
        }

        private float CalculateRoll(Quaternion rotation)
        {
            float x = rotation.X;
            float y = rotation.Y;
            float z = rotation.Z;
            float w = rotation.W;
            /*
            return (float)Math.Atan2(
                2 * rotation.Y * rotation.W - 2 * rotation.X * rotation.Z,
                1 - 2 * rotation.Y * rotation.Y - 2 * rotation.Z * rotation.Z);
             */
            return -(float)Math.Atan2(2 * (x * y + w * z), w * w + x * x - y * y - z * z);
        }
    }
}
