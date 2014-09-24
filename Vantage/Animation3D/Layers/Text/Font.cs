namespace Vantage.Animation3D.Layers.Text
{
    using System.Collections.Generic;

    public class Font
    {
        public Font(string directory, string charset)
            : this(directory, charset, "sb/lyrics/", ".png")
        {
        }

        public Font(string directory, string charset, string prefix, string extension)
        {
            this.Letters = new Dictionary<char, Letter>();
            this.Prefix = prefix;
            this.Extension = extension;
            int i = 0;
            foreach (char c in charset)
            {
                if (!this.Letters.ContainsKey(c) && !char.IsWhiteSpace(c))
                {
                    string imageName = prefix + i + extension;
                    Letter letter = new Letter(directory, imageName);
                    this.Letters[c] = letter;
                    i++;
                }
            }
        }

        public IDictionary<char, Letter> Letters { get; set; }

        public string Prefix { get; set; }

        public string Extension { get; set; }

        public int DefaultLetterSpacing { get; set; }

        public int DefaultSpaceWidth { get; set; }

        public Sprite3D LetterSprite(char c)
        {
            return new Sprite3D(this.Letters[c].ImageName);
        }
    }
}
