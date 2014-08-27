namespace Vantage
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class StoryboardSettings
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", 
            "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        private static readonly StoryboardSettings instance = new StoryboardSettings();

        private string directory;

        private StoryboardSettings()
        {
            this.SceneConversionSettings = new SceneConversionSettings();
        }

        public static StoryboardSettings Instance
        {
            get
            {
                return instance;
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
