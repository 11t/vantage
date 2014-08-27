namespace Vantage.Animation3D.Layers.Text
{
    public enum TextAlignment
    {
        Center,
        Left,
        Right
    }

    public interface ITextLayer : ILayer
    {
        Font Font { get; set; }

        string Text { get; set; }

        int LetterSpacing { get; set; }

        int SpaceWidth { get; set; }

        TextAlignment Alignment { get; set; }
    }
}