using Microsoft.EntityFrameworkCore;

namespace MinesweeperMVC.Models
{
    public class MinesweeperDbContext : DbContext
    {
        public MinesweeperDbContext(DbContextOptions<MinesweeperDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
