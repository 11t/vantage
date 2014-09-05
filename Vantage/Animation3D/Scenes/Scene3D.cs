namespace Vantage.Animation3D.Scenes
{
    using System.Collections.Generic;

    using Vantage.Animation3D.Layers;

    public class Scene3D
    {
        private const double DefaultResolutionWidth = 1366;
        private const double DefaultResolutionHeight = 768;

        private double bpm;

        public Scene3D(double bpm, double startTime, double endTime, double rendersPerBeat, double width, double height)
        {
            this.BPM = bpm;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.RenderTimeStep = this.BeatDuration / rendersPerBeat;
            this.RootLayer = new Layer();
            this.MainCamera = new Camera(width, height);
            this.Sprites = new List<Sprite3D>();
            this.ConversionSettings = new SceneConversionSettings();
        }

        public Scene3D(double bpm, double startTime, double endTime)
            : this(bpm, startTime, endTime, 4.0f, DefaultResolutionWidth, DefaultResolutionHeight)
        {
        }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public double BPM
        {
            get
            {
                return this.bpm;
            }

            set
            {
                this.bpm = value;
                this.BeatDuration = 60000.0 / value;
            }
        }

        public double BeatDuration { get; private set; }

        public double RenderTimeStep { get; set; }

        public Layer RootLayer { get; set; }

        public Camera MainCamera { get; set; }

        public IList<Sprite3D> Sprites { get; set; }

        public SceneConversionSettings ConversionSettings { get; set; }

        // Depth-first search here SHOULD preserve sprite order.
        public void AddSpritesFromLayer(ILayer layer)
        {
            if (layer.GetType() == typeof(Sprite3D) || layer.GetType().IsSubclassOf(typeof(Sprite3D)))
            {
                this.Sprites.Add((Sprite3D)layer);
            }

            foreach (ILayer child in layer.Children)
            {
                this.AddSpritesFromLayer(child);
            }
        }

        public string ToOsbString()
        {
            StoryboardSettings.Instance.SceneConversionSettings = this.ConversionSettings;
            this.Sprites.Clear();
            this.AddSpritesFromLayer(this.RootLayer);

            for (double time = this.StartTime; time < this.EndTime; time += this.RenderTimeStep)
            {
                this.UpdateSpriteStatesToTime(time);
            }

            this.UpdateSpriteStatesToTime(this.EndTime);
            
            foreach (Sprite3D sprite in this.Sprites)
            {
                sprite.Representative.AddCommandsFromStates();
            }

            string[] stringArray = new string[this.Sprites.Count];
            for (int i = 0; i < this.Sprites.Count; i++)
            {
                stringArray[i] = this.Sprites[i].Representative.ToOsbString();
                if (this.Sprites[i].DebugTrack)
                {
                    var debugSprite = this.Sprites[i];
                    System.Diagnostics.Debug.WriteLine("break");
                }
            }

            this.ClearSpriteStates();
            return string.Join("\n", stringArray);
        }

        public void ClearSpriteStates()
        {
            foreach (Sprite3D sprite in this.Sprites)
            {
                sprite.Representative.States.Clear();
                sprite.Representative.Commands.Clear();
            }
        }

        public void UpdateSpriteStatesToTime(double time)
        {
            this.UpdateToTime(time);
            foreach (Sprite3D sprite in this.Sprites)
            {
                sprite.UpdateState(this.MainCamera);
                sprite.Representative.States.Add(sprite.State);
            }
        }

        public void UpdateToTime(double time)
        {
            this.RootLayer.UpdateToTime(time);
            this.MainCamera.UpdateToTime(time);
        }

        public double Timing(double measure, double beat)
        {
            double totalBeats = (4 * measure) + beat;
            double totalDuration = totalBeats * this.BeatDuration;
            return (float)(this.StartTime + totalDuration);
        }
    }
}
