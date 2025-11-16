using CourseProject.BLL.Entities;

namespace CourseProject.BLL.Services;

public interface IGameService : IService<Game>
{
    List<Game> SearchGames(DateTime? date, string? opponentTeam);

    List<Game> SortGamesByDate();

    List<Game> SortGamesByResult();
}