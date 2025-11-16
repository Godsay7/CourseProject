using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.BLL.Services;
using CourseProject.Tests.Repositories;

namespace CourseProject.Tests.Services;

[TestClass]
public class PlayerServiceTests
{
    private IPlayerService _playerService = null!;
    private MockRepository<Player> _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = new MockRepository<Player>();
        _playerService = new PlayerService(_repository);
    }

    [TestMethod]
    public void GetAllPlayers_ShouldReturnAllPlayers()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        var player2 = new Player { Id = 2, FirstName = "Jane", LastName = "Smith" };
        _repository.Add(player1);
        _repository.Add(player2);

        // Act
        var result = _playerService.GetAllEntity();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Any(p => p.Id == 1));
        Assert.IsTrue(result.Any(p => p.Id == 2));
    }

    [TestMethod]
    public void GetAllPlayers_WhenNoPlayers_ShouldReturnEmptyList()
    {
        // Arrange - no players added

        // Act
        var result = _playerService.GetAllEntity();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetPlayerById_WhenPlayerExists_ShouldReturnPlayer()
    {
        // Arrange
        var player = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        _repository.Add(player);

        // Act
        var result = _playerService.GetByIdEntity(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("John", result.FirstName);
        Assert.AreEqual("Doe", result.LastName);
    }

    [TestMethod]
    [ExpectedException(typeof(PlayerNotFoundException))]
    public void GetPlayerById_WhenPlayerNotFound_ShouldThrowException()
    {
        // Arrange - no players added

        // Act
        _playerService.GetByIdEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void AddPlayer_ShouldAddPlayerWithGeneratedId()
    {
        // Arrange
        var player = new Player
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Status = "Active",
            HealthStatus = "Healthy",
            Salary = 50000
        };

        // Act
        _playerService.AddEntity(player);

        // Assert
        Assert.AreEqual(1, player.Id);
        var allPlayers = _repository.GetAll();
        Assert.AreEqual(1, allPlayers.Count);
        Assert.AreEqual("John", allPlayers[0].FirstName);
    }

    [TestMethod]
    public void AddPlayer_WithExistingId_ShouldUseProvidedId()
    {
        // Arrange
        var player = new Player { Id = 5, FirstName = "John", LastName = "Doe" };

        // Act
        _playerService.AddEntity(player);

        // Assert
        Assert.AreEqual(5, player.Id);
        var retrieved = _playerService.GetByIdEntity(5);
        Assert.IsNotNull(retrieved);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddPlayer_WhenPlayerIsNull_ShouldThrowException()
    {
        // Arrange
        Player? player = null;

        // Act
        _playerService.AddEntity(player!);

        // Assert - exception expected
    }

    [TestMethod]
    public void UpdatePlayer_WhenPlayerExists_ShouldUpdatePlayer()
    {
        // Arrange
        var player = new Player
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Status = "Active"
        };
        _repository.Add(player);

        var updatedPlayer = new Player
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            Status = "Inactive"
        };

        // Act
        _playerService.UpdateEntity(updatedPlayer);

        // Assert
        var result = _playerService.GetByIdEntity(1);
        Assert.AreEqual("Smith", result.LastName);
        Assert.AreEqual("Inactive", result.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(PlayerNotFoundException))]
    public void UpdatePlayer_WhenPlayerNotFound_ShouldThrowException()
    {
        // Arrange
        var player = new Player { Id = 999, FirstName = "John", LastName = "Doe" };

        // Act
        _playerService.UpdateEntity(player);

        // Assert - exception expected
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UpdatePlayer_WhenPlayerIsNull_ShouldThrowException()
    {
        // Arrange
        Player? player = null;

        // Act
        _playerService.UpdateEntity(player!);

        // Assert - exception expected
    }

    [TestMethod]
    public void DeletePlayer_WhenPlayerExists_ShouldDeletePlayer()
    {
        // Arrange
        var player = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        _repository.Add(player);

        // Act
        _playerService.DeleteEntity(1);

        // Assert
        var allPlayers = _repository.GetAll();
        Assert.AreEqual(0, allPlayers.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(PlayerNotFoundException))]
    public void DeletePlayer_WhenPlayerNotFound_ShouldThrowException()
    {
        // Arrange - no players added

        // Act
        _playerService.DeleteEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void SearchPlayers_ByFirstName_ShouldReturnMatchingPlayers()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        var player2 = new Player { Id = 2, FirstName = "Jane", LastName = "Smith" };
        var player3 = new Player { Id = 3, FirstName = "Johnny", LastName = "Depp" };
        _repository.Add(player1);
        _repository.Add(player2);
        _repository.Add(player3);

        // Act
        var result = _playerService.SearchPlayers("John");

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(p => p.FirstName.Contains("John", StringComparison.OrdinalIgnoreCase) ||
                                      p.LastName.Contains("John", StringComparison.OrdinalIgnoreCase)));
    }

    [TestMethod]
    public void SearchPlayers_ByLastName_ShouldReturnMatchingPlayers()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        var player2 = new Player { Id = 2, FirstName = "Jane", LastName = "Smith" };
        _repository.Add(player1);
        _repository.Add(player2);

        // Act
        var result = _playerService.SearchPlayers("Doe");

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Doe", result[0].LastName);
    }

    [TestMethod]
    public void SearchPlayers_WithEmptySearchTerm_ShouldReturnAllPlayers()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        var player2 = new Player { Id = 2, FirstName = "Jane", LastName = "Smith" };
        _repository.Add(player1);
        _repository.Add(player2);

        // Act
        var result = _playerService.SearchPlayers("");

        // Assert
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void SearchPlayers_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        _repository.Add(player1);

        // Act
        var result = _playerService.SearchPlayers("NonExistent");

        // Assert
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SearchPlayers_CaseInsensitive_ShouldReturnMatchingPlayers()
    {
        // Arrange
        var player1 = new Player { Id = 1, FirstName = "John", LastName = "Doe" };
        _repository.Add(player1);

        // Act
        var result = _playerService.SearchPlayers("JOHN");

        // Assert
        Assert.AreEqual(1, result.Count);
    }
}