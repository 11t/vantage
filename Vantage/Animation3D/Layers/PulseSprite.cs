namespace Vantage.Animation3D.Layers
{
    using System;
    using System.Collections.Generic;

    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.EasingCurves;

    public class PulseSprite : Sprite3D
    {
        public IList<Sprite3D> Afterimages { get; private set; }

        public PulseSprite(string imageName, int count, params OsbColor[] colors)
            : base(imageName)
        {
            this.Afterimages = new List<Sprite3D>(count);
            for (int i = 0; i < count; i++)
            {
                var afterimage = new Sprite3D(imageName) { Parent = this };
                afterimage.SetColor(0, colors[i]);
                afterimage.SetOpacity(0, 0, BasicEasingCurve.Step);
                afterimage.SetPosition(0, 0, 0, 0, BasicEasingCurve.Step);
                this.Afterimages[i] = afterimage;
            }
        }

        public void Pulse(double time, double duration, double radius)
        {
            int count = this.Afterimages.Count;
            double endTime = time + duration;
            double angleIncrement = 2 * Math.PI / this.Afterimages.Count;
            for (int i = 0; i < count; i++)
            {
                double angle = angleIncrement * i;
                double x = Math.Cos(angle) * radius;
                double y = Math.Sin(angle) * radius;
                var afterimage = this.Afterimages[i];
                afterimage.SetOpacity(time, 1, BasicEasingCurve.Step);
                afterimage.SetOpacity(endTime, 0, BasicEasingCurve.Step);
                afterimage.SetPosition(time, x, y, 0, BasicEasingCurve.Linear);
                afterimage.SetPosition(time, 0, 0, 0, BasicEasingCurve.Step);
            }
        }
    }
}
