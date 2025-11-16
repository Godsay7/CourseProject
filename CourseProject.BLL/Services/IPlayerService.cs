using CourseProject.BLL.Entities;

namespace CourseProject.BLL.Services;

public interface IPlayerService : IService<Player>
{
    List<Player> SearchPlayers(string searchTerm);
}