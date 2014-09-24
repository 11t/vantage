namespace Vantage.Animation3D.Layers.Text
{
    public class AdditiveEnhancedTextLayer : MultiTextLayer
    {
        public AdditiveEnhancedTextLayer(Font font, int letterSpacing, int spaceWidth, TextAlignment alignment)
            : base(font, letterSpacing, spaceWidth, alignment)
        {
            this.NewTextLayer<Sprite3D>();
            this.NewTextLayer<Sprite3D>().Additive = true;
        }

        public AdditiveEnhancedTextLayer(Font font, int letterSpacing, int spaceWidth)
            : this(font, letterSpacing, spaceWidth, TextAlignment.Center)
        {
        }

        public AdditiveEnhancedTextLayer(Font font, int letterSpacing)
            : this(font, letterSpacing, font.DefaultSpaceWidth)
        {
        }

        public AdditiveEnhancedTextLayer(Font font)
            : this(font, font.DefaultLetterSpacing)
        {
        }
    }
}