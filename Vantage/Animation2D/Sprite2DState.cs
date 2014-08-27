﻿namespace Vantage.Animation2D
{
    using SharpDX;

    public class Sprite2DState
    {
        public Sprite2DState(
            float time,
            Vector2 position,
            double rotation,
            Vector2 scale,
            Vector3 color,
            double opacity,
            bool additive,
            bool horizontalFlip,
            bool verticalFlip,
            int width,
            int height)
        {
            this.Time = time;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Color = color;
            this.Opacity = opacity;
            this.Additive = additive;
            this.HorizontalFlip = horizontalFlip;
            this.VerticalFlip = verticalFlip;
            this.Width = width;
            this.Height = height;
        }

        public float Time { get; set; }

        public Vector2 Position { get; set; }

        public double Rotation { get; set; }

        public Vector2 Scale { get; set; }

        public Vector3 Color { get; set; }

        public double Opacity { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool Additive { get; set; }

        public bool HorizontalFlip { get; set; }

        public bool VerticalFlip { get; set; }

        public bool Visible
        {
            get
            {
                float halfWidth = this.Width / 2.0f * this.Scale.X;
                float halfHeight = this.Height / 2.0f * this.Scale.Y;
                float left = this.Position.X - halfWidth;
                float right = this.Position.X + halfWidth;
                float top = this.Position.Y - halfHeight;
                float bottom = this.Position.Y + halfHeight;
                Rectangle bounds = Storyboard.ViewportBounds;
                if (!(bounds.Left < right && bounds.Right > left &&
                      bounds.Bottom > top && bounds.Top < bottom))
                {
                    return false;
                }

                if (this.Opacity <= 0)
                {
                    return false;
                }

                if (this.Scale.X <= 0 || this.Scale.Y <= 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
