using CourseProject.BLL.Entities;
using CourseProject.BLL.Services;

namespace CourseProject.PL;

public class ConsoleUI
{
    private readonly IPlayerService _playerService;
    private readonly IGameService _gameService;
    private readonly IStadiumService _stadiumService;

    public ConsoleUI(IPlayerService playerService, IGameService gameService, IStadiumService stadiumService)
    {
        _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _stadiumService = stadiumService ?? throw new ArgumentNullException(nameof(stadiumService));
    }
    public void Run()
    {
        Console.WriteLine("=== Football Match Planning System ===");
        Console.WriteLine();

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        PlayerManagementMenu();
                        break;
                    case "2":
                        GameManagementMenu();
                        break;
                    case "3":
                        StadiumManagementMenu();
                        break;
                    case "4":
                        SearchMenu();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Player Management");
        Console.WriteLine("2. Game Management");
        Console.WriteLine("3. Stadium Management");
        Console.WriteLine("4. Search");
        Console.WriteLine("0. Exit");
        Console.Write("Select an option: ");
    }

    private void PlayerManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Player Management ===");
        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. Delete Player");
        Console.WriteLine("3. Update Player");
        Console.WriteLine("4. View Player");
        Console.WriteLine("5. View All Players");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                AddPlayer();
                break;
            case "2":
                DeletePlayer();
                break;
            case "3":
                UpdatePlayer();
                break;
            case "4":
                ViewPlayer();
                break;
            case "5":
                ViewAllPlayers();
                break;
        }
    }

    private void AddPlayer()
    {
        Console.WriteLine("\n=== Add Player ===");
        var player = new Player();

        Console.Write("First Name: ");
        player.FirstName = ValidateRequiredString(Console.ReadLine(), "First Name");

        Console.Write("Last Name: ");
        player.LastName = ValidateRequiredString(Console.ReadLine(), "Last Name");

        Console.Write("Date of Birth (yyyy-mm-dd): ");
        player.DateOfBirth = ValidateDate(Console.ReadLine());

        Console.Write("Status: ");
        player.Status = ValidateRequiredString(Console.ReadLine(), "Status");

        Console.Write("Health Status: ");
        player.HealthStatus = ValidateRequiredString(Console.ReadLine(), "Health Status");

        Console.Write("Salary: ");
        player.Salary = ValidateDecimal(Console.ReadLine());

        _playerService.AddEntity(player);
        Console.WriteLine("Player added successfully!");
    }

    private void DeletePlayer()
    {
        Console.WriteLine("\n=== Delete Player ===");
        Console.Write("Enter Player ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _playerService.DeleteEntity(id);
            Console.WriteLine("Player deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void UpdatePlayer()
    {
        Console.WriteLine("\n=== Update Player ===");
        Console.Write("Enter Player ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var player = _playerService.GetByIdEntity(id);
        Console.WriteLine($"\nCurrent Player Info:");
        DisplayPlayer(player);

        Console.WriteLine("\nEnter new values (press Enter to keep current value):");

        Console.Write($"First Name ({player.FirstName}): ");
        var firstName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(firstName))
            player.FirstName = firstName;

        Console.Write($"Last Name ({player.LastName}): ");
        var lastName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(lastName))
            player.LastName = lastName;

        Console.Write($"Date of Birth ({player.DateOfBirth:yyyy-MM-dd}): ");
        var dob = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(dob))
            player.DateOfBirth = ValidateDate(dob);

        Console.Write($"Status ({player.Status}): ");
        var status = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(status))
            player.Status = status;

        Console.Write($"Health Status ({player.HealthStatus}): ");
        var health = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(health))
            player.HealthStatus = health;

        Console.Write($"Salary ({player.Salary}): ");
        var salary = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(salary))
            player.Salary = ValidateDecimal(salary);

        _playerService.UpdateEntity(player);
        Console.WriteLine("Player updated successfully!");
    }

    private void ViewPlayer()
    {
        Console.WriteLine("\n=== View Player ===");
        Console.Write("Enter Player ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var player = _playerService.GetByIdEntity(id);
            DisplayPlayer(player);
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void ViewAllPlayers()
    {
        Console.WriteLine("\n=== All Players ===");
        var players = _playerService.GetAllEntity();
        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
        }
        else
        {
            foreach (var player in players)
            {
                DisplayPlayer(player);
                Console.WriteLine();
            }
        }
    }

    private void DisplayPlayer(Player player)
    {
        Console.WriteLine($"ID: {player.Id}");
        Console.WriteLine($"Name: {player.FirstName} {player.LastName}");
        Console.WriteLine($"Date of Birth: {player.DateOfBirth:yyyy-mm-dd}");
        Console.WriteLine($"Status: {player.Status}");
        Console.WriteLine($"Health Status: {player.HealthStatus}");
        Console.WriteLine($"Salary: {player.Salary:C}");
    }

    private void GameManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Game Management ===");
        Console.WriteLine("1. Add Game");
        Console.WriteLine("2. Delete Game");
        Console.WriteLine("3. Update Game");
        Console.WriteLine("4. View Game");
        Console.WriteLine("5. View All Games");
        Console.WriteLine("6. Sort Games by Date");
        Console.WriteLine("7. Sort Games by Result");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                AddGame();
                break;
            case "2":
                DeleteGame();
                break;
            case "3":
                UpdateGame();
                break;
            case "4":
                ViewGame();
                break;
            case "5":
                ViewAllGames();
                break;
            case "6":
                ViewGamesSortedByDate();
                break;
            case "7":
                ViewGamesSortedByResult();
                break;
        }
    }

    private void AddGame()
    {
        Console.WriteLine("\n=== Add Game ===");
        var game = new Game();

        Console.Write("Date (yyyy-mm-dd): ");
        game.Date = ValidateDate(Console.ReadLine());

        Console.Write("Venue: ");
        game.Venue = ValidateRequiredString(Console.ReadLine(), "Venue");

        Console.Write("Opponent Team: ");
        game.OpponentTeam = ValidateRequiredString(Console.ReadLine(), "Opponent Team");

        Console.Write("Number of Spectators: ");
        game.Spectators = ValidateInteger(Console.ReadLine());

        Console.Write("Result (Won/Lost/Tie/Not yet played): ");
        game.Result = ValidateRequiredString(Console.ReadLine(), "Result");

        Console.WriteLine("Add players to game (enter player IDs, press Enter with empty line to finish):");
        game.PlayerIds = new List<int>();
        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                break;
            if (int.TryParse(input, out int playerId))
            {
                game.PlayerIds.Add(playerId);
            }
        }

        _gameService.AddEntity(game);
        Console.WriteLine("Game added successfully!");
    }

    private void DeleteGame()
    {
        Console.WriteLine("\n=== Delete Game ===");
        Console.Write("Enter Game ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _gameService.DeleteEntity(id);
            Console.WriteLine("Game deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void UpdateGame()
    {
        Console.WriteLine("\n=== Update Game ===");
        Console.Write("Enter Game ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var game = _gameService.GetByIdEntity(id);
        Console.WriteLine($"\nCurrent Game Info:");
        DisplayGame(game);

        Console.WriteLine("\nEnter new values (press Enter to keep current value):");

        Console.Write($"Date ({game.Date:yyyy-MM-dd}): ");
        var date = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(date))
            game.Date = ValidateDate(date);

        Console.Write($"Venue ({game.Venue}): ");
        var venue = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(venue))
            game.Venue = venue;

        Console.Write($"Opponent Team ({game.OpponentTeam}): ");
        var opponent = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(opponent))
            game.OpponentTeam = opponent;

        Console.Write($"Spectators ({game.Spectators}): ");
        var spectators = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(spectators))
            game.Spectators = ValidateInteger(spectators);

        Console.Write($"Result ({game.Result}): ");
        var result = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(result))
            game.Result = result;

        Console.WriteLine("Update players (enter 'a' to add, 'r' to remove, 'Enter' to skip):");
        var action = Console.ReadLine();
        if (action?.ToLower() == "a")
        {
            Console.Write("Enter Player ID to add: ");
            if (int.TryParse(Console.ReadLine(), out int playerId))
                game.PlayerIds.Add(playerId);
        }
        else if (action?.ToLower() == "r")
        {
            Console.Write("Enter Player ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int playerId))
                game.PlayerIds.Remove(playerId);
        }

        _gameService.UpdateEntity(game);
        Console.WriteLine("Game updated successfully!");
    }

    private void ViewGame()
    {
        Console.WriteLine("\n=== View Game ===");
        Console.Write("Enter Game ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var game = _gameService.GetByIdEntity(id);
            DisplayGame(game);
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void ViewAllGames()
    {
        Console.WriteLine("\n=== All Games ===");
        var games = _gameService.GetAllEntity();
        if (games.Count == 0)
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            foreach (var game in games)
            {
                DisplayGame(game);
                Console.WriteLine();
            }
        }
    }

    private void ViewGamesSortedByDate()
    {
        Console.WriteLine("\n=== Games Sorted by Date ===");
        var games = _gameService.SortGamesByDate();
        foreach (var game in games)
        {
            DisplayGame(game);
            Console.WriteLine();
        }
    }

    private void ViewGamesSortedByResult()
    {
        Console.WriteLine("\n=== Games Sorted by Result ===");
        var games = _gameService.SortGamesByResult();
        foreach (var game in games)
        {
            DisplayGame(game);
            Console.WriteLine();
        }
    }

    private void DisplayGame(Game game)
    {
        Console.WriteLine($"ID: {game.Id}");
        Console.WriteLine($"Date: {game.Date:yyyy-mm-dd}");
        Console.WriteLine($"Venue: {game.Venue}");
        Console.WriteLine($"Opponent Team: {game.OpponentTeam}");
        Console.WriteLine($"Spectators: {game.Spectators}");
        Console.WriteLine($"Result: {game.Result}");
        Console.WriteLine($"Players: {string.Join(", ", game.PlayerIds)}");
    }

    private void StadiumManagementMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Stadium Management ===");
        Console.WriteLine("1. Add Stadium");
        Console.WriteLine("2. Delete Stadium");
        Console.WriteLine("3. Update Stadium");
        Console.WriteLine("4. View Stadium");
        Console.WriteLine("5. View All Stadiums");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                AddStadium();
                break;
            case "2":
                DeleteStadium();
                break;
            case "3":
                UpdateStadium();
                break;
            case "4":
                ViewStadium();
                break;
            case "5":
                ViewAllStadiums();
                break;
        }
    }

    private void AddStadium()
    {
        Console.WriteLine("\n=== Add Stadium ===");
        var stadium = new Stadium();

        Console.Write("Name: ");
        stadium.Name = ValidateRequiredString(Console.ReadLine(), "Name");

        Console.Write("Number of Seats: ");
        stadium.NumberOfSeats = ValidateInteger(Console.ReadLine());

        Console.Write("Price per Seat: ");
        stadium.PricePerSeat = ValidateDecimal(Console.ReadLine());

        _stadiumService.AddEntity(stadium);
        Console.WriteLine("Stadium added successfully!");
    }

    private void DeleteStadium()
    {
        Console.WriteLine("\n=== Delete Stadium ===");
        Console.Write("Enter Stadium ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _stadiumService.DeleteEntity(id);
            Console.WriteLine("Stadium deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void UpdateStadium()
    {
        Console.WriteLine("\n=== Update Stadium ===");
        Console.Write("Enter Stadium ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var stadium = _stadiumService.GetByIdEntity(id);
        Console.WriteLine($"\nCurrent Stadium Info:");
        DisplayStadium(stadium);

        Console.WriteLine("\nEnter new values (press Enter to keep current value):");

        Console.Write($"Number of Seats ({stadium.NumberOfSeats}): ");
        var seats = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(seats))
            stadium.NumberOfSeats = ValidateInteger(seats);

        Console.Write($"Price per Seat ({stadium.PricePerSeat}): ");
        var price = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(price))
            stadium.PricePerSeat = ValidateDecimal(price);

        _stadiumService.UpdateEntity(stadium);
        Console.WriteLine("Stadium updated successfully!");
    }

    private void ViewStadium()
    {
        Console.WriteLine("\n=== View Stadium ===");
        Console.Write("Enter Stadium ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var stadium = _stadiumService.GetByIdEntity(id);
            DisplayStadium(stadium);

            // Show games at this stadium
            var allGames = _gameService.GetAllEntity();
            var stadiumGames = allGames.Where(g => g.Venue == stadium.Name).ToList();
            if (stadiumGames.Any())
            {
                Console.WriteLine("\nGames at this stadium:");
                foreach (var game in stadiumGames)
                {
                    Console.WriteLine($"  - {game.Date:yyyy-mm-dd}: vs {game.OpponentTeam}");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private void ViewAllStadiums()
    {
        Console.WriteLine("\n=== All Stadiums ===");
        var stadiums = _stadiumService.GetAllEntity();
        if (stadiums.Count == 0)
        {
            Console.WriteLine("No stadiums found.");
        }
        else
        {
            foreach (var stadium in stadiums)
            {
                DisplayStadium(stadium);
                Console.WriteLine();
            }
        }
    }

    private void DisplayStadium(Stadium stadium)
    {
        Console.WriteLine($"ID: {stadium.Id}");
        Console.WriteLine($"Name: {stadium.Name}");
        Console.WriteLine($"Number of Seats: {stadium.NumberOfSeats}");
        Console.WriteLine($"Price per Seat: {stadium.PricePerSeat:C}");
    }

    private void SearchMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Search ===");
        Console.WriteLine("1. Search Player by Name");
        Console.WriteLine("2. Search Game by Date and Opponent Team");
        Console.WriteLine("3. Search Stadium by Name");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                SearchPlayers();
                break;
            case "2":
                SearchGames();
                break;
            case "3":
                SearchStadiums();
                break;
        }
    }

    private void SearchPlayers()
    {
        Console.WriteLine("\n=== Search Players ===");
        Console.Write("Enter search term (first or last name): ");
        var searchTerm = Console.ReadLine();
        var players = _playerService.SearchPlayers(searchTerm ?? "");
        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
        }
        else
        {
            foreach (var player in players)
            {
                DisplayPlayer(player);
                Console.WriteLine();
            }
        }
    }

    private void SearchGames()
    {
        Console.WriteLine("\n=== Search Games ===");
        Console.Write("Enter date (yyyy-mm-dd) or press Enter to skip: ");
        DateTime? date = null;
        var dateInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (DateTime.TryParse(dateInput, out DateTime parsedDate))
                date = parsedDate;
        }

        Console.Write("Enter opponent team name or press Enter to skip: ");
        var opponentTeam = Console.ReadLine();

        var games = _gameService.SearchGames(date, opponentTeam);
        if (games.Count == 0)
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            foreach (var game in games)
            {
                DisplayGame(game);
                Console.WriteLine();
            }
        }
    }

    private void SearchStadiums()
    {
        Console.WriteLine("\n=== Search Stadiums ===");
        Console.Write("Enter stadium name: ");
        var name = Console.ReadLine();
        var stadiums = _stadiumService.SearchStadiums(name ?? "");
        if (stadiums.Count == 0)
        {
            Console.WriteLine("No stadiums found.");
        }
        else
        {
            foreach (var stadium in stadiums)
            {
                DisplayStadium(stadium);
                Console.WriteLine();
            }
        }
    }

    // Validation methods
    private string ValidateRequiredString(string? input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{fieldName} is required.");
        return input.Trim();
    }

    private DateTime ValidateDate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Date is required.");
        if (!DateTime.TryParse(input, out DateTime date))
            throw new ArgumentException("Invalid date format. Use yyyy-mm-dd.");
        return date;
    }

    private int ValidateInteger(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Value is required.");
        if (!int.TryParse(input, out int value) || value < 0)
            throw new ArgumentException("Invalid integer value. Must be a non-negative number.");
        return value;
    }

    private decimal ValidateDecimal(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Value is required.");
        if (!decimal.TryParse(input, out decimal value) || value < 0)
            throw new ArgumentException("Invalid decimal value. Must be a non-negative number.");
        return value;
    }
}