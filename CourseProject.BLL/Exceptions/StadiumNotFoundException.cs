namespace CourseProject.BLL.Exceptions;

public class StadiumNotFoundException : Exception
{
    public StadiumNotFoundException(int stadiumId)
        : base($"Stadium with ID {stadiumId} was not found.")
    {
    }

    public StadiumNotFoundException(string message) : base(message)
    {
    }
}

