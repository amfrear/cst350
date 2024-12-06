using Microsoft.EntityFrameworkCore;

namespace MinesweeperMVC.Models
{
    // This class represents the database context for the Minesweeper application.
    public class MinesweeperDbContext : DbContext
    {
        // Constructor for the MinesweeperDbContext.
        public MinesweeperDbContext(DbContextOptions<MinesweeperDbContext> options) : base(options) { }

        // DbSet<User> represents the Users table in the database.
        public DbSet<User> Users { get; set; }

        // DbSet<Game> represents the Games table in the database.
        public DbSet<Game> Games { get; set; }
    }
}
