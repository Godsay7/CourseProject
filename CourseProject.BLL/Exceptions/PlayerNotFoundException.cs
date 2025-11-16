namespace CourseProject.BLL.Exceptions;

public class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException(int playerId)
        : base($"Player with ID {playerId} was not found.")
    {
    }

    public PlayerNotFoundException(string message) : base(message)
    {
    }
}

