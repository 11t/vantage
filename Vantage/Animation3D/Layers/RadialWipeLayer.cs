namespace Vantage.Animation3D.Layers
{
    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.EasingCurves;

    public class RadialWipeCounterclockwiseLayer : Layer
    {
        public RadialWipeCounterclockwiseLayer(
            string semiCircleImageName,
            string leftSpriteImageName,
            string rightSpriteImageName,
            OsbColor color)
        {
            var blackCircle = this.NewLayer();
            var blackLeft = blackCircle.NewSprite(semiCircleImageName);
            var blackRight = blackCircle.NewSprite(semiCircleImageName);
            blackRight.SetAngles(0, 0, 0, 180);
            blackCircle.SetColor(0, 0, 0, 0);

            var coloredCircleLeft = this.NewSprite(semiCircleImageName);
            coloredCircleLeft.SetColor(0, color);
            coloredCircleLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteCircleLeftRotating = this.NewSprite(semiCircleImageName);
            whiteCircleLeftRotating.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredSpriteRight = this.NewSprite(rightSpriteImageName);
            coloredSpriteRight.SetColor(0, color);
            coloredSpriteRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleRightAdditive = this.NewSprite(semiCircleImageName);
            coloredCircleRightAdditive.SetColor(0, color);
            coloredCircleRightAdditive.Additive = true;
            coloredCircleRightAdditive.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteCircleRight = this.NewSprite(semiCircleImageName);
            whiteCircleRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredSpriteLeft = this.NewSprite(leftSpriteImageName);
            coloredSpriteLeft.SetColor(0, color);
            coloredSpriteLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleLeftAdditive = this.NewSprite(semiCircleImageName);
            coloredCircleLeftAdditive.SetColor(0, color);
            coloredCircleLeftAdditive.Additive = true;
            coloredCircleLeftAdditive.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleRight = this.NewSprite(semiCircleImageName);
            coloredCircleRight.SetColor(0, color);
            coloredCircleRight.SetAngles(0, 0, 0, 180);
            coloredCircleRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteSpriteRight = this.NewSprite(rightSpriteImageName);
            whiteSpriteRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            this.ColoredCircleRight = coloredCircleRight;
            this.WhiteCircleLeft = whiteCircleLeftRotating;
            this.ColoredSpriteRight = coloredSpriteRight;
            this.ColoredCircleRightAdditive = coloredCircleRightAdditive;
            this.WhiteCircleRight = whiteCircleRight;
            this.ColoredSpriteLeft = coloredSpriteLeft;
            this.ColoredCircleLeftAdditive = coloredCircleLeftAdditive;
            this.WhiteSpriteRight = whiteSpriteRight;
        }

        public Sprite3D ColoredCircleRight { get; set; }

        public Sprite3D WhiteCircleRight { get; set; }

        public Sprite3D ColoredSpriteLeft { get; set; }

        public Sprite3D ColoredCircleLeftAdditive { get; set; }

        public Sprite3D WhiteCircleLeft { get; set; }

        public Sprite3D ColoredSpriteRight { get; set; }

        public Sprite3D ColoredCircleRightAdditive { get; set; }

        public Sprite3D ColoredCircleLeft { get; set; }

        public Sprite3D WhiteSpriteRight { get; set; }

        public void RadialWipe(double startTime, double endTime)
        {
            double halfTime = (endTime + startTime) / 2;

            this.WhiteCircleLeft.SetOpacity(halfTime, 1);
            this.WhiteCircleLeft.SetAngles(halfTime, 0, 0, 0);
            this.WhiteCircleLeft.SetAngles(endTime, 0, 0, -180);

            this.ColoredSpriteRight.SetOpacity(halfTime, 1);

            this.ColoredCircleRightAdditive.SetOpacity(halfTime, 1);
            this.ColoredCircleRightAdditive.SetAngles(halfTime, 0, 0, 180);
            this.ColoredCircleRightAdditive.SetAngles(endTime, 0, 0, 0);

            this.WhiteCircleRight.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.WhiteCircleRight.SetAngles(startTime, 0, 0, 180);
            this.WhiteCircleRight.SetAngles(halfTime, 0, 0, 0);

            this.ColoredSpriteLeft.SetOpacity(startTime, 1, BasicEasingCurve.Step);

            this.ColoredCircleLeftAdditive.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.ColoredCircleLeftAdditive.SetOpacity(halfTime, 0);
            this.ColoredCircleLeftAdditive.SetAngles(startTime, 0, 0, 0);
            this.ColoredCircleLeftAdditive.SetAngles(halfTime, 0, 0, -180);
            
            this.ColoredCircleRight.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.ColoredCircleRight.SetOpacity(halfTime, 0);

            this.WhiteSpriteRight.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.WhiteSpriteRight.SetOpacity(halfTime, 0);
        }
    }

    public class RadialWipeClockwiseLayer : Layer
    {
        public RadialWipeClockwiseLayer(
            string semiCircleImageName,
            string leftSpriteImageName,
            string rightSpriteImageName,
            OsbColor color)
        {
            var coloredCircleRight = this.NewSprite(semiCircleImageName);
            coloredCircleRight.HorizontalFlip = true;
            coloredCircleRight.SetColor(0, color);
            coloredCircleRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteCircleRightRotating = this.NewSprite(semiCircleImageName);
            whiteCircleRightRotating.HorizontalFlip = true;
            whiteCircleRightRotating.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredSpriteLeft = this.NewSprite(leftSpriteImageName);
            coloredSpriteLeft.SetColor(0, color);
            coloredSpriteLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleLeftAdditive = this.NewSprite(semiCircleImageName);
            coloredCircleLeftAdditive.SetColor(0, color);
            coloredCircleLeftAdditive.Additive = true;
            coloredCircleLeftAdditive.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteCircleLeft = this.NewSprite(semiCircleImageName);
            whiteCircleLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredSpriteRight = this.NewSprite(rightSpriteImageName);
            coloredSpriteRight.SetColor(0, color);
            coloredSpriteRight.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleRightAdditive = this.NewSprite(semiCircleImageName);
            coloredCircleRightAdditive.SetColor(0, color);
            coloredCircleRightAdditive.Additive = true;
            coloredCircleRightAdditive.HorizontalFlip = true;
            coloredCircleRightAdditive.SetOpacity(0, 0, BasicEasingCurve.Step);

            var coloredCircleLeft = this.NewSprite(semiCircleImageName);
            coloredCircleLeft.SetColor(0, color);
            coloredCircleLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            var whiteSpriteLeft = this.NewSprite(leftSpriteImageName);
            whiteSpriteLeft.SetOpacity(0, 0, BasicEasingCurve.Step);

            this.ColoredCircleLeft = coloredCircleLeft;
            this.WhiteCircleRight = whiteCircleRightRotating;
            this.ColoredSpriteLeft = coloredSpriteLeft;
            this.ColoredCircleLeftAdditive = coloredCircleLeftAdditive;
            this.WhiteCircleLeft = whiteCircleLeft;
            this.ColoredSpriteRight = coloredSpriteRight;
            this.ColoredCircleRightAdditive = coloredCircleRightAdditive;
            this.WhiteSpriteLeft = whiteSpriteLeft;
        }

        public Sprite3D ColoredCircleRight { get; set; }

        public Sprite3D WhiteCircleRight { get; set; }

        public Sprite3D ColoredSpriteLeft { get; set; }

        public Sprite3D ColoredCircleLeftAdditive { get; set; }

        public Sprite3D WhiteCircleLeft { get; set; }

        public Sprite3D ColoredSpriteRight { get; set; }

        public Sprite3D ColoredCircleRightAdditive { get; set; }

        public Sprite3D ColoredCircleLeft { get; set; }

        public Sprite3D WhiteSpriteLeft { get; set; }

        public void RadialWipe(double startTime, double endTime)
        {
            double halfTime = (endTime + startTime) / 2;

            this.ColoredCircleLeft.SetOpacity(halfTime, 1);
            
            this.WhiteCircleRight.SetOpacity(halfTime, 1);
            this.WhiteCircleRight.SetAngles(halfTime, 0, 0, 0);
            this.WhiteCircleRight.SetAngles(endTime, 0, 0, 180);

            this.ColoredSpriteLeft.SetOpacity(halfTime, 1);

            this.ColoredCircleLeftAdditive.SetOpacity(halfTime, 1);
            this.ColoredCircleLeftAdditive.SetAngles(halfTime, 0, 0, 0);
            this.ColoredCircleLeftAdditive.SetAngles(endTime, 0, 0, 180);

            this.WhiteCircleLeft.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.WhiteCircleLeft.SetAngles(startTime, 0, 0, 0);
            this.WhiteCircleLeft.SetAngles(halfTime, 0, 0, 180);

            this.ColoredSpriteRight.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.ColoredSpriteRight.SetOpacity(endTime, 0);

            this.ColoredCircleRightAdditive.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.ColoredCircleRightAdditive.SetOpacity(halfTime, 0);
            this.ColoredCircleRightAdditive.SetAngles(startTime, 0, 0, 180);
            this.ColoredCircleRightAdditive.SetAngles(halfTime, 0, 0, 360);

            this.ColoredCircleLeft.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.ColoredCircleLeft.SetOpacity(halfTime, 0);

            this.WhiteSpriteLeft.SetOpacity(startTime, 1, BasicEasingCurve.Step);
            this.WhiteSpriteLeft.SetOpacity(halfTime, 0);
        }
    }
}
