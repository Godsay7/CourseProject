namespace CourseProject.BLL.Exceptions;

public class GameNotFoundException : Exception
{
    public GameNotFoundException(int gameId)
        : base($"Game with ID {gameId} was not found.")
    {
    }

    public GameNotFoundException(string message) : base(message)
    {
    }
}

