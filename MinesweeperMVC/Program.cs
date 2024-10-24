using Microsoft.EntityFrameworkCore;
using MinesweeperMVC.Data;

namespace MinesweeperMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add session services
            builder.Services.AddDistributedMemoryCache(); // Adds memory cache to store session data
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout (30 minutes)
                options.Cookie.HttpOnly = true; // Make session cookie HTTP-only (not accessible by JavaScript)
                options.Cookie.IsEssential = true; // Ensure session cookie is always set
            });

            // Configure MySQL Database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21)))); // Adjust MySQL version if necessary

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable session before authorization
            app.UseSession(); // Adds session middleware to handle sessions

            app.UseAuthorization();

            // Default routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Register routing
            app.MapControllerRoute(
                name: "register",
                pattern: "Register",
                defaults: new { controller = "Register", action = "Register" }
            );

            // Login route (simplified to avoid repeating "Login")
            app.MapControllerRoute(
                name: "login",
                pattern: "Login", // Now just '/Login'
                defaults: new { controller = "Login", action = "Login" }
            );

            // Game/StartGame route
            app.MapControllerRoute(
                name: "game",
                 pattern: "Game/StartGame",
                defaults: new { controller = "Game", action = "StartGame" }
            );

            app.Run();
        }
    }
}
