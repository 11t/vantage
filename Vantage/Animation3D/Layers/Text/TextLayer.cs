﻿namespace Vantage.Animation3D.Layers.Text
{
    using System.Collections.Generic;

    public class TextLayer : Layer, ITextLayer
    {
        private readonly IList<Sprite3D> textSprites; 

        private Font font;
        private string text;
        private int letterSpacing;
        private int spaceWidth;
        private TextAlignment alignment;

        public TextLayer(Font font)
            : this(font, StoryboardSettings.Instance.SceneConversionSettings.DefaultTextLetterSpacing)
        {
        }

        public TextLayer(Font font, int letterSpacing)
            : this(font, letterSpacing, StoryboardSettings.Instance.SceneConversionSettings.DefaultTextLetterSpacing)
        {
        }

        public TextLayer(Font font, int letterSpacing, int spaceWidth)
            : this(font, letterSpacing, spaceWidth, TextAlignment.Center)
        {
        }

        public TextLayer(Font font, int letterSpacing, int spaceWidth, TextAlignment alignment)
        {
            this.font = font;
            this.letterSpacing = letterSpacing;
            this.spaceWidth = spaceWidth;
            this.alignment = alignment;
            this.textSprites = new List<Sprite3D>();
        }

        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                this.font = value;
                this.ClearTextSprites();
                this.CreateTextSprites();
            }
        }
        
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.ClearTextSprites();
                this.CreateTextSprites();
            }
        }
        
        public int LetterSpacing
        {
            get
            {
                return this.letterSpacing;
            }

            set
            {
                this.letterSpacing = value;
                this.UpdateTextSpritesPositions();
            }
        }

        public int SpaceWidth
        {
            get
            {
                return this.spaceWidth;
            }

            set
            {
                this.spaceWidth = value;
                this.UpdateTextSpritesPositions();
            }
        }

        public TextAlignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                this.alignment = value;
                this.UpdateTextSpritesPositions();
            }
        }

        private void ClearTextSprites()
        {
            foreach (Sprite3D sprite in this.textSprites)
            {
                Children.Remove(sprite);
            }

            this.textSprites.Clear();
        }

        private void CreateTextSprites()
        {
            if (this.Text == null || this.Text.Length <= 0)
            {
                return;
            }

            IList<int> positions = this.HorizontalPositions();
            int i = 0;
            foreach (char c in this.Text)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                string imageName = this.Font.Letters[c].ImageName;
                Sprite3D sprite = this.NewSprite(imageName);
                sprite.SetPosition(0, positions[i], 0, 0);
                this.textSprites.Add(sprite);
                i++;
            }
        }

        private void UpdateTextSpritesPositions()
        {
            if (this.Text.Length <= 0)
            {
                return;
            }

            IList<int> positions = this.HorizontalPositions();
            int i = 0;
            foreach (char c in this.Text)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                Sprite3D sprite = this.textSprites[i];
                sprite.SetPosition(0, positions[i], 0, 0);
                i++;
            }
        }

        private IList<int> HorizontalPositions()
        {
            int spaceAdjustment = 0;
            int prevPosition = 0;
            int prevHalfWidth = this.Font.Letters[this.Text[0]].Width / 2;
            var positions = new List<int> { 0 };
            for (int i = 1; i < this.Text.Length; i++)
            {
                char c = this.Text[i];
                if (char.IsWhiteSpace(c))
                {
                    spaceAdjustment += this.SpaceWidth;
                    continue;
                }

                int halfWidth = this.Font.Letters[c].Width / 2;
                int position = prevPosition + prevHalfWidth + this.LetterSpacing + halfWidth + spaceAdjustment;
                positions.Add(position);
                prevHalfWidth = halfWidth;
                prevPosition = position;
                spaceAdjustment = 0;
            }

            // Alignment adjust
            switch (this.Alignment)
            {
                case TextAlignment.Center:
                    int center = positions[positions.Count - 1] / 2;
                    for (int i = 0; i < positions.Count; i++)
                    {
                        positions[i] -= center;
                    }

                    break;

                case TextAlignment.Left:
                    break;

                case TextAlignment.Right:
                    int right = positions[positions.Count - 1];
                    for (int i = 0; i < positions.Count; i++)
                    {
                        positions[i] -= right;
                    }

                    break;
            }

            return positions;
        }
    }
}
