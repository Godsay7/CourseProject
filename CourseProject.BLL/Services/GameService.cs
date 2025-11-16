using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.DAL.Repositories;

namespace CourseProject.BLL.Services;

public class GameService : IGameService
{
    private readonly IRepository<Game> _repository;

    public GameService(IRepository<Game> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<Game> GetAllEntity()
    {
        try
        {
            return _repository.GetAll();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve games: {ex.Message}", ex);
        }
    }

    public Game GetByIdEntity(int id)
    {
        try
        {
            var game = _repository.GetById(id);
            if (game == null)
                throw new GameNotFoundException(id);
            return game;
        }
        catch (GameNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve game: {ex.Message}", ex);
        }
    }

    public void AddEntity(Game game)
    {
        if (game == null)
            throw new ArgumentNullException(nameof(game));

        try
        {
            // Generate ID if not set
            if (game.Id == 0)
            {
                var allGames = _repository.GetAll();
                game.Id = allGames.Count > 0 ? allGames.Max(g => g.Id) + 1 : 1;
            }

            _repository.Add(game);
            _repository.Save();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to add game: {ex.Message}", ex);
        }
    }

    public void UpdateEntity(Game game)
    {
        if (game == null)
            throw new ArgumentNullException(nameof(game));

        try
        {
            var existing = _repository.GetById(game.Id);
            if (existing == null)
                throw new GameNotFoundException(game.Id);

            _repository.Update(game);
            _repository.Save();
        }
        catch (GameNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to update game: {ex.Message}", ex);
        }
    }

    public void DeleteEntity(int id)
    {
        try
        {
            var game = _repository.GetById(id);
            if (game == null)
                throw new GameNotFoundException(id);

            _repository.Delete(id);
            _repository.Save();
        }
        catch (GameNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to delete game: {ex.Message}", ex);
        }
    }

    public List<Game> SearchGames(DateTime? date, string? opponentTeam)
    {
        try
        {
            var allGames = GetAllEntity();
            var query = allGames.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(g => g.Date.Date == date.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(opponentTeam))
            {
                var team = opponentTeam.ToLowerInvariant();
                query = query.Where(g => g.OpponentTeam.ToLowerInvariant().Contains(team));
            }

            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to search games: {ex.Message}", ex);
        }
    }

    public List<Game> SortGamesByDate()
    {
        try
        {
            return GetAllEntity().OrderBy(g => g.Date).ToList();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to sort games by date: {ex.Message}", ex);
        }
    }

    public List<Game> SortGamesByResult()
    {
        try
        {
            var games = GetAllEntity();
            var resultOrder = new Dictionary<string, int>
            {
                { "Won", 1 },
                { "Lost", 2 },
                { "Tie", 3 },
                { "Not yet played", 4 }
            };

            return games.OrderBy(g => 
                resultOrder.TryGetValue(g.Result, out var order) ? order : 5).ToList();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to sort games by result: {ex.Message}", ex);
        }
    }
}