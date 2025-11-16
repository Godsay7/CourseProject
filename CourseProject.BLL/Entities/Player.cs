namespace CourseProject.BLL.Entities;

public class Player
{
    private int _id;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private DateTime _dateOfBirth;
    private string _status = string.Empty;
    private string _healthStatus = string.Empty;
    private decimal _salary;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value ?? string.Empty; }
    }

    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value ?? string.Empty; }
    }

    public DateTime DateOfBirth
    {
        get { return _dateOfBirth; }
        set { _dateOfBirth = value; }
    }

    public string Status
    {
        get { return _status; }
        set { _status = value ?? string.Empty; }
    }

    public string HealthStatus
    {
        get { return _healthStatus; }
        set { _healthStatus = value ?? string.Empty; }
    }

    public decimal Salary
    {
        get { return _salary; }
        set { _salary = value; }
    }
}