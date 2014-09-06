namespace Vantage.Animation2D.Commands 
{
    using System;
    using System.Collections.Generic;

    public abstract class CommandGroup : ICommand 
    {
        public CommandGroup() 
        {
            this.Commands = new List<ICommand>();
        }

        public double StartTime { get; set; }

        public virtual double EndTime { get; set; }

        public IList<ICommand> Commands { get; private set; }
        
        public void AddCommand(ICommand command) 
        {
            if (command != null) 
            {
                this.Commands.Add(command);
            }
        }

        public double GetCommandsStartTime() 
        {
            double commandsStartTime = double.MaxValue;
            foreach (ICommand command in this.Commands) 
            {
                commandsStartTime = Math.Min(commandsStartTime, command.EndTime);
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

        public string ToOsbString() 
        {
            if (this.Commands.Count <= 0) 
            {
                return string.Empty;
            }

            string header = this.GetOsbStringHeader();
            string[] stringArray = new string[this.Commands.Count + 1];
            stringArray[0] = header;
            for (int i = 0; i < this.Commands.Count; i++) 
            {
                stringArray[i + 1] = " " + this.Commands[i].ToOsbString();
            }

            return string.Join("\n ", stringArray);
        }

        protected abstract string GetOsbStringHeader();
    }
}
