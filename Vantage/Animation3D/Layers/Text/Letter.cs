namespace Vantage.Animation3D.Layers.Text
{
    public class Letter
    {
        public Letter(string directory, string imageName)
        {
            this.ImageName = imageName;
            string imagePath = System.IO.Path.Combine(directory, imageName);
            this.Width = System.Drawing.Image.FromFile(imagePath).Width;
        }

        public string ImageName { get; private set; }

        public int Width { get; private set; }
    }
}
