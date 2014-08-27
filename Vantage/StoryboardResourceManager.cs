namespace Vantage
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;

    public sealed class StoryboardResourceManager
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules",
            "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        private static readonly StoryboardResourceManager _instance =
            new StoryboardResourceManager();

        private StoryboardResourceManager()
        {
            SpriteImageDictionary = new Dictionary<string, Image>();
        }

        public static StoryboardResourceManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public IDictionary<string, Image> SpriteImageDictionary { get; private set; }

        public Image GetImage(string imageName)
        {
            return
                GetImageFromAbsolutePath(
                    System.IO.Path.Combine(StoryboardSettings.Instance.Directory, imageName));
        }

        public Image GetImageFromAbsolutePath(string imagePath)
        {
            if (SpriteImageDictionary.ContainsKey(imagePath))
            {
                return SpriteImageDictionary[imagePath];
            }

            Image image = Image.FromFile(imagePath);
            SpriteImageDictionary[imagePath] = image;
            return image;
        }
    }
}
