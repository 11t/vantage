namespace Vantage
{
    public abstract class StoryboardGenerator
    {
        public Storyboard Storyboard { get; set; }

        public virtual void WriteStoryboard(string mapFolderPath, string osbFileName)
        {
            string osbFilePath = System.IO.Path.Combine(mapFolderPath, osbFileName);
            StoryboardSettings.Instance.Directory = mapFolderPath;
            this.Generate();
            System.IO.File.WriteAllText(osbFilePath, this.Storyboard.ToOsbString());
        }

        protected abstract void Generate();
    }
}