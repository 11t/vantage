namespace Vantage
{
    using System.Collections.Generic;
    using System.Drawing;

    public sealed class StoryboardResourceManager
    {
        private static readonly StoryboardResourceManager InstancePrivate = new StoryboardResourceManager();

        private StoryboardResourceManager()
        {
            this.SpriteImageDictionary = new Dictionary<string, Image>();
        }

        public static StoryboardResourceManager Instance
        {
            get
            {
                return InstancePrivate;
            }
        }

        public IDictionary<string, Image> SpriteImageDictionary { get; private set; }

        public Image GetImage(string imageName)
        {
            return
                this.GetImageFromAbsolutePath(System.IO.Path.Combine(StoryboardSettings.Instance.Directory, imageName));
        }

        public Image GetImageFromAbsolutePath(string imagePath)
        {
            if (this.SpriteImageDictionary.ContainsKey(imagePath))
            {
                return this.SpriteImageDictionary[imagePath];
            }

            Image image = Image.FromFile(imagePath);
            this.SpriteImageDictionary[imagePath] = image;
            return image;
        }
    }
}
