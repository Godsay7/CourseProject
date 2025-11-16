using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.DAL.Repositories;

namespace CourseProject.BLL.Services;

public class PlayerService : IPlayerService
{
    private readonly IRepository<Player> _repository;

    public PlayerService(IRepository<Player> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<Player> GetAllEntity()
    {
        try
        {
            return _repository.GetAll();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve players: {ex.Message}", ex);
        }
    }

    public Player GetByIdEntity(int id)
    {
        try
        {
            var player = _repository.GetById(id);
            if (player == null)
                throw new PlayerNotFoundException(id);
            return player;
        }
        catch (PlayerNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve player: {ex.Message}", ex);
        }
    }

    public void AddEntity(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        try
        {
            // Generate ID if not set
            if (player.Id == 0)
            {
                var allPlayers = _repository.GetAll();
                player.Id = allPlayers.Count > 0 ? allPlayers.Max(p => p.Id) + 1 : 1;
            }

            _repository.Add(player);
            _repository.Save();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to add player: {ex.Message}", ex);
        }
    }

    public void UpdateEntity(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        try
        {
            var existing = _repository.GetById(player.Id);
            if (existing == null)
                throw new PlayerNotFoundException(player.Id);

            _repository.Update(player);
            _repository.Save();
        }
        catch (PlayerNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to update player: {ex.Message}", ex);
        }
    }

    public void DeleteEntity(int id)
    {
        try
        {
            var player = _repository.GetById(id);
            if (player == null)
                throw new PlayerNotFoundException(id);

            _repository.Delete(id);
            _repository.Save();
        }
        catch (PlayerNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to delete player: {ex.Message}", ex);
        }
    }

    public List<Player> SearchPlayers(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return GetAllEntity();

        try
        {
            var allPlayers = GetAllEntity();
            var term = searchTerm.ToLowerInvariant();
            return allPlayers.Where(p =>
                p.FirstName.ToLowerInvariant().Contains(term) ||
                p.LastName.ToLowerInvariant().Contains(term)).ToList();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to search players: {ex.Message}", ex);
        }
    }
}