namespace Vantage.Animation2D.Commands
{
    public interface ICommand
    {
		float StartTime { get; set; }
		
		float EndTime { get; set; }

        string ToOsbString();
    }
}