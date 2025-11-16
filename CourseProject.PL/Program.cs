using CourseProject.BLL.Entities;
using CourseProject.BLL.Services;
using CourseProject.DAL.Repositories;

namespace CourseProject.PL;

class Program
{
    static void Main(string[] args)
    {
        // Шлях до User/AppData + Нова папка
        var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FootballMatchPlanning");
        Directory.CreateDirectory(dataDirectory);

        IPlayerService playerService = new PlayerService(new FileRepository<Player>(Path.Combine(dataDirectory, "players.json")));
        IGameService gameService = new GameService(new FileRepository<Game>(Path.Combine(dataDirectory, "games.json")));
        IStadiumService stadiumService = new StadiumService(new FileRepository<Stadium>(Path.Combine(dataDirectory, "stadiums.json")));

        var consoleUI = new ConsoleUI(playerService, gameService, stadiumService);
        consoleUI.Run();
    }
}

