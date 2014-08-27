namespace Vantage
{
    using System;

    public sealed class StoryboardSettings
    {
        private static readonly StoryboardSettings InstancePrivate = new StoryboardSettings();

        private string directory;

        private StoryboardSettings()
        {
            this.SceneConversionSettings = new SceneConversionSettings();
        }

        public static StoryboardSettings Instance
        {
            get
            {
                return InstancePrivate;
            }
        }

        public string Directory
        {
            get
            {
                if (this.directory == null)
                {
                    throw new Exception("StoryboardSettings.Directory not initialized");
                }

                return this.directory;
            }

            set
            {
                this.directory = value;
            }
        }

        public SceneConversionSettings SceneConversionSettings { get; set; }
    }
}
