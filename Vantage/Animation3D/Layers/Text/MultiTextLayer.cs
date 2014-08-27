namespace Vantage.Animation3D.Layers.Text
{
    using System.Collections.Generic;

    public class MultiTextLayer : Layer, ITextLayer
    {
        private Font font;
        private string text;
        private int letterSpacing;
        private int spaceWidth;
        private TextAlignment alignment;

        public MultiTextLayer(Font font, int letterSpacing, int spaceWidth, TextAlignment alignment)
        {
            this.font = font;
            this.letterSpacing = letterSpacing;
            this.spaceWidth = spaceWidth;
            this.alignment = alignment;
            this.TextLayers = new List<ITextLayer>();
        }

        public IList<ITextLayer> TextLayers { get; private set; } 

        public Font Font
        {
            get
            {
                return this.font;
            }

            set
            {
                this.font = value;
                foreach (ITextLayer textLayer in this.TextLayers)
                {
                    textLayer.Font = value;
                }
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
                foreach (ITextLayer textLayer in this.TextLayers)
                {
                    textLayer.Text = value;
                }
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
                foreach (ITextLayer textLayer in this.TextLayers)
                {
                    textLayer.LetterSpacing = value;
                }
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
                foreach (ITextLayer textLayer in this.TextLayers)
                {
                    textLayer.SpaceWidth = value;
                }
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
                foreach (ITextLayer textLayer in this.TextLayers)
                {
                    textLayer.Alignment = value;
                }
            }
        }

        public TextLayer NewTextLayer()
        {
            var textLayer = new TextLayer(this.Font, this.LetterSpacing, this.SpaceWidth, this.Alignment);
            textLayer.Parent = this;
            textLayer.Text = this.Text;
            this.TextLayers.Add(textLayer);
            return textLayer;
        }
    }
}