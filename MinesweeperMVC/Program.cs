using MinesweeperMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MinesweeperMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register DbContext with DI
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            builder.Services.AddDbContext<MinesweeperDbContext>(options =>
                options.UseMySQL(connectionString));

            // Register HttpContextAccessor for session use
            builder.Services.AddHttpContextAccessor();

            // Add session services with configuration
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Prevent client-side script access
                options.Cookie.IsEssential = true; // Make the cookie essential for compliance
            });

            // Add controllers with views
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Session middleware
            app.UseSession();

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Endpoint Routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
