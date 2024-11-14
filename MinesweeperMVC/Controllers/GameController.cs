using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using Newtonsoft.Json;

namespace MinesweeperMVC.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public IActionResult StartGame()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public IActionResult StartGame(string boardSize, string difficulty)
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

            var board = new Board(size) { Difficulty = difficultyLevel };
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();

            // Serialize the board and store it in the session
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
                return RedirectToAction("StartGame"); // Handle deserialization error
            }

            if (board == null || board.Grid == null)
            {
                return RedirectToAction("StartGame");
            }

            // Check if the game is already over
            if (HttpContext.Session.GetString("GameOver") == "true")
            {
                ViewData["GameOver"] = true;
                return View("MineSweeperBoard", board);
            }

            // Validate cell coordinates
            if (row < 0 || row >= board.Size || col < 0 || col >= board.Size)
            {
                return View("MineSweeperBoard", board); // Invalid cell coordinates, just reload the board
            }

            var cell = board.Grid[row][col];

            if (cell.Visited)
            {
                // Cell already revealed, no action needed
                return View("MineSweeperBoard", board);
            }

            if (cell.Live)
            {
                // Game over: reveal all mines and set GameOver flag
                board.RevealAllBombs();
                HttpContext.Session.SetString("GameOver", "true");
                ViewData["GameOver"] = true;
            }
            else if (cell.LiveNeighbors > 0)
            {
                // If the cell has neighboring mines, simply reveal it
                cell.Visited = true;
            }
            else
            {
                // If the cell has no neighboring mines, perform FloodFill
                board.FloodFill(row, col);
            }

            // Check for win condition
            if (CheckWinCondition(board))
            {
                ViewData["WinMessage"] = "Congratulations! You've won the game!";
                HttpContext.Session.SetString("GameOver", "true"); // End game on win
                ViewData["GameOver"] = true;
            }

            // Update session with new board state
            HttpContext.Session.SetString("CurrentBoard", JsonConvert.SerializeObject(board));
            ViewData["GameOver"] = HttpContext.Session.GetString("GameOver") == "true";

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
    }
}
