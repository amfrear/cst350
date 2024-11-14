namespace MinesweeperMVC.Models
{
    // The Cell class represents the basic unit within the Minesweeper game grid.
    public class Cell
    {
        // The row position of the cell within the Minesweeper grid.
        public int Row { get; set; } = -1;

        // The column position of the cell within the Minesweeper grid.
        public int Column { get; set; } = -1;

        // Indicates whether the cell has been revealed or visited by the player.
        public bool Visited { get; set; } = false;

        // Indicates whether the cell is a mine (live) or not.
        public bool Live { get; set; } = false;

        // The number of adjacent cells that are mines (live).
        // This is only relevant if the cell itself is not a mine.
        public int LiveNeighbors { get; set; } = 0;
    }
}
