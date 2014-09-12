namespace Vantage.Animation3D.Layers
{
    using Vantage.Animation3D.Animation.EasingCurves;

    public class UISelectionLayer : Layer
    {
        public UISelectionLayer(string imageName, double width, double height)
        {
            for (int i = 0; i < 4; i++)
            {
                var sprite = this.NewSprite(imageName);
                sprite.LockScale = true;
                double x;
                double y;
                switch (i)
                {
                    case 0:
                        x = (width - sprite.Width) / 2;
                        y = (height - sprite.Height) / 2;
                        sprite.SetPosition(0, -x, y, 0);
                        break;
                    case 1:
                        y = (width - sprite.Width) / 2;
                        x = (height - sprite.Height) / 2;
                        sprite.SetPosition(0, x, y, 0);
                        sprite.SetAngles(0, 0, 0, -90);
                        break;
                    case 2:
                        y = (width - sprite.Width) / 2;
                        x = (height - sprite.Height) / 2;
                        sprite.SetPosition(0, x, -y, 0);
                        sprite.SetAngles(0, 0, 0, -180);
                        break;
                    case 3:
                        x = (width - sprite.Width) / 2;
                        y = (height - sprite.Height) / 2;
                        sprite.SetPosition(0, -x, -y, 0);
                        sprite.SetAngles(0, 0, 0, -270);
                        break;
                }
            }

            this.SetScale(0, 1, BasicEasingCurve.Step);
        }

        public void Pulse(double startTime, double duration, double scaleFactor, IEasingCurve easingCurve)
        {
            this.SetScale(startTime, scaleFactor, scaleFactor, 1, easingCurve);
            this.SetScale(startTime + duration, 1, 1, 1, BasicEasingCurve.Step);
        }
    }
}
