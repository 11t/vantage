// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommand.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Vantage.Animation2D.Commands
{
    public interface ICommand
    {
        #region Public Properties

        float EndTime { get; set; }

        float StartTime { get; set; }

        #endregion

        #region Public Methods and Operators

        string ToOsbString();

        #endregion
    }
}