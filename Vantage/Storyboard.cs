﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Storyboard.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage
{
    using System.Collections.Generic;

    using SharpDX;

    using Vantage.Animation2D;
    using Vantage.Animation3D.Scenes;

    public class Storyboard
    {
        #region Static Fields

        public static readonly int DefaultResolutionHeight = 768;

        public static readonly int DefaultResolutionWidth = 1366;

        public static readonly Rectangle ViewportBounds = new Rectangle(-107, 0, 854, 480);

        public static readonly Vector2 ViewportSize = new Vector2(854, 480);

        #endregion

        #region Constructors and Destructors

        public Storyboard()
            : this(1366, 768)
        {
        }

        public Storyboard(int width, int height)
        {
            this.Sprite2Ds = new List<Sprite2D>();
            this.Scene3Ds = new List<Scene3D>();
            this.Resolution = new Vector2(width, height);
        }

        #endregion

        #region Public Properties

        public Vector2 Resolution { get; set; }

        public IList<Scene3D> Scene3Ds { get; set; }

        public IList<Sprite2D> Sprite2Ds { get; set; }

        #endregion

        #region Public Methods and Operators

        public Scene3D NewScene3D(double bpm, double startTime, double endTime)
        {
            return this.NewScene3D(bpm, startTime, endTime, 8.0f);
        }

        public Scene3D NewScene3D(double bpm, double startTime, double endTime, double rendersPerBeat)
        {
            var scene = new Scene3D(bpm, startTime, endTime, rendersPerBeat, this.Resolution.X, this.Resolution.Y);
            this.Scene3Ds.Add(scene);
            return scene;
        }

        public Sprite2D NewSprite2D(string image, string layer, string origin, int initialX, int initialY)
        {
            var sprite = new Sprite2D(image, layer, origin, initialX, initialY);
            this.Sprite2Ds.Add(sprite);
            return sprite;
        }

        public Sprite2D NewSprite2D(string image, string layer, string origin)
        {
            return this.NewSprite2D(image, layer, origin, 320, 240);
        }

        public Sprite2D NewSprite2D(string image)
        {
            return this.NewSprite2D(image, "Foreground", "Centre");
        }

        public Sprite2D NewUnregisteredSprite2D(string image, string layer, string origin)
        {
            return new Sprite2D(image, layer, origin);
        }

        public void RegisterSprite2D(Sprite2D sprite)
        {
            this.Sprite2Ds.Add(sprite);
        }

        public string ToOsbString()
        {
            const string Header = "[Events]\n";

            string[] stringArray = new string[this.Scene3Ds.Count + this.Sprite2Ds.Count];
            for (int i = 0; i < this.Scene3Ds.Count; i++)
            {
                stringArray[i] = this.Scene3Ds[i].ToOsbString();
            }

            // 2D
            for (int i = 0; i < this.Sprite2Ds.Count; i++)
            {
                stringArray[i + this.Scene3Ds.Count] = this.Sprite2Ds[i].ToOsbString();
            }

            return Header + string.Join("\n", stringArray) + "\n//Storyboard Sound Samples";
        }

        #endregion
    }
}