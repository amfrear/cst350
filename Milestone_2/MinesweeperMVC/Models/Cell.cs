using Newtonsoft.Json;

namespace MinesweeperMVC.Models
{
    // This class represents a single cell in the Minesweeper game board.
    // It contains properties to track the cell's state, position, and behavior during gameplay.
    [JsonObject(MemberSerialization.OptIn)] // Specifies that only explicitly marked properties will be serialized.
    public class Cell
    {
        // The row index of the cell on the game board.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public int Row { get; set; } = -1;

        // The column index of the cell on the game board.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public int Column { get; set; } = -1;

        // Indicates whether this cell has been revealed during gameplay.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public bool Visited { get; set; } = false;

        // Indicates whether this cell contains a mine.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public bool Live { get; set; } = false;

        // The number of neighboring cells that contain mines.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public int LiveNeighbors { get; set; } = 0;

        // Indicates whether this cell has been flagged by the player.
        // Flagging is used by players to mark cells they suspect contain mines.
        [JsonProperty] // Marks this property for inclusion during JSON serialization.
        public bool Flagged { get; set; } = false;

        // Default constructor, required for serialization and deserialization purposes.
        // This ensures the `Cell` class can be properly initialized during JSON operations.
        public Cell()
        {
        }
    }
}
