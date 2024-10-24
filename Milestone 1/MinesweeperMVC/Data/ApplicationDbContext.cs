using Microsoft.EntityFrameworkCore;
using MinesweeperMVC.Models;

namespace MinesweeperMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet for each model class you want to store in the database.
        public DbSet<User> Users { get; set; }
    }
}
