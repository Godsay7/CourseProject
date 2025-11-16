using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.BLL.Services;
using CourseProject.Tests.Repositories;

namespace CourseProject.Tests.Services;

[TestClass]
public class GameServiceTests
{
    private IGameService _gameService = null!;
    private MockRepository<Game> _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = new MockRepository<Game>();
        _gameService = new GameService(_repository);
    }

    [TestMethod]
    public void GetAllGames_ShouldReturnAllGames()
    {
        // Arrange
        var game1 = new Game { Id = 1, Date = DateTime.Now, OpponentTeam = "Team A" };
        var game2 = new Game { Id = 2, Date = DateTime.Now.AddDays(1), OpponentTeam = "Team B" };
        _repository.Add(game1);
        _repository.Add(game2);

        // Act
        var result = _gameService.GetAllEntity();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void GetGameById_WhenGameExists_ShouldReturnGame()
    {
        // Arrange
        var game = new Game { Id = 1, Date = DateTime.Now, OpponentTeam = "Team A" };
        _repository.Add(game);

        // Act
        var result = _gameService.GetByIdEntity(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Team A", result.OpponentTeam);
    }

    [TestMethod]
    [ExpectedException(typeof(GameNotFoundException))]
    public void GetGameById_WhenGameNotFound_ShouldThrowException()
    {
        // Arrange - no games added

        // Act
        _gameService.GetByIdEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void AddGame_ShouldAddGameWithGeneratedId()
    {
        // Arrange
        var game = new Game
        {
            Date = DateTime.Now,
            Venue = "Stadium A",
            OpponentTeam = "Team A",
            Spectators = 1000,
            Result = "Not yet played"
        };

        // Act
        _gameService.AddEntity(game);

        // Assert
        Assert.AreEqual(1, game.Id);
        var allGames = _repository.GetAll();
        Assert.AreEqual(1, allGames.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddGame_WhenGameIsNull_ShouldThrowException()
    {
        // Arrange
        Game? game = null;

        // Act
        _gameService.AddEntity(game!);

        // Assert - exception expected
    }

    [TestMethod]
    public void DeleteGame_WhenGameExists_ShouldDeleteGame()
    {
        // Arrange
        var game = new Game { Id = 1, Date = DateTime.Now, OpponentTeam = "Team A" };
        _repository.Add(game);

        // Act
        _gameService.DeleteEntity(1);

        // Assert
        var allGames = _repository.GetAll();
        Assert.AreEqual(0, allGames.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(GameNotFoundException))]
    public void DeleteGame_WhenGameNotFound_ShouldThrowException()
    {
        // Arrange - no games added

        // Act
        _gameService.DeleteEntity(999);

        // Assert - exception expected
    }

    [TestMethod]
    public void SortGamesByDate_ShouldReturnGamesSortedByDate()
    {
        // Arrange
        var game1 = new Game { Id = 1, Date = DateTime.Now.AddDays(2) };
        var game2 = new Game { Id = 2, Date = DateTime.Now };
        var game3 = new Game { Id = 3, Date = DateTime.Now.AddDays(1) };
        _repository.Add(game1);
        _repository.Add(game2);
        _repository.Add(game3);

        // Act
        var result = _gameService.SortGamesByDate();

        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(2, result[0].Id);
        Assert.AreEqual(3, result[1].Id);
        Assert.AreEqual(1, result[2].Id);
    }

    [TestMethod]
    public void SortGamesByResult_ShouldReturnGamesSortedByResult()
    {
        // Arrange
        var game1 = new Game { Id = 1, Result = "Lost" };
        var game2 = new Game { Id = 2, Result = "Won" };
        var game3 = new Game { Id = 3, Result = "Not yet played" };
        var game4 = new Game { Id = 4, Result = "Tie" };
        _repository.Add(game1);
        _repository.Add(game2);
        _repository.Add(game3);
        _repository.Add(game4);

        // Act
        var result = _gameService.SortGamesByResult();

        // Assert
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("Won", result[0].Result);
        Assert.AreEqual("Lost", result[1].Result);
        Assert.AreEqual("Tie", result[2].Result);
        Assert.AreEqual("Not yet played", result[3].Result);
    }
}