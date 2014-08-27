namespace Vantage
{
    using System.Collections.Generic;

    using SharpDX;

    using Vantage.Animation2D;
    using Vantage.Animation3D;

    public class Storyboard
    {
        public static readonly Vector2 ViewportSize = new Vector2(854, 480);

        public static readonly Rectangle ViewportBounds = new Rectangle(-107, 0, 854, 480);

        public static readonly int DefaultResolutionWidth = 1366;

        public static readonly int DefaultResolutionHeight = 768;

        public Storyboard()
            : this(1366, 768)
        {
        }

        public Storyboard(int width, int height)
        {
            Sprite2Ds = new List<Sprite2D>();
            Scene3Ds = new List<Scene3D>();
            Resolution = new Vector2(width, height);
        }

        public IList<Sprite2D> Sprite2Ds { get; set; }

        public IList<Scene3D> Scene3Ds { get; set; }

        public Vector2 Resolution { get; set; }

        public Sprite2D NewSprite2D(string image, string layer, string origin)
        {
            var sprite = new Sprite2D(image, layer, origin);
            Sprite2Ds.Add(sprite);
            return sprite;
        }

        public Sprite2D NewSprite2D(string image)
        {
            return NewSprite2D(image, "Foreground", "Centre");
        }

        public Scene3D NewScene3D(float bpm, float startTime, float endTime)
        {
            return NewScene3D(bpm, startTime, endTime, 8.0f);
        }

        public Scene3D NewScene3D(float bpm, float startTime, float endTime, float rendersPerBeat)
        {
            var scene = new Scene3D(bpm, startTime, endTime, rendersPerBeat, Resolution.X, Resolution.Y);
            Scene3Ds.Add(scene);
            return scene;
        }

        public string ToOsbString()
        {
            string header = @"[Events]
//Background and Video events
//Storyboard Layer 0 (Background)
//Storyboard Layer 1 (Fail)
//Storyboard Layer 2 (Pass)
//Storyboard Layer 3 (Foreground)";

            string[] stringArray = new string[Scene3Ds.Count + Sprite2Ds.Count + 1];
            for (int i = 0; i < Scene3Ds.Count; i++)
            {
                stringArray[i] = Scene3Ds[i].ToOsbString();
            }

            // 2D
            for (int i = 0; i < Sprite2Ds.Count; i++)
            {
                stringArray[i + Scene3Ds.Count] = Sprite2Ds[i].ToOsbString();
            }

            stringArray[Scene3Ds.Count + Sprite2Ds.Count] = @"//Storyboard Sound Samples";
            return header + "\n" + string.Join("\n", stringArray);
        }
    }
}
