namespace CourseProject.BLL.Entities;

public class Stadium
{
    private int _id;
    private string _name = string.Empty;
    private int _numberOfSeats;
    private decimal _pricePerSeat;
    private List<int> _gameIds = new();

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value ?? string.Empty; }
    }

    public int NumberOfSeats
    {
        get { return _numberOfSeats; }
        set { _numberOfSeats = value; }
    }

    public decimal PricePerSeat
    {
        get { return _pricePerSeat; }
        set { _pricePerSeat = value; }
    }

    public List<int> GameIds
    {
        get { return _gameIds; }
        set { _gameIds = value ?? new List<int>(); }
    }
}