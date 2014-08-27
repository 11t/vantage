namespace Vantage.Animation3D.Layers
{
    using System;
    using System.Drawing;
    using System.Globalization;

    using SharpDX;

    using Vantage.Animation2D;

    public class Sprite3D : Layer
    {
        private readonly Sprite2D representative;

        private string imageName;
        private int width;
        private int height;

        public Sprite3D(string imageName, string layer, string origin)
        {
            this.ImageName = imageName;
            this.Layer = layer;
            this.Origin = origin;

            this.representative = new Sprite2D(imageName, layer, origin);
        }

        public Sprite3D(string imageName)
            : this(imageName, "Foreground", "Centre")
        {
        }

        public string ImageName
        {
            get
            {
                return this.imageName;
            }

            set
            {
                if (value != null)
                {
                    Image image = StoryboardResourceManager.Instance.GetImage(value);
                    this.width = image.Width;
                    this.height = image.Height;
                }

                this.imageName = value;
            }
        }

        public string Layer { get; set; }

        public string Origin { get; set; }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public Sprite2DState State { get; set; }

        public Sprite2D Representative
        {
            get
            {
                return this.representative;
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

            if (this.DebugTrack)
            {
                System.Diagnostics.Debug.WriteLine(this.CurrentTime.ToString(CultureInfo.InvariantCulture), mainCamera.ViewProjection.ToString());
            }
        }

        private float CalculateRoll(Quaternion rotation)
        {
            float x = rotation.X;
            float y = rotation.Y;
            float z = rotation.Z;
            float w = rotation.W;
            float a = 2 * ((x * y) + (w * z));
            float b = (w * w) + (x * x) - (y * y) - (z * z);
            return -(float)Math.Atan2(a, b);
        }
    }
}
