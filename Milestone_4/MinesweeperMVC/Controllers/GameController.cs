using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using MinesweeperMVC.Models;
using Newtonsoft.Json;

namespace MinesweeperMVC.Controllers
{
    public class GameController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<GameController> _logger;
        private readonly MinesweeperDbContext _context;

        public GameController(MinesweeperDbContext context, ICompositeViewEngine viewEngine, ILogger<GameController> logger)
        {
            _context = context;
            _viewEngine = viewEngine;
            _logger = logger;
        }

        // Handles the GET request to start or restart a game
        [HttpGet]
        public IActionResult StartGame(bool isRestart = false)
        {
            // Get the current user's username from the authentication system
            var currentUsername = User.Identity?.Name;

            // Redirect to login if the user is not authenticated
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            // If this is a restart, retrieve the last-used settings from the session and initialize the game
            if (isRestart)
            {
                var boardSize = HttpContext.Session.GetString($"LastBoardSize_{currentUsername}") ?? "small";
                var difficulty = HttpContext.Session.GetString($"LastDifficulty_{currentUsername}") ?? "easy";
                return StartGame(boardSize, difficulty);
            }

            // Clear any existing game ID to ensure a fresh game starts
            HttpContext.Session.Remove("CurrentGameId");

            // Render the Start Game view for a new game setup
            return View();
        }

        // Handles the POST request to initialize a new game
        [HttpPost]
        public IActionResult StartGame(string boardSize = "small", string difficulty = "easy")
        {
            // Get the current user's username
            var currentUsername = User.Identity.Name;

            // Determine the board size and difficulty level
            int size = boardSize switch { "small" => 9, "medium" => 16, "large" => 24, _ => 9 };
            double difficultyLevel = difficulty switch { "easy" => 0.10, "medium" => 0.15, "hard" => 0.20, _ => 0.15 };

            // Initialize the game board with the chosen size and difficulty
            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();

            // Save the board state and metadata to the session
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", JsonConvert.SerializeObject(board));
            HttpContext.Session.SetString($"LastBoardSize_{currentUsername}", boardSize);
            HttpContext.Session.SetString($"LastDifficulty_{currentUsername}", difficulty);

            // Save the game start time for scoring purposes
            HttpContext.Session.SetString($"StartTime_{currentUsername}", DateTime.UtcNow.ToString());

            // Mark the game as not over
            HttpContext.Session.SetString($"GameOver_{currentUsername}", "false");

            // Redirect to the Minesweeper game board
            return RedirectToAction("MineSweeperBoard");
        }

        private Board GetBoardFromSession()
        {
            var currentUsername = User.Identity?.Name;

            if (string.IsNullOrEmpty(currentUsername))
            {
                _logger.LogWarning("User not authenticated. Cannot retrieve board from session.");
                return null;
            }

            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");

            if (string.IsNullOrEmpty(boardJson))
            {
                _logger.LogWarning($"No board data found in session for user: {currentUsername}");
                return null;
            }

            try
            {
                _logger.LogInformation("Board data retrieved and deserialized successfully.");
                return JsonConvert.DeserializeObject<Board>(boardJson);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing board from session.");
                return null;
            }
        }

        // Updated MineSweeperBoard using the helper
        public IActionResult MineSweeperBoard()
        {
            var board = GetBoardFromSession();
            if (board == null)
            {
                return RedirectToAction("StartGame");
            }

            return View(board);
        }

        [HttpPost]
        public IActionResult RevealCell(int row, int col)
        {
            // Get the current user's username
            var currentUsername = User.Identity.Name;

            _logger.LogInformation($"RevealCell action invoked by user '{currentUsername}' for cell at row {row}, col {col}.");

            // Redirect to login if the user is not authenticated
            if (string.IsNullOrEmpty(currentUsername))
            {
                _logger.LogWarning("User not authenticated. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the serialized game board from the session
            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");
            if (string.IsNullOrEmpty(boardJson))
            {
                _logger.LogWarning("Board not found in session. Redirecting to StartGame.");
                return RedirectToAction("StartGame");
            }

            // Deserialize the board from the session
            Board board;
            try
            {
                board = JsonConvert.DeserializeObject<Board>(boardJson);
                _logger.LogInformation("Board deserialized successfully.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing board from session.");
                return RedirectToAction("StartGame");
            }

            // Ensure the board and its grid are valid
            if (board == null || board.Grid == null)
            {
                _logger.LogWarning("Invalid board or grid. Redirecting to StartGame.");
                return RedirectToAction("StartGame");
            }

            // Prevent further actions if the game is already over
            if (HttpContext.Session.GetString($"GameOver_{currentUsername}") == "true")
            {
                _logger.LogInformation("Game is already over. Returning MineSweeperBoard view.");
                return View("MineSweeperBoard", board);
            }

            // Ensure the cell indices are within valid bounds
            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size)
            {
                _logger.LogWarning("Invalid cell indices. Returning MineSweeperBoard view.");
                return View("MineSweeperBoard", board);
            }

            // Access the specified cell
            var cell = board.Grid[row][col];

            // Log the cell state before update
            _logger.LogInformation($"Cell state before update: Visited={cell.Visited}, Live={cell.Live}, LiveNeighbors={cell.LiveNeighbors}");

            // Introduce a flag to indicate if a flood-fill occurred
            bool floodFillOccurred = false;

            // Determine the action based on the cell's state
            if (cell.Flagged)
            {
                _logger.LogInformation($"Cell at Row {row}, Column {col} is flagged. No action taken.");
                ViewData["Timestamp"] = $"AJAX Update: Cell at Row {row}, Column {col} is flagged. Unflag it to reveal.";
                return PartialView("_GameContainer", board); // Return without modifying the game state
            }
            else if (cell.Visited)
            {
                _logger.LogInformation("Cell already visited. No action taken.");
                ViewData["Timestamp"] = $"AJAX Update: Cell at Row {row}, Column {col} was already revealed at {DateTime.UtcNow:HH:mm:ss}";
            }
            else if (cell.Live)
            {
                _logger.LogInformation("Mine cell clicked. Revealing all bombs and marking game as over.");
                board.RevealAllBombs(); // Reveal all mines on the board
                HttpContext.Session.SetString($"GameOver_{currentUsername}", "true"); // Mark the game as over
                ViewData["GameOver"] = true;
                ViewData["LossMessage"] = "Game Over! You clicked on a mine.";
                ViewData["Timestamp"] = $"AJAX Update: Game Over! Mine clicked at Row {row}, Column {col} at {DateTime.UtcNow:HH:mm:ss}";
            }
            else if (cell.LiveNeighbors > 0)
            {
                cell.Visited = true; // Reveal the numbered cell
                _logger.LogInformation("Cell with live neighbors revealed.");
                ViewData["Timestamp"] = $"AJAX Update: Cell at Row {row}, Column {col} revealed with {cell.LiveNeighbors} neighboring mines at {DateTime.UtcNow:HH:mm:ss}";
            }
            else
            {
                board.FloodFill(row, col); // Perform flood-fill for empty cells
                floodFillOccurred = true;
                _logger.LogInformation("Flood-fill performed from empty cell.");
                ViewData["Timestamp"] = $"AJAX Update: Flood-fill started from Row {row}, Column {col} at {DateTime.UtcNow:HH:mm:ss}";
            }

            // Check if the player has won
            bool gameIsOver = false;
            if (CheckWinCondition(board))
            {
                _logger.LogInformation("Win condition met. Calculating score.");
                HttpContext.Session.SetString($"GameOver_{currentUsername}", "true");

                // Calculate the elapsed time
                var startTimeStr = HttpContext.Session.GetString($"StartTime_{currentUsername}");
                DateTime startTime = DateTime.TryParse(startTimeStr, out startTime) ? startTime : DateTime.UtcNow;
                var elapsedTime = DateTime.UtcNow - startTime;

                // Retrieve the difficulty setting
                var difficultyStr = HttpContext.Session.GetString($"LastDifficulty_{currentUsername}") ?? "easy";

                // Calculate the score
                int baseScore = 1000;
                double sizeMultiplier = board.Size / 9.0;
                double difficultyMultiplier = difficultyStr switch
                {
                    "easy" => 1.0,
                    "medium" => 1.5,
                    "hard" => 2.0,
                    _ => 1.0
                };

                double timePenalty = elapsedTime.TotalSeconds;
                timePenalty = Math.Max(timePenalty, 1); // Avoid division by zero or small penalties

                int finalScore = (int)(baseScore * sizeMultiplier * difficultyMultiplier / timePenalty);
                finalScore = Math.Max(finalScore, 1); // Ensure the score is at least 1

                // Set ViewData for the win message
                ViewData["GameOver"] = true;
                ViewData["WinMessage"] = "Congratulations! You’ve won the game!";
                ViewData["Score"] = finalScore; // Pass the score
                ViewData["ElapsedTime"] = elapsedTime.ToString("mm\\:ss"); // Pass the formatted time
                ViewData["Timestamp"] = $"AJAX Update: Game Won! All safe cells revealed at {DateTime.UtcNow:HH:mm:ss}";

                // Return updated game container with the win message
                return PartialView("_GameContainer", board);
            }

            // Save the updated board state
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", JsonConvert.SerializeObject(board));

            // Handle AJAX and full page requests
            if (IsAjaxRequest())
            {
                if (gameIsOver || floodFillOccurred)
                {
                    _logger.LogInformation("Game over or flood-fill detected. Returning full game container view.");
                    return PartialView("_GameContainer", board);
                }
                else
                {
                    _logger.LogInformation("AJAX request detected. Returning full game container view.");
                    return PartialView("_GameContainer", board);
                }
            }
            else
            {
                _logger.LogInformation("Non-AJAX request detected. Returning MineSweeperBoard view.");
                return View("MineSweeperBoard", board);
            }
        }

        // Helper method to check if the request is an AJAX call
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"].ToString().Contains("XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        }

        [HttpPost]
        public IActionResult ToggleFlag(int row, int col)
        {
            // Get the current user's username
            var currentUsername = User.Identity.Name;

            _logger.LogInformation($"ToggleFlag action invoked by user '{currentUsername}' for cell at row {row}, col {col}.");

            // Redirect to login if the user is not authenticated
            if (string.IsNullOrEmpty(currentUsername))
            {
                _logger.LogWarning("User not authenticated. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the board from the session
            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");
            if (string.IsNullOrEmpty(boardJson))
            {
                _logger.LogWarning("Board not found in session. Redirecting to StartGame.");
                return RedirectToAction("StartGame");
            }

            var board = JsonConvert.DeserializeObject<Board>(boardJson);

            // Toggle the flag state for the specified cell
            board.ToggleFlag(row, col);

            // Save the updated board to the session
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", JsonConvert.SerializeObject(board));

            // Update the timestamp
            ViewData["Timestamp"] = $"AJAX Update: Flag toggled at Row {row}, Column {col} at {DateTime.UtcNow:HH:mm:ss}";

            // Return the updated game container
            return PartialView("_GameContainer", board);
        }

        // Checks if the player has met the win condition
        private bool CheckWinCondition(Board board)
        {
            foreach (var row in board.Grid)
            {
                foreach (var cell in row)
                {
                    // If any non-mine cell is unvisited, the player hasn't won
                    if (!cell.Live && !cell.Visited)
                        return false;
                }
            }
            // All non-mine cells are revealed
            return true;
        }

        // Restarts the game using the last-used settings
        [HttpPost]
        public IActionResult RestartGame()
        {
            // Get the current user's username
            var currentUsername = User.Identity?.Name;

            // Redirect to login if the user is not authenticated
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            // Clear any existing game ID to ensure a fresh game restarts
            HttpContext.Session.Remove("CurrentGameId");

            // Restart the game with the previous settings
            return RedirectToAction("StartGame", new { isRestart = true });
        }

        [HttpPost]
        public IActionResult SaveGame()
        {
            // Log that the SaveGame action has been invoked
            _logger.LogInformation("SaveGame action invoked.");

            // Retrieve the UserId from the session and check if it's valid
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // If UserId is missing, display an error and return the current game board
                ViewData["Error"] = "Unable to save the game. Please log in again.";
                return View("MineSweeperBoard", GetBoardFromSession());
            }

            // Attempt to parse the UserId string to an integer
            if (!int.TryParse(userIdString, out int userId))
            {
                // If parsing fails, display an error and return the current game board
                ViewData["Error"] = "Invalid User ID. Please log in again.";
                return View("MineSweeperBoard", GetBoardFromSession());
            }

            _logger.LogInformation($"UserId retrieved: {userId}");

            // Retrieve the current game state and check if it's valid
            var gameState = GetSerializedGameState();
            if (string.IsNullOrEmpty(gameState))
            {
                // If no game state is found, display an error and return the current board
                ViewData["Error"] = "No valid game found to save. Please start or continue a game first.";
                return View("MineSweeperBoard", GetBoardFromSession());
            }

            // Retrieve the current game ID from the session
            var currentGameIdString = HttpContext.Session.GetString("CurrentGameId");
            if (!int.TryParse(currentGameIdString, out int currentGameId))
            {
                // If no game ID is stored, assume it's a new game
                currentGameId = 0;
            }

            try
            {
                Game existingGame = null;

                // If a current game ID exists, attempt to retrieve the existing game from the database
                if (currentGameId > 0)
                {
                    existingGame = _context.Games.FirstOrDefault(g => g.Id == currentGameId && g.UserId == userId);
                }

                if (existingGame != null)
                {
                    // If the game already exists, check if there are any changes to update
                    if (existingGame.GameData != gameState)
                    {
                        _logger.LogInformation($"Updating existing game with ID: {existingGame.Id}");
                        existingGame.DateSaved = DateTime.Now;
                        existingGame.GameData = gameState;
                        _context.Games.Update(existingGame);
                        _context.SaveChanges();
                        ViewData["SuccessMessage"] = "Game updated successfully!";
                    }
                    else
                    {
                        // If no changes are detected, inform the user
                        _logger.LogInformation("No changes detected in the game data.");
                        ViewData["Error"] = "No changes were made to the game.";
                    }
                }
                else
                {
                    // If no existing game is found, create a new game record
                    _logger.LogInformation("Creating a new game record.");
                    var newGame = new Game
                    {
                        UserId = userId,
                        DateSaved = DateTime.Now,
                        GameData = gameState
                    };
                    _context.Games.Add(newGame);
                    _context.SaveChanges(); // Save to generate the new Game ID

                    // Save the new Game ID in the session
                    HttpContext.Session.SetString("CurrentGameId", newGame.Id.ToString());
                    ViewData["SuccessMessage"] = "New game saved successfully!";
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the save process
                _logger.LogError(ex, "Error saving game to database.");
                ViewData["Error"] = "An error occurred while saving the game.";
            }

            // Return the game board view with the latest session data
            return View("MineSweeperBoard", GetBoardFromSession());
        }

        private string GetSerializedGameState()
        {
            // Log that the method was invoked
            _logger.LogInformation("GetSerializedGameState method invoked.");

            // Retrieve the current user's username
            var currentUsername = User.Identity.Name;

            // If the user is not authenticated, return an empty JSON object
            if (string.IsNullOrEmpty(currentUsername))
            {
                _logger.LogWarning("User is not authenticated. Cannot serialize game state.");
                return "{}";
            }

            // Retrieve the board JSON stored in the session
            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");

            // If no board data is found in the session, return an empty JSON object
            if (string.IsNullOrEmpty(boardJson))
            {
                _logger.LogWarning($"No board data found in session for user: {currentUsername}");
                return "{}";
            }

            _logger.LogInformation($"Board data retrieved from session for user: {currentUsername}");

            return boardJson;
        }

        [Authorize]
        public IActionResult ShowSavedGames()
        {
            // Retrieve the UserId from the session
            var userIdString = HttpContext.Session.GetString("UserId");

            // If there's no UserId in session, redirect to the login page
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            // Attempt to parse the UserId string to an integer
            if (!int.TryParse(userIdString, out int userId))
            {
                // If parsing fails, redirect to the login page
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the saved games for the user
            var savedGames = _context.Games
                .Where(g => g.UserId == userId)
                .ToList();

            // Pass the saved games to the view
            return View(savedGames);
        }

        [HttpPost]
        public IActionResult LoadGame(int gameId)
        {
            // Retrieve the UserId from the session and validate it
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ViewData["Error"] = "Please log in to load a game.";
                return View("ShowSavedGames", _context.Games.Where(g => g.UserId == 0).ToList());
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                ViewData["Error"] = "Invalid User ID. Please log in again.";
                return View("ShowSavedGames", _context.Games.Where(g => g.UserId == userId).ToList());
            }

            // Retrieve the game by ID and ensure it belongs to the current user
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId && g.UserId == userId);
            if (game == null)
            {
                ViewData["Error"] = "Game not found or you do not have permission to access it.";
                return View("ShowSavedGames", _context.Games.Where(g => g.UserId == userId).ToList());
            }

            // Retrieve the current username and validate it
            var currentUsername = User.Identity.Name;
            if (string.IsNullOrEmpty(currentUsername))
            {
                ViewData["Error"] = "Please log in to load a game.";
                return View("ShowSavedGames", _context.Games.Where(g => g.UserId == userId).ToList());
            }

            // Save the game data in the session
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", game.GameData);
            HttpContext.Session.SetString("CurrentGameId", game.Id.ToString());

            ViewData["SuccessMessage"] = "Game loaded successfully!";
            return View("MineSweeperBoard", JsonConvert.DeserializeObject<Board>(game.GameData));
        }

        [HttpPost]
        public IActionResult DeleteGame(int gameId)
        {
            // Retrieve the UserId from the session and validate it
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ViewData["Error"] = "Please log in to delete a game.";
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                ViewData["Error"] = "Invalid User ID. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the game by ID and ensure it belongs to the current user
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId && g.UserId == userId);
            if (game == null)
            {
                ViewData["Error"] = "Game not found or you do not have permission to delete it.";
                return RedirectToAction("ShowSavedGames");
            }

            // Remove the game from the database
            _context.Games.Remove(game);
            _context.SaveChanges();

            ViewData["Message"] = "Game deleted successfully!";
            return RedirectToAction("ShowSavedGames");
        }
    }
}
