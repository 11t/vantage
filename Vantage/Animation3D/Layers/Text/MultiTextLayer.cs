namespace Vantage.Animation3D.Layers.Text
{
    using System.Collections.Generic;

    using SharpDX;

    public class MultiTextLayer : Layer, ITextLayer
    {
        private Font _font;
        private string _text;
        private int _letterSpacing;
        private int _spaceWidth;
        private TextAlignment _alignment;

        public MultiTextLayer(Font font, int letterSpacing, int spaceWidth, TextAlignment alignment)
        {
            this._font = font;
            this._letterSpacing = letterSpacing;
            this._spaceWidth = spaceWidth;
            this._alignment = alignment;
            this.TextLayers = new List<ITextLayer>();
        }

        public IList<ITextLayer> TextLayers { get; private set; } 

        public Font Font
        {
            get
            {
                return this._font;
            }

            set
            {
                this._font = value;
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
                return this._text;
            }

            set
            {
                this._text = value;
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
                return this._letterSpacing;
            }

            set
            {
                this._letterSpacing = value;
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
                return this._spaceWidth;
            }

            set
            {
                this._spaceWidth = value;
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
                return this._alignment;
            }

            set
            {
                this._alignment = value;
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