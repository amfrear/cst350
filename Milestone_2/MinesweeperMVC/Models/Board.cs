using Newtonsoft.Json;

namespace MinesweeperMVC.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Board
    {
        [JsonProperty]
        public int Size { get; set; }

        [JsonProperty]
        public double Difficulty { get; set; } = 0.15;

        [JsonProperty]
        public Cell[][] Grid { get; set; }

        public Board()
        {
            // Parameterless constructor needed for serialization
        }

        public Board(int size)
        {
            this.Size = size;
            InitializeGrid(size);
        }

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

        // Method to randomly place mines on the board based on the difficulty.
        public void SetupLiveNeighbors()
        {
            int totalCells = Size * Size;
            int liveCells = (int)(totalCells * Difficulty);
            Random rand = new Random();

            while (liveCells > 0)
            {
                int i = rand.Next(Size);
                int j = rand.Next(Size);
                if (!Grid[i][j].Live)
                {
                    Grid[i][j].Live = true;
                    liveCells--;
                }
            }
        }

        // Method to calculate the number of mines adjacent to each cell.
        public void CalculateLiveNeighbors()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Grid[i][j].Live)
                        continue;

                    int liveCount = 0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0)
                                continue;

                            int ni = i + x;
                            int nj = j + y;

                            if (ni >= 0 && ni < Size && nj >= 0 && nj < Size && Grid[ni][nj].Live)
                            {
                                liveCount++;
                            }
                        }
                    }

                    Grid[i][j].LiveNeighbors = liveCount;
                }
            }
        }

        // Recursive method to perform a flood fill from a given cell.
        public void FloodFill(int row, int col)
        {
            // Boundary check to ensure we're within the grid limits
            if (row < 0 || col < 0 || row >= Size || col >= Size)
            {
                return;
            }

            Cell cell = Grid[row][col];

            // Stop if the cell is already visited or contains a mine
            if (cell.Visited || cell.Live)
            {
                return;
            }

            // Mark the cell as visited
            cell.Visited = true;

            // Stop further recursion if there are neighboring mines
            if (cell.LiveNeighbors > 0)
            {
                return;
            }

            // Recursively visit all surrounding cells
            FloodFill(row - 1, col);     // North
            FloodFill(row + 1, col);     // South
            FloodFill(row, col - 1);     // West
            FloodFill(row, col + 1);     // East
            FloodFill(row - 1, col - 1); // Northwest
            FloodFill(row - 1, col + 1); // Northeast
            FloodFill(row + 1, col - 1); // Southwest
            FloodFill(row + 1, col + 1); // Southeast
        }

        public void RevealAllBombs()
        {
            foreach (var row in Grid)
            {
                foreach (var cell in row)
                {
                    if (cell.Live)
                    {
                        cell.Visited = true;
                    }
                }
            }
        }
    }
}
