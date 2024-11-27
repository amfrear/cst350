using Newtonsoft.Json;

namespace MinesweeperMVC.Models
{
    // Represents the Minesweeper game board, managing its size, difficulty, and cells.
    [JsonObject(MemberSerialization.OptIn)] // Specifies that only explicitly marked properties will be serialized.
    public class Board
    {
        // The size of the board (e.g., 9x9 for a small board).
        [JsonProperty] // Marks this property for JSON serialization.
        public int Size { get; set; }

        // The difficulty level, representing the proportion of mines on the board.
        [JsonProperty] // Marks this property for JSON serialization.
        public double Difficulty { get; set; } = 0.15; // Default difficulty is 15%.

        // The grid of cells representing the board.
        [JsonProperty] // Marks this property for JSON serialization.
        public Cell[][] Grid { get; set; }

        // Default constructor for serialization and deserialization purposes.
        public Board()
        {
        }

        // Constructor to initialize the board with a given size.
        public Board(int size)
        {
            this.Size = size;
            InitializeGrid(size); // Set up the grid with empty cells.
        }

        // Initializes the grid with empty cells, assigning their row and column indices.
        private void InitializeGrid(int size)
        {
            Grid = new Cell[size][];
            for (int i = 0; i < size; i++)
            {
                Grid[i] = new Cell[size];
                for (int j = 0; j < size; j++)
                {
                    Grid[i][j] = new Cell
                    {
                        Row = i,
                        Column = j
                    };
                }
            }
        }

        // Randomly places mines on the board based on the difficulty level.
        public void SetupLiveNeighbors()
        {
            int totalCells = Size * Size; // Total number of cells on the board.
            int liveCells = (int)(totalCells * Difficulty); // Number of mines to place.
            Random rand = new Random();

            while (liveCells > 0)
            {
                // Generate random coordinates for placing a mine.
                int i = rand.Next(Size);
                int j = rand.Next(Size);

                // Only place a mine if the cell does not already contain one.
                if (!Grid[i][j].Live)
                {
                    Grid[i][j].Live = true;
                    liveCells--; // Decrement the remaining mine count.
                }
            }
        }

        // Calculates the number of mines adjacent to each cell on the board.
        public void CalculateLiveNeighbors()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    // Skip cells that are mines.
                    if (Grid[i][j].Live)
                        continue;

                    int liveCount = 0; // Counter for neighboring mines.

                    // Iterate over all neighboring cells.
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            // Skip the current cell.
                            if (x == 0 && y == 0)
                                continue;

                            int ni = i + x; // Neighbor row index.
                            int nj = j + y; // Neighbor column index.

                            // Check if the neighbor is within bounds and contains a mine.
                            if (ni >= 0 && ni < Size && nj >= 0 && nj < Size && Grid[ni][nj].Live)
                            {
                                liveCount++;
                            }
                        }
                    }

                    // Set the count of live neighbors for the current cell.
                    Grid[i][j].LiveNeighbors = liveCount;
                }
            }
        }

        // Performs a flood-fill operation to reveal all connected empty cells.
        public void FloodFill(int row, int col)
        {
            // Ensure the cell is within the grid boundaries.
            if (row < 0 || col < 0 || row >= Size || col >= Size)
            {
                return;
            }

            Cell cell = Grid[row][col];

            // Stop recursion if the cell is already visited or contains a mine.
            if (cell.Visited || cell.Live)
            {
                return;
            }

            // Mark the cell as visited.
            cell.Visited = true;

            // Stop further recursion if the cell has neighboring mines.
            if (cell.LiveNeighbors > 0)
            {
                return;
            }

            // Recursively visit all surrounding cells.
            FloodFill(row - 1, col);     // North
            FloodFill(row + 1, col);     // South
            FloodFill(row, col - 1);     // West
            FloodFill(row, col + 1);     // East
            FloodFill(row - 1, col - 1); // Northwest
            FloodFill(row - 1, col + 1); // Northeast
            FloodFill(row + 1, col - 1); // Southwest
            FloodFill(row + 1, col + 1); // Southeast
        }

        // Reveals all cells containing mines, used when the game ends in a loss.
        public void RevealAllBombs()
        {
            foreach (var row in Grid)
            {
                foreach (var cell in row)
                {
                    if (cell.Live)
                    {
                        cell.Visited = true; // Mark mine cells as visited.
                    }
                }
            }
        }

        public void ToggleFlag(int row, int col)
        {
            // Ensure the cell is within bounds
            if (row >= 0 && col >= 0 && row < Size && col < Size)
            {
                var cell = Grid[row][col];

                // Only toggle the flag if the cell has not been visited
                if (!cell.Visited)
                {
                    cell.Flagged = !cell.Flagged; // Toggle the flagged state
                }
            }
        }
    }
}
