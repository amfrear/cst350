using Microsoft.EntityFrameworkCore;

namespace MinesweeperMVC.Models
{
    // This class represents the database context for the Minesweeper application.
    // It inherits from DbContext, which is provided by Entity Framework Core.
    public class MinesweeperDbContext : DbContext
    {
        // Constructor for the MinesweeperDbContext.
        // Takes DbContextOptions as a parameter to configure the context, such as specifying the database provider and connection string.
        public MinesweeperDbContext(DbContextOptions<MinesweeperDbContext> options) : base(options) { }

        // DbSet<User> represents a table in the database where User entities are stored.
        // It allows querying, adding, updating, and deleting User records in the database.
        public DbSet<User> Users { get; set; }
    }
}
