using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using Newtonsoft.Json;

namespace MinesweeperMVC.Controllers
{
    public class GameController : Controller
    {
        // Handles the GET request to start or restart a game
        [HttpGet]
        public IActionResult StartGame(bool isRestart = false)
        {
            // Redirect to login if the user is not logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // If restarting, retrieve last used settings and start the game
            if (isRestart)
            {
                var boardSize = HttpContext.Session.GetString("LastBoardSize") ?? "small";
                var difficulty = HttpContext.Session.GetString("LastDifficulty") ?? "easy";
                return StartGame(boardSize, difficulty); // Call the POST method directly
            }

            // Show the game settings view for a new game
            return View();
        }

        // Handles the POST request to initialize a new game
        [HttpPost]
        public IActionResult StartGame(string boardSize = "small", string difficulty = "easy")
        {
            // Get the current user's username
            var currentUsername = HttpContext.Session.GetString("Username");

            // Redirect to login if the user is not logged in
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            // Store the username as the game initiator
            HttpContext.Session.SetString("GameInitiator", currentUsername);

            // Determine the board size and difficulty level
            int size = boardSize switch { "small" => 9, "medium" => 16, "large" => 24, _ => 9 };
            double difficultyLevel = difficulty switch { "easy" => 0.10, "medium" => 0.15, "hard" => 0.20, _ => 0.15 };

            // Save board size and difficulty in the session for future use
            HttpContext.Session.SetString("LastBoardSize", boardSize);
            HttpContext.Session.SetString("LastDifficulty", difficulty);

            // Initialize the game board
            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors(); // Place mines randomly
            board.CalculateLiveNeighbors(); // Calculate adjacent mine counts

            // Store the board state and game metadata in the session
            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            HttpContext.Session.SetString("GameOver", "false");
            HttpContext.Session.SetString("StartTime", DateTime.UtcNow.ToString());

            // Redirect to the game board view
            return RedirectToAction("MineSweeperBoard");
        }

        // Displays the Minesweeper game board
        public IActionResult MineSweeperBoard()
        {
            // Get the current username and game initiator from the session
            var currentUsername = HttpContext.Session.GetString("Username");
            var gameInitiator = HttpContext.Session.GetString("GameInitiator");

            // Ensure only the game initiator can access the board
            if (string.IsNullOrEmpty(currentUsername) || currentUsername != gameInitiator)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            // Load the board state from the session
            var boardJson = HttpContext.Session.GetString("CurrentBoard");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            // Deserialize the board and return the view
            var board = JsonConvert.DeserializeObject<Board>(boardJson);
            return View(board);
        }

        // Handles revealing a cell on the board
        [HttpPost]
        public IActionResult RevealCell(int row, int col)
        {
            // Load the current board from the session
            var boardJson = HttpContext.Session.GetString("CurrentBoard");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            // Deserialize the board from JSON
            Board board;
            try
            {
                board = JsonConvert.DeserializeObject<Board>(boardJson);
            }
            catch (JsonException)
            {
                return RedirectToAction("StartGame");
            }

            // Check if the board or its grid is null
            if (board == null || board.Grid == null)
            {
                return RedirectToAction("StartGame");
            }

            // Prevent further actions if the game is over
            if (HttpContext.Session.GetString("GameOver") == "true")
            {
                return View("MineSweeperBoard", board);
            }

            // Ensure the cell indices are within bounds
            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size)
            {
                return View("MineSweeperBoard", board);
            }

            var cell = board.Grid[row][col];

            // If the cell is already visited, do nothing
            if (cell.Visited)
            {
                return View("MineSweeperBoard", board);
            }

            // Handle the scenario where the cell contains a mine
            if (cell.Live)
            {
                board.RevealAllBombs(); // Reveal all mines
                HttpContext.Session.SetString("GameOver", "true"); // Mark the game as over
                ViewData["GameOver"] = true;
                ViewData["LossMessage"] = "Game Over! You clicked on a mine.";
                return View("MineSweeperBoard", board);
            }
            else if (cell.LiveNeighbors > 0)
            {
                // Mark the cell as visited if it has live neighbors
                cell.Visited = true;
            }
            else
            {
                // Perform a flood-fill to reveal connected empty cells
                board.FloodFill(row, col);
            }

            // Check if the player has won
            if (CheckWinCondition(board))
            {
                HttpContext.Session.SetString("GameOver", "true");

                // Calculate the elapsed time
                var startTimeStr = HttpContext.Session.GetString("StartTime");
                DateTime startTime = DateTime.TryParse(startTimeStr, out startTime) ? startTime : DateTime.UtcNow;
                var elapsedTime = DateTime.UtcNow - startTime;

                // Get the difficulty from the session
                var difficultyStr = HttpContext.Session.GetString("LastDifficulty") ?? "easy";

                // Calculate the player's score
                int baseScore = 1000;
                double sizeMultiplier = board.Size / 9.0; // Small board is the baseline
                double difficultyMultiplier = difficultyStr switch
                {
                    "easy" => 1.0,
                    "medium" => 1.5,
                    "hard" => 2.0,
                    _ => 1.0
                };
                double timePenalty = elapsedTime.TotalSeconds;
                int finalScore = (int)(baseScore * sizeMultiplier * difficultyMultiplier / timePenalty);

                // Display the win message and score
                ViewData["GameOver"] = true;
                ViewData["Score"] = finalScore;
                ViewData["ElapsedTime"] = elapsedTime.ToString("mm\\:ss");

                return View("MineSweeperBoard", board);
            }

            // Save the updated board state to the session
            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            return View("MineSweeperBoard", board);
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

        // Restarts the game using the last used settings
        [HttpPost]
        public IActionResult RestartGame()
        {
            return RedirectToAction("StartGame", new { isRestart = true });
        }
    }
}
