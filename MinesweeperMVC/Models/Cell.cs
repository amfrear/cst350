using Newtonsoft.Json;

namespace MinesweeperMVC.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Cell
    {
        [JsonProperty]
        public int Row { get; set; } = -1;

        [JsonProperty]
        public int Column { get; set; } = -1;

        [JsonProperty]
        public bool Visited { get; set; } = false;

        [JsonProperty]
        public bool Live { get; set; } = false;

        [JsonProperty]
        public int LiveNeighbors { get; set; } = 0;

        [JsonProperty]
        public bool Flagged { get; set; } = false;

        public Cell()
        {
            // Default constructor for serialization
        }
    }
}
