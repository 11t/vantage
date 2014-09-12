namespace Vantage.Animation2D
{
    using System;
    using System.Collections.Generic;

    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.OsbTypes;

    /// <summary>
    /// Represents a 2D .osb Sprite.
    /// </summary>
    public class Sprite2D
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite2D"/> class.
        /// </summary>
        /// <param name="image">
        /// The Sprite's image name within the map directory.
        /// </param>
        public Sprite2D(string image)
            : this(image, "Foreground", "Centre")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite2D"/> class.
        /// </summary>
        /// <param name="image">
        /// The Sprite's image name within the map directory.
        /// </param>
        /// <param name="layer">
        /// The name of the .osb layer (Foreground, Background, etc.) where the Sprite resides.
        /// </param>
        /// <param name="origin">
        /// The name of the location of the anchor point (Centre, TopLeft, etc.) of the Sprite. 
        /// </param>
        public Sprite2D(string image, string layer, string origin)
            : this(image, layer, origin, false, false, false)
        {
        }

        public Sprite2D(
            string image, 
            string layer, 
            string origin, 
            bool additive, 
            bool horizontalFlip, 
            bool verticalFlip)
        {
            this.Commands = new List<ICommand>();
            this.States = new List<Sprite2DState>();
            this.ImageName = image;
            this.Layer = layer;
            this.Origin = origin;
            this.Additive = additive;
            this.HorizontalFlip = horizontalFlip;
            this.VerticalFlip = verticalFlip;
        }

        #endregion

        #region Public Properties

        public bool Additive { get; set; }

        public IList<ICommand> Commands { get; private set; }

        public bool HorizontalFlip { get; set; }

        public string ImageName { get; set; }

        public string Layer { get; set; }

        public string Origin { get; set; }

        public IList<Sprite2DState> States { get; private set; }

        public bool VerticalFlip { get; set; }

        protected CommandGroup CurrentCommandGroup { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddCommand(ICommand command)
        {
            if (command != null)
            {
                var commandGroup = command as CommandGroup;
                if (commandGroup != null)
                {
                    this.CurrentCommandGroup = commandGroup;
                    this.Commands.Add(commandGroup);
                }
                else if (this.CurrentCommandGroup != null)
                {
                    this.CurrentCommandGroup.AddCommand(command);
                }
                else
                {
                    this.Commands.Add(command);
                }
            }
        }

        public void AddCommandsFromStates()
        {
            MoveSplitCommandConverter.AddCommandsFromStateList(
                this.Commands,
                this.States,
                this.Additive,
                this.HorizontalFlip,
                this.VerticalFlip);
        }

        public void AdditiveP(double startTime, double endTime)
        {
            this.Parameter(0, startTime, endTime, OsbParameter.AdditiveBlending);
        }

        public void ClearCommands()
        {
            this.Commands.Clear();
        }

        public void Color(int easing, double startTime, double endTime, OsbColor startColor, OsbColor endColor)
        {
            this.Commands.Add(new ColorCommand(easing, startTime, endTime, startColor, endColor));
        }

        public void Color(double startTime, double endTime, OsbColor startColor, OsbColor endColor)
        {
            this.Color(0, startTime, endTime, startColor, endColor);
        }

        public void Color(int easing, double time, OsbColor color)
        {
            this.Color(easing, time, time, color, color);
        }

        public void Color(double time, OsbColor color)
        {
            this.Color(0, time, time, color, color);
        }

        public void Color(
            int easing,
            double startTime,
            double endTime, 
            double startRed, 
            double startGreen, 
            double startBlue, 
            double endRed, 
            double endGreen, 
            double endBlue)
        {
            this.Color(
                easing, 
                startTime, 
                endTime, 
                new OsbColor(startRed, startGreen, startBlue), 
                new OsbColor(endRed, endGreen, endBlue));
        }

        public void Color(
            double startTime,
            double endTime,
            double startRed, 
            double startGreen, 
            double startBlue, 
            double endRed, 
            double endGreen, 
            double endBlue)
        {
            this.Color(0, startTime, endTime, startRed, startGreen, startBlue, endRed, endGreen, endBlue);
        }

        public void Color(int easing, double time, double red, double green, double blue)
        {
            OsbColor color = new OsbColor(red, green, blue);
            this.Color(easing, time, time, color, color);
        }

        public void Color(double time, double red, double green, double blue)
        {
            this.Color(0, time, red, green, blue);
        }

        public void ColorHsb(
            int easing,
            double startTime,
            double endTime, 
            double startHue, 
            double startSaturation, 
            double startBrightness, 
            double endHue, 
            double endSaturation, 
            double endBrightness)
        {
            this.Color(
                easing, 
                startTime, 
                endTime, 
                OsbColor.FromHsb(startHue, startSaturation, startBrightness), 
                OsbColor.FromHsb(endHue, endSaturation, endBrightness));
        }

        public void ColorHsb(
            double startTime,
            double endTime, 
            double startHue, 
            double startSaturation, 
            double startBrightness, 
            double endHue, 
            double endSaturation, 
            double endBrightness)
        {
            this.ColorHsb(
                0, 
                startTime, 
                endTime, 
                startHue, 
                startSaturation, 
                startBrightness, 
                endHue, 
                endSaturation, 
                endBrightness);
        }

        public void ColorHsb(int easing, double time, double hue, double saturation, double brightness)
        {
            OsbColor color = OsbColor.FromHsb(hue, saturation, brightness);
            this.Color(easing, time, time, color, color);
        }

        public void ColorHsb(double time, double hue, double saturation, double brightness)
        {
            this.ColorHsb(0, time, hue, saturation, brightness);
        }

        public void EndCommandGroup()
        {
            this.CurrentCommandGroup = null;
        }

        public void Fade(int easing, double startTime, double endTime, OsbDecimal startOpacity, OsbDecimal endOpacity)
        {
            this.Commands.Add(new FadeCommand(easing, startTime, endTime, startOpacity, endOpacity));
        }

        public void Fade(double startTime, double endTime, OsbDecimal startOpacity, OsbDecimal endOpacity)
        {
            this.Fade(0, startTime, endTime, startOpacity, endOpacity);
        }

        public void Fade(int easing, double time, OsbDecimal opacity)
        {
            this.Fade(easing, time, time, opacity, opacity);
        }

        public void Fade(double time, OsbDecimal opacity)
        {
            this.Fade(0, time, time, opacity, opacity);
        }

        public void FlipH(double startTime, double endTime)
        {
            this.Parameter(0, startTime, endTime, OsbParameter.FlipHorizontal);
        }

        public void FlipV(double startTime, double endTime)
        {
            this.Parameter(0, startTime, endTime, OsbParameter.FlipVertical);
        }

        public double GetCommandsStartTime()
        {
            double commandsStartTime = double.MaxValue;
            foreach (ICommand command in this.Commands)
            {
                commandsStartTime = Math.Min(commandsStartTime, command.StartTime);
            }

            return commandsStartTime;
        }

        public double GetCommandsEndTime()
        {
            double commandsEndTime = 0;
            foreach (ICommand command in this.Commands)
            {
                commandsEndTime = Math.Max(commandsEndTime, command.EndTime);
            }

            return commandsEndTime;
        }

        public void Move(int easing, double startTime, double endTime, OsbPosition startPosition, OsbPosition endPosition)
        {
            this.Commands.Add(new MoveCommand(easing, startTime, endTime, startPosition, endPosition));
        }

        public void Move(double startTime, double endTime, OsbPosition startPosition, OsbPosition endPosition)
        {
            this.Move(0, startTime, endTime, startPosition, endPosition);
        }

        public void Move(int easing, double time, OsbPosition position)
        {
            this.Move(easing, time, time, position, position);
        }

        public void Move(double time, OsbPosition position)
        {
            this.Move(0, time, time, position, position);
        }

        public void Move(
            int easing,
            double startTime,
            double endTime, 
            double startX, 
            double startY, 
            double endX, 
            double endY)
        {
            this.Move(easing, startTime, endTime, new OsbPosition(startX, startY), new OsbPosition(endX, endY));
        }

        public void Move(double startTime, double endTime, double startX, double startY, double endX, double endY)
        {
            this.Move(0, startTime, endTime, startX, startY, endX, endY);
        }

        public void Move(int easing, double time, double x, double y)
        {
            this.Move(easing, time, new OsbPosition(x, y));
        }

        public void Move(double time, double x, double y)
        {
            this.Move(0, time, x, y);
        }

        public void MoveX(int easing, double startTime, double endTime, OsbDecimal startX, OsbDecimal endX)
        {
            this.Commands.Add(new MoveXCommand(easing, startTime, endTime, startX, endX));
        }

        public void MoveX(double startTime, double endTime, OsbDecimal startX, OsbDecimal endX)
        {
            this.MoveX(0, startTime, endTime, startX, endX);
        }

        public void MoveX(int easing, double time, OsbDecimal x)
        {
            this.MoveX(easing, time, time, x, x);
        }

        public void MoveX(double time, OsbDecimal x)
        {
            this.MoveX(0, time, time, x, x);
        }

        public void MoveY(int easing, double startTime, double endTime, OsbDecimal startY, OsbDecimal endY)
        {
            this.Commands.Add(new MoveYCommand(easing, startTime, endTime, startY, endY));
        }

        public void MoveY(double startTime, double endTime, OsbDecimal startY, OsbDecimal endY)
        {
            this.MoveY(0, startTime, endTime, startY, endY);
        }

        public void MoveY(int easing, double time, OsbDecimal y)
        {
            this.MoveY(easing, time, time, y, y);
        }

        public void MoveY(double time, OsbDecimal y)
        {
            this.MoveY(0, time, time, y, y);
        }

        public void Parameter(int easing, double startTime, double endTime, OsbParameter parameter)
        {
            this.Commands.Add(new ParameterCommand(easing, startTime, endTime, parameter));
        }

        public void Rotate(int easing, double startTime, double endTime, OsbDecimal startRotation, OsbDecimal endRotation)
        {
            this.Commands.Add(new RotateCommand(easing, startTime, endTime, startRotation, endRotation));
        }

        public void Rotate(double startTime, double endTime, OsbDecimal startRotation, OsbDecimal endRotation)
        {
            this.Rotate(0, startTime, endTime, startRotation, endRotation);
        }

        public void Rotate(int easing, double time, OsbDecimal rotation)
        {
            this.Rotate(easing, time, time, rotation, rotation);
        }

        public void Rotate(double time, OsbDecimal rotation)
        {
            this.Rotate(0, time, time, rotation, rotation);
        }

        public void Scale(int easing, double startTime, double endTime, OsbDecimal startScale, OsbDecimal endScale)
        {
            this.Commands.Add(new ScaleCommand(easing, startTime, endTime, startScale, endScale));
        }

        public void Scale(double startTime, double endTime, OsbDecimal startScale, OsbDecimal endScale)
        {
            this.Scale(0, startTime, endTime, startScale, endScale);
        }

        public void Scale(int easing, double time, OsbDecimal scale)
        {
            this.Scale(easing, time, time, scale, scale);
        }

        public void Scale(double time, OsbDecimal scale)
        {
            this.Scale(0, time, time, scale, scale);
        }

        public LoopCommand StartLoopGroup(double startTime, int loopCount)
        {
            LoopCommand loopCommand = new LoopCommand(startTime, loopCount);
            this.AddCommand(loopCommand);
            return loopCommand;
        }

        public TriggerCommand StartTriggerGroup(string triggerName, double startTime, double endTime)
        {
            TriggerCommand triggerCommand = new TriggerCommand(triggerName, startTime, endTime);
            this.AddCommand(triggerCommand);
            return triggerCommand;
        }

        public string ToOsbString()
        {
            if (this.Commands.Count <= 0)
            {
                return string.Empty;
            }

            string[] headerArray = { "Sprite", this.Layer, this.Origin, "\"" + this.ImageName + "\"", "320,240" };
            string header = string.Join(",", headerArray);
            string[] stringArray = new string[this.Commands.Count + 1];
            stringArray[0] = header;
            for (int i = 0; i < this.Commands.Count; i++)
            {
                stringArray[i + 1] = this.Commands[i].ToOsbString();
            }

            return string.Join("\n ", stringArray);
        }

        public void VScale(int easing, double startTime, double endTime, OsbScale startScale, OsbScale endScale)
        {
            this.Commands.Add(new VScaleCommand(easing, startTime, endTime, startScale, endScale));
        }

        public void VScale(double startTime, double endTime, OsbScale startScale, OsbScale endScale)
        {
            this.VScale(0, startTime, endTime, startScale, endScale);
        }

        public void VScale(
            int easing,
            double startTime,
            double endTime, 
            OsbDecimal startScaleX, 
            OsbDecimal startScaleY, 
            OsbDecimal endScaleX, 
            OsbDecimal endScaleY)
        {
            this.VScale(
                easing, 
                startTime, 
                endTime, 
                new OsbScale(startScaleX, startScaleY), 
                new OsbScale(endScaleX, endScaleY));
        }

        public void VScale(
            double startTime,
            double endTime, 
            OsbDecimal startScaleX, 
            OsbDecimal startScaleY, 
            OsbDecimal endScaleX, 
            OsbDecimal endScaleY)
        {
            this.VScale(0, startTime, endTime, startScaleX, startScaleY, endScaleX, endScaleY);
        }

        public void VScale(int easing, double time, OsbDecimal scaleX, OsbDecimal scaleY)
        {
            this.VScale(easing, time, time, scaleX, scaleY, scaleX, scaleY);
        }

        public void VScale(double time, OsbDecimal scaleX, OsbDecimal scaleY)
        {
            this.VScale(0, time, time, scaleX, scaleY, scaleX, scaleY);
        }

        #endregion
    }
}