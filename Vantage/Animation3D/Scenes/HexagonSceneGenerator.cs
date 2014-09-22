namespace Vantage.Animation3D.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Layers;
    using Vantage.Animation3D.Layers.Text;

    public class HexagonSceneGenerator
    {
        protected static readonly Random Random = new Random();

        public HexagonSceneGenerator()
        {
            this.HexagonSpriteName = @"sb/hexagon.png";
            this.HexagonRadius = 120;
            this.HexagonThickness = 0;
            this.HexagonBeginScale = 10;
            this.HexagonAppearScale = 1;
            this.HexagonAppearTransitionDurationBeats = 1;

            this.HexagonTextScale = 0.3f;
            this.HexagonTextLetterSpacing = -14;

            this.MasterOpacity = 1;
        }

        public string HexagonSpriteName { get; set; }

        public float HexagonRadius { get; set; }

        public float HexagonThickness { get; set; }

        public float HexagonBeginScale { get; set; }

        public float HexagonAppearScale { get; set; }

        public float HexagonAppearTransitionDurationBeats { get; set; }

        public Font HexagonTextFont { get; set; }

        public int HexagonTextLetterSpacing { get; set; }

        public float HexagonTextScale { get; set; }

        public float MasterOpacity { get; set; }

        protected IEnumerator<Tuple<int, int>> GridPositionEnumerator { get; private set; }

        protected IEnumerator<Vector3> CameraPositionEnumerator { get; private set; }

        protected IEnumerator<OsbColor> ColorEnumerator { get; private set; }

        protected IEnumerator<float> AngleEnumerator { get; private set; }

        protected IEnumerator<string> TextEnumerator { get; private set; }

        public Scene3D MakeScene(Storyboard storyboard, float bpm, float startTime, BeatPattern beatPattern)
        {
            this.SetEnumerators(beatPattern);
            float beatDuration = 60000.0f / bpm;
            float endTime = startTime + (beatDuration * beatPattern.TotalBeats);
            Scene3D scene = storyboard.NewScene3D(bpm, startTime, endTime);

            Layer rl = scene.RootLayer;
            Camera mc = scene.MainCamera;

            Layer hexagonMasterLayer = rl.NewLayer();
            hexagonMasterLayer.SetOpacity(0, this.MasterOpacity);
            hexagonMasterLayer.Additive = true;

            float defaultZ = (float)-mc.NearPlaneWidth;

            List<float> beats = beatPattern.AbsoluteBeats().ToList();

            float previousAngle = 0;
            Vector2 previousPosition = Vector2.Zero;

            for (int i = 0; i < beats.Count - 1; i++)
            {
                this.EnumeratorsMoveNext();

                float beat = beats[i];
                float appearTime = (float)scene.Timing(0, beat);
                float beginTime = (float)scene.Timing(0, beat - this.HexagonAppearTransitionDurationBeats);
                float nextBeginTime = (float)scene.Timing(0, beats[i + 1] - 0.25);

                Tuple<int, int> gridPosition = this.GridPositionEnumerator.Current;
                Vector2 position = HexagonPosition(
                    gridPosition.Item1,
                    gridPosition.Item2,
                    this.HexagonRadius * this.HexagonAppearScale,
                    this.HexagonThickness);
                Vector2 slidePosition = this.CalculatePositionSlide(previousPosition, position);
                Vector3 hexagonPosition = new Vector3(position.X, position.Y, defaultZ);
                Vector3 cameraPosition = this.CameraPositionEnumerator.Current;
                Vector3 cameraSlidePosition = new Vector3(slidePosition.X, slidePosition.Y, cameraPosition.Z);

                float angle = this.AngleEnumerator.Current;
                float slideAngle = this.CalculateSlideAngle(previousAngle, angle);
                Quaternion rotation = Quaternion.RotationAxis(Vector3.ForwardRH, angle);
                Quaternion slideRotation = Quaternion.RotationAxis(Vector3.ForwardRH, slideAngle);

                var hexagonLayer = hexagonMasterLayer.NewLayer();
                hexagonLayer.Fade(0, beginTime, appearTime, 0, 1);
                hexagonLayer.SetPosition(scene.StartTime, hexagonPosition);
                hexagonLayer.SetScale(beginTime, this.HexagonBeginScale);
                hexagonLayer.SetScale(appearTime, this.HexagonAppearScale);
                hexagonLayer.SetAngles(beginTime, 0, 80, 0);
                hexagonLayer.SetAngles(appearTime, 0, 0, 0);

                OsbColor color = this.ColorEnumerator.Current;
                var hexagonSprite = hexagonLayer.NewSprite(this.HexagonSpriteName);
                hexagonSprite.SetColor(scene.StartTime, color);

                if (this.HexagonTextFont != null)
                {
                    string text = this.TextEnumerator.Current;
                    var hexagonText = new TextLayer<Sprite3D>(this.HexagonTextFont, this.HexagonTextLetterSpacing)
                                          {
                                              Parent =
                                                  hexagonLayer,
                                              Text = text
                                          };
                    hexagonText.SetRotation(beginTime, rotation);
                    hexagonText.SetScale(beginTime, this.HexagonTextScale, this.HexagonTextScale, 1);
                }

                Tuple<bool, bool, bool> cameraMovementFlags = this.CameraMovementForBeat(beat);
                bool moveCamera = cameraMovementFlags.Item1;
                bool rotateCamera = cameraMovementFlags.Item2;
                bool slideCamera = cameraMovementFlags.Item3;

                if (moveCamera)
                {
                    mc.SetPosition(appearTime, cameraPosition);
                    if (slideCamera)
                    {
                        cameraPosition = cameraSlidePosition;
                        position = slidePosition;
                    }

                    mc.SetPosition(nextBeginTime, cameraPosition);
                    previousPosition = position;
                }

                if (rotateCamera)
                {
                    mc.SetRotation(appearTime, rotation, CubicBezierEasingCurve.EaseOut);
                    if (slideCamera)
                    {
                        rotation = slideRotation;
                        angle = slideAngle;
                    }

                    mc.SetRotation(nextBeginTime, rotation, CubicBezierEasingCurve.EaseIn);
                    previousAngle = angle;
                }
            }

            return scene;
        }

        protected static Vector2 HexagonPosition(int gridX, int gridY, float radius, float thickness)
        {
            double sqrt3 = Math.Sqrt(3);
            double adjustedRadius = radius + (thickness / sqrt3);
            double innerRadius = 0.5 * adjustedRadius * sqrt3;

            float y = (float)(2 * innerRadius * gridY);
            if (Math.Abs(gridX) % 2 != 0)
            {
                y = (float)(y + innerRadius);
            }

            float x = (float)(1.5 * adjustedRadius * gridX);
            return new Vector2(x, y);
        }

        protected virtual Tuple<bool, bool, bool> CameraMovementForBeat(float beat)
        {
            bool moveCamera = false;
            bool rotateCamera = false;
            bool slideCamera = false;
            
            moveCamera = true;
            if (beat % 4 == 3)
            {
                rotateCamera = true;
            }
            else if (beat % 4 == 2.5)
            {
            }
            else
            {
                rotateCamera = true;
                slideCamera = true;
            }

            return new Tuple<bool, bool, bool>(moveCamera, rotateCamera, slideCamera);
        }

        protected virtual IEnumerable<Vector3> CameraPositionEnumerable(BeatPattern beatPattern)
        {
            foreach (var beat in beatPattern.AbsoluteBeats())
            {
                Tuple<int, int> gridPosition = this.GridPositionEnumerator.Current;
                Vector2 position = HexagonPosition(
                    gridPosition.Item1,
                    gridPosition.Item2,
                    this.HexagonRadius * this.HexagonAppearScale,
                    this.HexagonThickness);
                float z = 0;
                if (beat % 2 == 1)
                {
                    z = -500;
                }

                yield return new Vector3(position.X, position.Y, z);
            }
        }

        protected virtual IEnumerable<OsbColor> HexagonColorEnumerable(BeatPattern beatPattern)
        {
            List<OsbColor> colors = new List<OsbColor>
                                        {
                                            OsbColor.Red,
                                            OsbColor.Lime,
                                            OsbColor.Yellow,
                                            OsbColor.Fuchsia,
                                            OsbColor.Aqua,
                                        };
            foreach (var beat in beatPattern.AbsoluteBeats())
            {
                if (beat <= 31 && beat >= 29)
                {
                    yield return OsbColor.White;
                }
                else
                {
                    yield return colors[Random.Next(colors.Count)];
                }
            }
        }

        protected virtual IEnumerable<float> HexagonAngleEnumerable(BeatPattern beatPattern)
        {
            for (int i = 0;; i++)
            {
                yield return (float)(Random.Next(0, 5) * Math.PI / 3.0);
            }
        }

        protected virtual IEnumerable<string> HexagonTextEnumerable(BeatPattern beatPattern)
        {
            for (int i = 0;; i++)
            {
                // yield return "hexagon" + i;
                Tuple<int, int> gridPosition = this.GridPositionEnumerator.Current;
                yield return gridPosition.Item1 + "," + gridPosition.Item2;
            }
        }

        protected virtual IEnumerable<Tuple<int, int>> HexagonGridPositionEnumerable(BeatPattern beatPattern)
        {
            List<int> previousGridLocationsX = new List<int> { 0 };
            List<int> previousGridLocationsY = new List<int> { 0 };
            const int GridRange = 1;

            for (int i = 0;; i++)
            {
                int indexRange = 1;
                int gridX;
                int gridY;
                do
                {
                    // Get the grid location of one of the previous indexRange hexagons
                    int indexOffset = Random.Next(1, indexRange + 1);
                    int index = Math.Max(previousGridLocationsX.Count - indexOffset, 0);
                    int previousGridX = previousGridLocationsX[index];
                    int previousGridY = previousGridLocationsY[index];

                    // Generate a random grid position close to the selected previous hexagon position
                    gridX = previousGridX + Random.Next(-GridRange, GridRange + 1);
                    gridY = previousGridY + Random.Next(-GridRange, GridRange + 1);

                    // indexRange++;
                }
                while (previousGridLocationsX.Contains(gridX) && previousGridLocationsY.Contains(gridY));

                previousGridLocationsX.Add(gridX);
                previousGridLocationsY.Add(gridY);

                yield return new Tuple<int, int>(gridX, gridY);
            }
        }

        protected virtual float CalculateSlideAngle(float previousAngle, float currentAngle)
        {
            float angleSlideAmount = (float)(Random.Next(20, 160) * Math.PI / 180.0);
            if (Math3D.IsClockwiseRotation(previousAngle, currentAngle))
            {
                angleSlideAmount *= -1;
            }

            return (float)((currentAngle + angleSlideAmount) % (2 * Math.PI));
        }

        protected virtual Vector2 CalculatePositionSlide(Vector2 previousPosition, Vector2 currentPosition)
        {
            float slideDistance = Random.Next(20, 200);
            Vector2 slideDirection = Vector2.Normalize(currentPosition - previousPosition);
            return currentPosition + (slideDirection * slideDistance);
        }

        private void SetEnumerators(BeatPattern beatPattern)
        {
            this.GridPositionEnumerator = this.HexagonGridPositionEnumerable(beatPattern).GetEnumerator();
            this.CameraPositionEnumerator = this.CameraPositionEnumerable(beatPattern).GetEnumerator();
            this.AngleEnumerator = this.HexagonAngleEnumerable(beatPattern).GetEnumerator();
            this.ColorEnumerator = this.HexagonColorEnumerable(beatPattern).GetEnumerator();
            this.TextEnumerator = this.HexagonTextEnumerable(beatPattern).GetEnumerator();
        }

        private void EnumeratorsMoveNext()
        {
            this.GridPositionEnumerator.MoveNext();
            this.CameraPositionEnumerator.MoveNext();
            this.AngleEnumerator.MoveNext();
            this.ColorEnumerator.MoveNext();
            this.TextEnumerator.MoveNext();
        }
    }
}
