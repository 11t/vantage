namespace Okaerinasai
{
    using System;
    using System.Collections.Generic;

    using SharpDX;

    using Vantage;
    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Scenes;

    public class OkaerinasaiHexagonSceneGenerator : HexagonSceneGenerator
    {
        /*
        protected override Tuple<bool, bool, bool> CameraMovementForBeat(float beat)
        {
            bool moveCamera = true;
            bool rotateCamera = true;
            bool slideCamera = true;
            return new Tuple<bool, bool, bool>(moveCamera, rotateCamera, slideCamera);
        }
        */

        protected override IEnumerable<OsbColor> HexagonColorEnumerable(BeatPattern beatPattern)
        {
            var colors = new List<OsbColor> { OsbColor.Yellow, OsbColor.Aqua };
            foreach (var beat in beatPattern.AbsoluteBeats())
            {
                if (beat >= 30 && beat <= 32)
                {
                    yield return OsbColor.White;
                    continue;
                }

                yield return colors[Random.Next(0, 2)];
            }
        }

        protected override IEnumerable<string> HexagonTextEnumerable(BeatPattern beatPattern)
        {
            for (int i = 0;; i++)
            {
                // yield return "hexagon" + i;
                Vector3 position = this.CameraPositionEnumerator.Current;
                yield return (int)position.X + ", " + (int)position.Y;
            }
        }

        protected override IEnumerable<Vector3> CameraPositionEnumerable(BeatPattern beatPattern)
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
                    z = 500;
                }

                if (beat >= 30 && beat <= 32)
                {
                    z = 500 + (beat - 30) * 200;
                    yield return new Vector3(position.X, position.Y, -z);
                    continue;
                }

                if (beat > 32)
                {
                    z += 400;
                }

                yield return new Vector3(position.X, position.Y, -z);
            }
        }

        protected override float CalculateSlideAngle(float previousAngle, float currentAngle)
        {
            float angleSlideAmount = (float)(Random.Next(20, 60) * Math.PI / 180.0);
            if (Math3D.IsClockwiseRotation(previousAngle, currentAngle))
            {
                angleSlideAmount *= -1;
            }

            return (float)((currentAngle + angleSlideAmount) % (2 * Math.PI));
        }

        protected override IEnumerable<Tuple<int, int>> HexagonGridPositionEnumerable(BeatPattern beatPattern)
        {
            List<int> previousGridLocationsX = new List<int> { 0 };
            List<int> previousGridLocationsY = new List<int> { 0 };
            const int GridRange = 1;

            Tuple<int, int> position3032 = new Tuple<int, int>(0, 0);
            foreach (var beat in beatPattern.AbsoluteBeats())
            {
                if (beat > 30 && beat <= 32)
                {
                    yield return position3032;
                    continue;
                }
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

                var position = new Tuple<int, int>(gridX, gridY);
                if (beat == 30)
                {
                    position3032 = position;
                }

                yield return position;
            }
        }

        protected override IEnumerable<float> HexagonAngleEnumerable(BeatPattern beatPattern)
        {
            foreach (var beat in beatPattern.AbsoluteBeats())
            {
                if (beat >= 30 && beat <= 32)
                {
                    yield return 0;
                    continue;
                }
                yield return (float)(Random.Next(0, 5) * Math.PI / 3.0);
            }
        }
    }
}
