using System;
using System.ComponentModel.DataAnnotations;

namespace MinesweeperMVC.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; } // User ID associated with the game
        public DateTime DateSaved { get; set; } // Save timestamp
        public string GameData { get; set; } // Serialized game data (JSON)
    }
}
