namespace Vantage.OsuObjects
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Vantage.Animation2D.OsbTypes;

    public class OsuBeatmap
    {
        public OsuBeatmap()
        {
            this.HitObjects = new List<IHitObject>();
            this.ControlPoints = new List<ControlPoint>();
            this.ComboColors = new List<OsbColor>();
        }

        public IList<IHitObject> HitObjects { get; private set; }

        public IList<ControlPoint> ControlPoints { get; private set; }

        public IList<OsbColor> ComboColors { get; private set; }

        public double SliderVelocity { get; set; }

        public void SetHitObjectControlPoints()
        {
            foreach (var hitObject in this.HitObjects)
            {
                foreach (var controlPoint in this.ControlPoints)
                {
                    if (controlPoint.Time > hitObject.Time)
                    {
                        break;
                    }

                    if (controlPoint.IsTimingPoint)
                    {
                        hitObject.ControlPoint = controlPoint;
                    }
                }
            }
        }

        public void SetControlPointParents()
        {
            ControlPoint currentTimingPoint = null;
            foreach (var controlPoint in this.ControlPoints)
            {
                if (controlPoint.IsTimingPoint)
                {
                    currentTimingPoint = controlPoint;
                }
                else
                {
                    controlPoint.Parent = currentTimingPoint;
                }
            }
        }

        public void SetHitObjectColors()
        {
            int colorIndex = 0;
            foreach (var hitObject in this.HitObjects)
            {
                if (hitObject is Spinner)
                {
                    continue;
                }

                if (hitObject.NewCombo)
                {
                    colorIndex = (colorIndex + 1 + hitObject.ComboColorShiftAmount) % this.ComboColors.Count;
                }

                hitObject.Color = this.ComboColors[colorIndex];
            }
        }

        public Tuple<IHitObject, int> HitObjectNearTime(double time)
        {
            const double AllowedError = 8;
            foreach (var hitObject in this.HitObjects)
            {
                if (Math.Abs(hitObject.Time - time) < AllowedError)
                {
                    return new Tuple<IHitObject, int>(hitObject, 0);
                }

                if (hitObject is Slider)
                {
                    Slider slider = hitObject as Slider;
                    double sliderDuration = slider.Duration(this.SliderVelocity);
                    double sliderTime = slider.Time;
                    for (int i = 0; i < slider.RepeatAmount; i++)
                    {
                        sliderTime += sliderDuration;
                        if (Math.Abs(sliderTime - time) < AllowedError)
                        {
                            return new Tuple<IHitObject, int>(slider, i + 1);
                        }
                    }
                }
            }

            return null;
        }

        public void ReadFromFile(string filepath)
        {
            string osuString = File.ReadAllText(filepath);
            string[] data = osuString.Split(new[] { "[TimingPoints]" }, StringSplitOptions.None);
            string[] controlPointData = data[1].Split(new[] { "[Colours]" }, StringSplitOptions.None);
            string[] colorHitobjectData = controlPointData[1].Split(new[] { "[HitObjects]" }, StringSplitOptions.None);
            string[] controlPointStrings = controlPointData[0].Trim().Split('\n');
            string[] colorStrings = colorHitobjectData[0].Trim().Split('\n');
            string[] hitobjectStrings = colorHitobjectData[1].Trim().Split('\n');
            
            foreach (var controlPointString in controlPointStrings)
            {
                this.ControlPoints.Add(ControlPoint.FromOsuString(controlPointString));
            }
            
            foreach (var hitobjectString in hitobjectStrings)
            {
                this.HitObjects.Add(HitObject.FromOsuString(hitobjectString));
            }

            foreach (var colorString in colorStrings)
            {
                string[] colorData = colorString.Split(':')[1].Split(',');
                int r = int.Parse(colorData[0].Trim());
                int g = int.Parse(colorData[1].Trim());
                int b = int.Parse(colorData[2].Trim());
                var color = OsbColor.FromRgb((byte)r, (byte)g, (byte)b);
                this.ComboColors.Add(color);
            }
        }
    }
}
