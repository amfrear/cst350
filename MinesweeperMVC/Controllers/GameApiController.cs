using Microsoft.AspNetCore.Mvc;
using MinesweeperMVC.Models;
using System.Linq;

namespace MinesweeperMVC.Controllers
{
    [Route("api")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        private readonly MinesweeperDbContext _context;

        public GameApiController(MinesweeperDbContext context)
        {
            _context = context;
        }

        // Endpoint: GET localhost/api/showSavedGames
        [HttpGet("showSavedGames")]
        public IActionResult GetAllSavedGames()
        {
            var savedGames = _context.Games
                .OrderByDescending(g => g.DateSaved)
                .Select(g => new
                {
                    g.Id,
                    g.UserId,
                    g.DateSaved,
                    GameDataPreview = g.GameData.Length > 50 ? g.GameData.Substring(0, 50) + "..." : g.GameData
                })
                .ToList();

            return Ok(savedGames);
        }

        // Endpoint: GET localhost/api/showSavedGames/{id}
        [HttpGet("showSavedGames/{id}")]
        public IActionResult GetGameById(int id)
        {
            var game = _context.Games
                .FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound(new { Message = $"Game with ID {id} not found." });
            }

            return Ok(game);
        }

        // Endpoint: DELETE localhost/api/deleteOneGame/{id}
        [HttpDelete("deleteOneGame/{id}")]
        public IActionResult DeleteGameById(int id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound(new { Message = $"Game with ID {id} not found." });
            }

            _context.Games.Remove(game);
            _context.SaveChanges();

            return Ok(new { Message = $"Game with ID {id} deleted successfully." });
        }
    }
}
