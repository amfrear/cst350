namespace MinesweeperMVC.Models
{
    // Represents the game board for Minesweeper.
    public class Board
    {
        // A 2D array representing the grid of cells on the board.
        private Cell[][] grid;

        // The size of the game board (number of cells in a row/column).
        public int Size { get; }

        // Difficulty level of the game, affects the number of mines on the board.
        public double Difficulty { get; set; } = 0.15;

        // Constructor for creating a new board with a specified size.
        public Board(int size)
        {
            this.Size = size;

            // Initialize the 2D array of cells.
            grid = new Cell[size][];
            for (int i = 0; i < size; i++)
            {
                grid[i] = new Cell[size];
                for (int j = 0; j < size; j++)
                {
                    // Initialize each cell and set its row and column.
                    grid[i][j] = new Cell
                    {
                        Row = i,
                        Column = j
                    };
                }
            }
        }

        // Getter to access the private grid from outside the class.
        public Cell[][] Grid
        {
            get { return grid; }
        }

        // Method to randomly place mines on the board based on the difficulty.
        public void SetupLiveNeighbors()
        {
            // Calculate the total number of mines to place.
            int totalCells = Size * Size;
            int liveCells = (int)(totalCells * Difficulty);
            Random rand = new Random();

            // Place mines randomly until the required number of mines is reached.
            while (liveCells > 0)
            {
                int i = rand.Next(Size);
                int j = rand.Next(Size);
                // If the selected cell isn't already a mine, make it a mine.
                if (!grid[i][j].Live)
                {
                    grid[i][j].Live = true;
                    liveCells--;
                }
            }
        }

        // Method to calculate the number of mines adjacent to each cell.
        public void CalculateLiveNeighbors()
        {
            // Iterate through each cell on the board.
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    // Skip the calculation for cells that are mines.
                    if (grid[i][j].Live)
                    {
                        continue;
                    }

                    // Count the adjacent mines.
                    int liveCount = 0;
                    // Check all surrounding cells.
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            // Avoid checking the cell itself.
                            if (x == 0 && y == 0) continue;

                            int ni = i + x;
                            int nj = j + y;

                            // Check within grid bounds and for live (mine) cells.
                            if (ni >= 0 && ni < Size && nj >= 0 && nj < Size && grid[ni][nj].Live)
                            {
                                liveCount++;
                            }
                        }
                    }

                    // Set the count of live neighbors for the cell.
                    grid[i][j].LiveNeighbors = liveCount;
                }
            }
        }

        // Recursive method to perform a flood fill from a given cell.
        public void FloodFill(int row, int col)
        {
            // Check for boundary conditions.
            if (row < 0 || col < 0 || row >= Size || col >= Size)
            {
                return;
            }

            // Retrieve the cell from the grid.
            Cell cell = grid[row][col];

            // If cell is already visited or is a mine, stop the flood fill.
            if (cell.Visited || cell.Live)
            {
                return;
            }

            // Mark the cell as visited.
            cell.Visited = true;

            // If the cell has neighboring mines, stop the flood fill.
            if (cell.LiveNeighbors > 0)
            {
                return;
            }

            // Recursively flood fill the neighboring cells.
            FloodFill(row - 1, col);     // North
            FloodFill(row + 1, col);     // South
            FloodFill(row, col - 1);     // West
            FloodFill(row, col + 1);     // East
            FloodFill(row - 1, col - 1); // Northwest
            FloodFill(row - 1, col + 1); // Northeast
            FloodFill(row + 1, col - 1); // Southwest
            FloodFill(row + 1, col + 1); // Southeast
        }
    }
}