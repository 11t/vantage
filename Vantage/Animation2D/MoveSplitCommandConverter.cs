namespace Vantage.Animation2D
{
    using System.Collections.Generic;

    using Vantage.Animation2D.Commands;
    using Vantage.Animation2D.Commands.Generators;
    using Vantage.Animation2D.OsbTypes;

    public static class MoveSplitCommandConverter
    {
        /// <summary>
        /// The generator used for creating Color commands from Sprite2DState objects.
        /// </summary>
        private static readonly ColorCommandGenerator ColorGenerator =
            new ColorCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ColorThreshold);

        /// <summary>
        /// The generator used for creating Fade commands from Sprite2DState objects.
        /// </summary>
        private static readonly FadeCommandGenerator FadeGenerator =
            new FadeCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.FadeThreshold);

        /// <summary>
        /// The generator used for creating Move commands from Sprite2DState objects.
        /// </summary>
        private static readonly MoveXCommandGenerator MoveXGenerator =
            new MoveXCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold);

        /// <summary>
        /// The generator used for creating Move commands from Sprite2DState objects.
        /// </summary>
        private static readonly MoveYCommandGenerator MoveYGenerator =
            new MoveYCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold);

        /// <summary>
        /// The generator used for creating Rotate commands from Sprite2DState objects.
        /// </summary>
        private static readonly RotateCommandGenerator RotateGenerator =
            new RotateCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.RotateThreshold);

        /// <summary>
        /// The generator used for creating Scale and VScale commands from Sprite2DState objects.
        /// </summary>
        private static readonly ScaleCommandGenerator ScaleGenerator =
            new ScaleCommandGenerator(StoryboardSettings.Instance.SceneConversionSettings.ScaleThreshold);

        public static void AddCommandsFromStateList(
            IList<ICommand> commandList,
            IList<Sprite2DState> stateList,
            bool additive,
            bool horizontalFlip,
            bool verticalFlip)
        {
            ResetGeneratorIssuedCommands();

            Sprite2DState initialState = stateList[0];
            double initialTime = initialState.Time;
            bool initialVisible = initialState.Visible;
            bool everVisible = initialVisible;

            UpdateGeneratorAllowedErrors(initialTime);
            SetGenerators(initialTime, initialState, initialVisible);

            for (int i = 1; i < stateList.Count - 1; i++)
            {
                Sprite2DState state = stateList[i];
                double time = state.Time;
                UpdateGeneratorAllowedErrors(time);
                bool visible = state.Visible;
                if (visible)
                {
                    everVisible = true;
                }

                AddGeneratedCommands(commandList, time, state, visible);
            }

            Sprite2DState finalState = stateList[stateList.Count - 1];
            double finalTime = finalState.Time;

            AddGeneratedCommands(commandList, finalTime, finalState, false);

            if (!everVisible)
            {
                return;
            }

            if (additive)
            {
                AddCommandToList(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.AdditiveBlending),
                    commandList);
            }

            if (horizontalFlip)
            {
                AddCommandToList(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.FlipHorizontal),
                    commandList);
            }

            if (verticalFlip)
            {
                AddCommandToList(
                    new ParameterCommand(0, initialTime, finalTime, OsbParameter.FlipVertical),
                    commandList);
            }

            OsbColor finalColor = finalState.Color;
            if (!ColorGenerator.IssuedCommand && finalColor != OsbColor.White)
            {
                AddCommandToList(new ColorCommand(0, initialTime, finalTime, finalColor, finalColor), commandList);
            }

            OsbScale finalScale = finalState.Scale;
            if (!ScaleGenerator.IssuedCommand && finalScale != OsbScale.One)
            {
                // TODO: Select for S or V
                AddCommandToList(new VScaleCommand(0, initialTime, finalTime, finalScale, finalScale), commandList);
            }
        }

        private static void AddCommandToList(ICommand command, IList<ICommand> commandList)
        {
            if (command != null)
            {
                commandList.Add(command);
            }
        }

        private static void AddGeneratedCommands(
            IList<ICommand> commandList,
            double time,
            Sprite2DState state,
            bool visible)
        {
            AddCommandToList(MoveXGenerator.Generate(time, state.Position.X, visible), commandList);
            AddCommandToList(MoveYGenerator.Generate(time, state.Position.Y, visible), commandList);
            AddCommandToList(RotateGenerator.Generate(time, state.Rotation, visible), commandList);
            AddCommandToList(ScaleGenerator.Generate(time, state.Scale, visible), commandList);
            AddCommandToList(ColorGenerator.Generate(time, state.Color, visible), commandList);
            AddCommandToList(FadeGenerator.Generate(time, state.Opacity, visible), commandList);
        }

        private static void ResetGeneratorIssuedCommands()
        {
            MoveXGenerator.IssuedCommand = false;
            MoveYGenerator.IssuedCommand = false;
            RotateGenerator.IssuedCommand = false;
            ScaleGenerator.IssuedCommand = false;
            ColorGenerator.IssuedCommand = false;
            FadeGenerator.IssuedCommand = false;
        }

        private static void SetGenerators(double time, Sprite2DState state, bool visible)
        {
            MoveXGenerator.Set(time, state.Position.X, visible);
            MoveYGenerator.Set(time, state.Position.Y, visible);
            RotateGenerator.Set(time, state.Rotation, visible);
            ScaleGenerator.Set(time, state.Scale, visible);
            ColorGenerator.Set(time, state.Color, visible);
            FadeGenerator.Set(time, state.Opacity, visible);
        }

        private static void UpdateGeneratorAllowedErrors(double time)
        {
            StoryboardSettings.Instance.SceneConversionSettings.UpdateToTime(time);

            MoveXGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold;
            MoveYGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.MoveThreshold;
            RotateGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.RotateThreshold;
            ScaleGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.ScaleThreshold;
            ColorGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.ColorThreshold;
            FadeGenerator.AllowedError = StoryboardSettings.Instance.SceneConversionSettings.FadeThreshold;
        }
    }
}
