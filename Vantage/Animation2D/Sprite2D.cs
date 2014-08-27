namespace Vantage.Animation2D
{
    using System.Collections.Generic;

    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.Commands.Generators;
    using Vantage.Animation2D.OsbTypes;

    // TODO: faster to use enum, then convert to string when converting to .osb
    public class OsbSpriteLayer
    {
        public static readonly string Foreground = "Foreground";
        public static readonly string Background = "Background";
        public static readonly string Pass = "Pass";
        public static readonly string Fail = "Fail";
    }

    public class Sprite2D
    {
        public static readonly MoveCommandGenerator MoveGenerator =
            new MoveCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold);

        public static readonly RotateCommandGenerator RotateGenerator =
            new RotateCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.RotateThreshold);

        public static readonly ScaleCommandGenerator ScaleGenerator =
            new ScaleCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ScaleThreshold);

        public static readonly ColorCommandGenerator ColorGenerator =
            new ColorCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ColorThreshold);

        public static readonly FadeCommandGenerator FadeGenerator =
            new FadeCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.FadeThreshold);

        public Sprite2D(string image)
            : this(image, "Foreground", "Centre")
        {
        }

        public Sprite2D(string image, string layer, string origin)
            : this(image, layer, origin, false, false, false)
        {
        }

        public Sprite2D(string image, string layer, string origin, bool additive, bool horizontalFlip, bool verticalFlip)
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

        public IList<ICommand> Commands { get; private set; }

        public IList<Sprite2DState> States { get; private set; }

        public string ImageName { get; set; }

        public string Layer { get; set; }

        public string Origin { get; set; }

        public bool Additive { get; set; }

        public bool HorizontalFlip { get; set; }

        public bool VerticalFlip { get; set; }

        public void AddCommandsFromStates()
        {
            MoveGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold;
            RotateGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.RotateThreshold;
            ScaleGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.ScaleThreshold;
            ColorGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.ColorThreshold;
            FadeGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.FadeThreshold;

            Sprite2DState initialState = this.States[0];
            float initialTime = initialState.Time;
            bool initialVisible = initialState.Visible;
            bool everVisible = initialVisible;

            MoveGenerator.Set(initialTime, initialState.Position, initialVisible);
            RotateGenerator.Set(initialTime, initialState.Rotation, initialVisible);
            ScaleGenerator.Set(initialTime, initialState.Scale, initialVisible);
            ColorGenerator.Set(initialTime, initialState.Color, initialVisible);
            FadeGenerator.Set(initialTime, initialState.Opacity, initialVisible);

            for (int i = 1; i < this.States.Count - 1; i++)
            {
                Sprite2DState state = this.States[i];
                float time = state.Time;
                bool visible = state.Visible;
                if (visible)
                {
                    everVisible = true;
                }

                this.AddCommand(MoveGenerator.Generate(time, state.Position, visible));
                this.AddCommand(RotateGenerator.Generate(time, state.Rotation, visible));
                this.AddCommand(ScaleGenerator.Generate(time, state.Scale, visible));
                this.AddCommand(ColorGenerator.Generate(time, (OsbColor)state.Color, visible));
                this.AddCommand(FadeGenerator.Generate(time, state.Opacity, visible));
            }

            Sprite2DState finalState = this.States[this.States.Count - 1];
            float finalTime = finalState.Time;

            this.AddCommand(MoveGenerator.Generate(finalTime, finalState.Position, false));
            this.AddCommand(RotateGenerator.Generate(finalTime, finalState.Rotation, false));
            this.AddCommand(ScaleGenerator.Generate(finalTime, finalState.Scale, false));
            this.AddCommand(ColorGenerator.Generate(finalTime, finalState.Color, false));
            this.AddCommand(FadeGenerator.Generate(finalTime, finalState.Opacity, false));

            if (!everVisible)
            {
                return;
            }

            if (this.Additive)
            {
                this.Commands.Add(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.AdditiveBlending));
            }

            if (this.HorizontalFlip)
            {
                this.Commands.Add(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.FlipHorizontal));
            }

            if (this.VerticalFlip)
            {
                this.Commands.Add(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.FlipVertical));
            }

            OsbColor finalColor = finalState.Color;
            if (!ColorGenerator.IssuedCommand && finalColor != OsbColor.White)
            {
                this.Commands.Add(new ColorCommand(0, initialTime, finalTime, finalColor, finalColor));
            }
        }

        public void AddCommand(ICommand command)
        {
            if (command != null)
            {
                this.Commands.Add(command);
            }
        }

        public void Move(int easing, float startTime, float endTime, OsbPosition startPosition, OsbPosition endPosition)
        {
            this.Commands.Add(new MoveCommand(easing, startTime, endTime, startPosition, endPosition));
        }

        public void Rotate(int easing, float startTime, float endTime, OsbDecimal startRotation, OsbDecimal endRotation)
        {
            this.Commands.Add(new RotateCommand(easing, startTime, endTime, startRotation, endRotation));
        }

        public void Scale(int easing, float startTime, float endTime, OsbDecimal startScale, OsbDecimal endScale)
        {
            this.Commands.Add(new ScaleCommand(easing, startTime, endTime, startScale, endScale));
        }

        public void VScale(int easing, float startTime, float endTime, OsbScale startScale, OsbScale endScale)
        {
            this.Commands.Add(new VScaleCommand(easing, startTime, endTime, startScale, endScale));
        }

        public void Color(int easing, float startTime, float endTime, OsbColor startColor, OsbColor endColor)
        {
            this.Commands.Add(new ColorCommand(easing, startTime, endTime, startColor, endColor));
        }

        public void Fade(int easing, float startTime, float endTime, OsbDecimal startOpacity, OsbDecimal endOpacity)
        {
            this.Commands.Add(new FadeCommand(easing, startTime, endTime, startOpacity, endOpacity));
        }

        public void Parameter(int easing, float startTime, float endTime, OsbParameter parameter)
        {
            this.Commands.Add(new ParameterCommand(easing, startTime, endTime, parameter));
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

        public void ClearCommands()
        {
            this.Commands.Clear();
        }
    }
}
