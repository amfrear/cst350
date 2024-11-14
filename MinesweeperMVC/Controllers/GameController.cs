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
            int size = boardSize switch
            {
                "small" => 9,
                "medium" => 16,
                "large" => 24,
                _ => 9
            };

            double difficultyLevel = difficulty switch
            {
                "easy" => 0.10,
                "medium" => 0.15,
                "hard" => 0.20,
                _ => 0.15
            };

            // Store the last used board size and difficulty in the session
            HttpContext.Session.SetString("LastBoardSize", boardSize);
            HttpContext.Session.SetString("LastDifficulty", difficulty);

            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();

            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            HttpContext.Session.SetString("GameOver", "false");

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
                return View("Loss", board); // Redirect to Loss view on game over
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
                return View("Win", board); // Redirect to Win view if the player wins
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
