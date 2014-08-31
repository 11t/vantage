namespace Vantage.Animation2D.Commands
{
    public interface ICommand
    {
        #region Public Properties

        double EndTime { get; set; }

        double StartTime { get; set; }

        #endregion

        #region Public Methods and Operators

        string ToOsbString();

        #endregion
    }
}