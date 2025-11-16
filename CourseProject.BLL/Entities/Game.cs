namespace CourseProject.BLL.Entities;

public class Game
{
    private int _id;
    private DateTime _date;
    private string _venue = string.Empty;
    private string _opponentTeam = string.Empty;
    private int _spectators;
    private string _result = "Not yet played";
    private List<int> _playerIds = new();

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public DateTime Date
    {
        get { return _date; }
        set { _date = value; }
    }

    public string Venue
    {
        get { return _venue; }
        set { _venue = value ?? string.Empty; }
    }

    public string OpponentTeam
    {
        get { return _opponentTeam; }
        set { _opponentTeam = value ?? string.Empty; }
    }

    public int Spectators
    {
        get { return _spectators; }
        set { _spectators = value; }
    }

    public string Result
    {
        get { return _result; }
        set { _result = value ?? "Not yet played"; }
    }

    public List<int> PlayerIds
    {
        get { return _playerIds; }
        set { _playerIds = value ?? new List<int>(); }
    }
}

