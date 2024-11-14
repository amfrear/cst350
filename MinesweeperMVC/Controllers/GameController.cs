using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using Newtonsoft.Json;

namespace MinesweeperMVC.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult StartGame(bool isRestart = false)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // If this is a restart, retrieve settings from the session
            if (isRestart)
            {
                var boardSize = HttpContext.Session.GetString("LastBoardSize") ?? "small";
                var difficulty = HttpContext.Session.GetString("LastDifficulty") ?? "easy";
                return StartGame(boardSize, difficulty); // Call the POST overload directly
            }

            // Otherwise, show the settings selection view
            return View();
        }

        [HttpPost]
        public IActionResult StartGame(string boardSize = "small", string difficulty = "easy")
        {
            // Set up board size and difficulty
            int size = boardSize switch { "small" => 9, "medium" => 16, "large" => 24, _ => 9 };
            double difficultyLevel = difficulty switch { "easy" => 0.10, "medium" => 0.15, "hard" => 0.20, _ => 0.15 };

            // Store last settings in session
            HttpContext.Session.SetString("LastBoardSize", boardSize);
            HttpContext.Session.SetString("LastDifficulty", difficulty);

            // Initialize the game board
            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();

            // Store the board in session
            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            HttpContext.Session.SetString("GameOver", "false");

            // Start the timer
            HttpContext.Session.SetString("StartTime", DateTime.UtcNow.ToString());

            return RedirectToAction("MineSweeperBoard");
        }

        public IActionResult MineSweeperBoard()
        {
            // Deserialize the board from session and pass it to the view
            var boardJson = HttpContext.Session.GetString("CurrentBoard");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            Board board;
            try
            {
                board = JsonConvert.DeserializeObject<Board>(boardJson);
            }
            catch (JsonException)
            {
                // Handle deserialization error
                return RedirectToAction("StartGame");
            }

            if (board == null || board.Grid == null)
            {
                return RedirectToAction("StartGame");
            }

            ViewData["GameOver"] = HttpContext.Session.GetString("GameOver") == "true";

            return View(board);
        }

        [HttpPost]
        public IActionResult RevealCell(int row, int col)
        {
            var boardJson = HttpContext.Session.GetString("CurrentBoard");
            if (string.IsNullOrEmpty(boardJson))
            {
                return RedirectToAction("StartGame");
            }

            Board board;
            try
            {
                board = JsonConvert.DeserializeObject<Board>(boardJson);
            }
            catch (JsonException)
            {
                return RedirectToAction("StartGame");
            }

            if (board == null || board.Grid == null)
            {
                return RedirectToAction("StartGame");
            }

            if (HttpContext.Session.GetString("GameOver") == "true")
            {
                return View("MineSweeperBoard", board);
            }

            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size)
            {
                return View("MineSweeperBoard", board);
            }

            var cell = board.Grid[row][col];

            if (cell.Visited)
            {
                return View("MineSweeperBoard", board);
            }

            if (cell.Live)
            {
                board.RevealAllBombs();
                HttpContext.Session.SetString("GameOver", "true");

                // Set a flag to display the loss message on the MineSweeperBoard view
                ViewData["GameOver"] = true;
                ViewData["LossMessage"] = "Game Over! You clicked on a mine.";

                return View("MineSweeperBoard", board);
            }
            else if (cell.LiveNeighbors > 0)
            {
                cell.Visited = true;
            }
            else
            {
                board.FloodFill(row, col);
            }

            if (CheckWinCondition(board))
            {
                HttpContext.Session.SetString("GameOver", "true");

                // Retrieve StartTime and compute elapsed time
                var startTimeStr = HttpContext.Session.GetString("StartTime");
                DateTime startTime;
                if (!DateTime.TryParse(startTimeStr, out startTime))
                {
                    startTime = DateTime.UtcNow; // Default to now if parsing fails
                }
                var elapsedTime = DateTime.UtcNow - startTime;

                // Retrieve difficulty from session
                var difficultyStr = HttpContext.Session.GetString("LastDifficulty") ?? "easy";

                // Compute score
                int baseScore = 1000; // Base score
                double sizeMultiplier = board.Size / 9.0; // Since 9 is the small board
                double difficultyMultiplier = difficultyStr switch
                {
                    "easy" => 1.0,
                    "medium" => 1.5,
                    "hard" => 2.0,
                    _ => 1.0
                };
                double timePenalty = elapsedTime.TotalSeconds;
                int finalScore = (int)(baseScore * sizeMultiplier * difficultyMultiplier / timePenalty);

                // Set ViewData for the win message, including the score and elapsed time
                ViewData["GameOver"] = true;
                ViewData["Score"] = finalScore;
                ViewData["ElapsedTime"] = elapsedTime.ToString("mm\\:ss");

                // Pass the board back to the MineSweeperBoard view
                return View("MineSweeperBoard", board);
            }

            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            return View("MineSweeperBoard", board);
        }

        private bool CheckWinCondition(Board board)
        {
            foreach (var row in board.Grid)
            {
                foreach (var cell in row)
                {
                    if (!cell.Live && !cell.Visited)
                        return false; // If any non-mine cell is unvisited, player hasn't won
                }
            }
            return true; // All non-mine cells are revealed, player has won
        }

        [HttpPost]
        public IActionResult RestartGame()
        {
            // Redirect to StartGame with isRestart flag set to true
            return RedirectToAction("StartGame", new { isRestart = true });
        }
    }
}
