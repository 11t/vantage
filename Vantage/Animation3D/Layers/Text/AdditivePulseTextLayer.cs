namespace Vantage.Animation3D.Layers.Text
{
    public class AdditivePulseTextLayer : MultiTextLayer
    {
        public AdditivePulseTextLayer(Font font, int letterSpacing, int spaceWidth, TextAlignment alignment)
            : base(font, letterSpacing, spaceWidth, alignment)
        {
            var textLayer = new TextLayer<PulseSprite>(this.Font, this.LetterSpacing, this.SpaceWidth, this.Alignment)
                                {
                                    Parent = this,
                                    Text = this.Text
                                };
            this.TextLayers.Add(textLayer);

            var textLayerAdditive = new TextLayer<PulseSprite>(
                this.Font,
                this.LetterSpacing,
                this.SpaceWidth,
                this.Alignment) { Parent = this, Text = this.Text, Additive = true };
            this.TextLayers.Add(textLayerAdditive);
        }

        public AdditivePulseTextLayer(Font font, int letterSpacing, int spaceWidth)
            : this(font, letterSpacing, spaceWidth, TextAlignment.Center)
        {
        }

        public AdditivePulseTextLayer(Font font, int letterSpacing)
            : this(font, letterSpacing, font.DefaultSpaceWidth)
        {
        }

        public AdditivePulseTextLayer(Font font)
            : this(font, font.DefaultLetterSpacing)
        {
        }
    }
}