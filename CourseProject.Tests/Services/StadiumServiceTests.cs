using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.BLL.Services;
using CourseProject.Tests.Repositories;

namespace CourseProject.Tests.Services;


[TestClass]
public class StadiumServiceTests
{
    private IStadiumService _stadiumService = null!;
    private MockRepository<Stadium> _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = new MockRepository<Stadium>();
        _stadiumService = new StadiumService(_repository);
    }

    [TestMethod]
    public void GetAllStadiums_ShouldReturnAllStadiums()
    {
        // Arrange
        var stadium1 = new Stadium { Id = 1, Name = "Stadium A", NumberOfSeats = 10000 };
        var stadium2 = new Stadium { Id = 2, Name = "Stadium B", NumberOfSeats = 20000 };
        _repository.Add(stadium1);
        _repository.Add(stadium2);

        // Act
        var result = _stadiumService.GetAllEntity();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void GetStadiumById_WhenStadiumExists_ShouldReturnStadium()
    {
        // Arrange
        var stadium = new Stadium { Id = 1, Name = "Stadium A", NumberOfSeats = 10000 };
        _repository.Add(stadium);

        // Act
        var result = _stadiumService.GetByIdEntity(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Stadium A", result.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(StadiumNotFoundException))]
    public void GetStadiumById_WhenStadiumNotFound_ShouldThrowException()
    {
        // Arrange - no stadiums added

        // Act
        _stadiumService.GetByIdEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void AddStadium_ShouldAddStadiumWithGeneratedId()
    {
        // Arrange
        var stadium = new Stadium
        {
            Name = "Stadium A",
            NumberOfSeats = 10000,
            PricePerSeat = 50.00m
        };

        // Act
        _stadiumService.AddEntity(stadium);

        // Assert
        Assert.AreEqual(1, stadium.Id);
        var allStadiums = _repository.GetAll();
        Assert.AreEqual(1, allStadiums.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddStadium_WhenStadiumIsNull_ShouldThrowException()
    {
        // Arrange
        Stadium? stadium = null;

        // Act
        _stadiumService.AddEntity(stadium!);

        // Assert - exception expected
    }

    [TestMethod]
    public void DeleteStadium_WhenStadiumExists_ShouldDeleteStadium()
    {
        // Arrange
        var stadium = new Stadium { Id = 1, Name = "Stadium A", NumberOfSeats = 10000 };
        _repository.Add(stadium);

        // Act
        _stadiumService.DeleteEntity(1);

        // Assert
        var allStadiums = _repository.GetAll();
        Assert.AreEqual(0, allStadiums.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(StadiumNotFoundException))]
    public void DeleteStadium_WhenStadiumNotFound_ShouldThrowException()
    {
        // Arrange - no stadiums added

        // Act
        _stadiumService.DeleteEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void SearchStadiums_ByName_ShouldReturnMatchingStadiums()
    {
        // Arrange
        var stadium1 = new Stadium { Id = 1, Name = "Old Trafford", NumberOfSeats = 75000 };
        var stadium2 = new Stadium { Id = 2, Name = "Wembley Stadium", NumberOfSeats = 90000 };
        var stadium3 = new Stadium { Id = 3, Name = "Anfield", NumberOfSeats = 54000 };
        _repository.Add(stadium1);
        _repository.Add(stadium2);
        _repository.Add(stadium3);

        // Act
        var result = _stadiumService.SearchStadiums("Stadium");

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Wembley Stadium", result[0].Name);
    }

    [TestMethod]
    public void SearchStadiums_WithEmptySearchTerm_ShouldReturnAllStadiums()
    {
        // Arrange
        var stadium1 = new Stadium { Id = 1, Name = "Stadium A" };
        var stadium2 = new Stadium { Id = 2, Name = "Stadium B" };
        _repository.Add(stadium1);
        _repository.Add(stadium2);

        // Act
        var result = _stadiumService.SearchStadiums("");

        // Assert
        Assert.AreEqual(2, result.Count);
    }
}