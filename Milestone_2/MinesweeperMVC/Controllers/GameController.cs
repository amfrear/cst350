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
            var currentUsername = User.Identity.Name;
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            if (isRestart)
            {
                var boardSize = HttpContext.Session.GetString($"LastBoardSize_{currentUsername}") ?? "small";
                var difficulty = HttpContext.Session.GetString($"LastDifficulty_{currentUsername}") ?? "easy";
                return StartGame(boardSize, difficulty);
            }

            return View();
        }

        // Handles the POST request to initialize a new game
        [HttpPost]
        public IActionResult StartGame(string boardSize = "small", string difficulty = "easy")
        {
            var currentUsername = User.Identity.Name;

            // Initialize the game board
            int size = boardSize switch { "small" => 9, "medium" => 16, "large" => 24, _ => 9 };
            double difficultyLevel = difficulty switch { "easy" => 0.10, "medium" => 0.15, "hard" => 0.20, _ => 0.15 };

            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();

            // Save board state and metadata to the session
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", JsonConvert.SerializeObject(board));
            HttpContext.Session.SetString($"LastBoardSize_{currentUsername}", boardSize);
            HttpContext.Session.SetString($"LastDifficulty_{currentUsername}", difficulty);

            // Initialize the start time
            HttpContext.Session.SetString($"StartTime_{currentUsername}", DateTime.UtcNow.ToString());

            HttpContext.Session.SetString($"GameOver_{currentUsername}", "false");

            return RedirectToAction("MineSweeperBoard");
        }

        // Displays the Minesweeper game board
        public IActionResult MineSweeperBoard()
        {
            var currentUsername = User.Identity.Name;
            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            var board = JsonConvert.DeserializeObject<Board>(boardJson);
            return View(board);
        }

        // Handles revealing a cell on the board
        [HttpPost]
        public IActionResult RevealCell(int row, int col)
        {
            // Get the current user's username from authentication
            var currentUsername = User.Identity.Name;

            // Redirect to login if the user is not logged in
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the board state for the current user
            var boardJson = HttpContext.Session.GetString($"CurrentBoard_{currentUsername}");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            // Deserialize the board from the session
            Board board;
            try
            {
                board = JsonConvert.DeserializeObject<Board>(boardJson);
            }
            catch (JsonException)
            {
                return RedirectToAction("StartGame");
            }

            // Ensure the board and its grid are valid
            if (board == null || board.Grid == null)
            {
                return RedirectToAction("StartGame");
            }

            // Prevent further actions if the game is already over
            if (HttpContext.Session.GetString($"GameOver_{currentUsername}") == "true")
            {
                return View("MineSweeperBoard", board);
            }

            // Ensure the cell indices are within valid bounds
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

            // Handle the case where the cell contains a mine
            if (cell.Live)
            {
                board.RevealAllBombs(); // Reveal all mines
                HttpContext.Session.SetString($"GameOver_{currentUsername}", "true"); // Mark the game as over
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
                HttpContext.Session.SetString($"GameOver_{currentUsername}", "true");

                // Calculate the elapsed time
                var startTimeStr = HttpContext.Session.GetString($"StartTime_{currentUsername}");
                DateTime startTime = DateTime.TryParse(startTimeStr, out startTime) ? startTime : DateTime.UtcNow;
                var elapsedTime = DateTime.UtcNow - startTime;

                // Get the difficulty from the session
                var difficultyStr = HttpContext.Session.GetString($"LastDifficulty_{currentUsername}") ?? "easy";

                // Calculate the player's score
                // Base score
                int baseScore = 1000;

                // Size multiplier based on the board size
                double sizeMultiplier = board.Size / 9.0; // Small board is the baseline

                // Difficulty multiplier
                double difficultyMultiplier = difficultyStr switch
                {
                    "easy" => 1.0,
                    "medium" => 1.5,
                    "hard" => 2.0,
                    _ => 1.0
                };

                // Elapsed time penalty
                double timePenalty = elapsedTime.TotalSeconds;
                if (timePenalty < 1)
                {
                    // Avoid division by zero or extremely small penalty
                    timePenalty = 1;
                }

                // Final score calculation
                int finalScore = (int)(baseScore * sizeMultiplier * difficultyMultiplier / timePenalty);

                // Ensure the score is at least 1
                finalScore = Math.Max(finalScore, 1);

                // Display the win message and score
                ViewData["GameOver"] = true;
                ViewData["Score"] = finalScore;
                ViewData["ElapsedTime"] = elapsedTime.ToString("mm\\:ss");

                return View("MineSweeperBoard", board);
            }

            // Save the updated board state to the session
            HttpContext.Session.SetString($"CurrentBoard_{currentUsername}", JsonConvert.SerializeObject(board));
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
            var currentUsername = User.Identity.Name;

            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("StartGame", new { isRestart = true });

        }
    }
}
