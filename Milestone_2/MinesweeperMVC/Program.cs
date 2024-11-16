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

            // Add authentication with cookie-based authentication
            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.Cookie.Name = "MinesweeperAuthCookie";
                    options.LoginPath = "/Account/Login"; // Redirect to login page
                    options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect to Access Denied page
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
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

            // Add Authentication Middleware
            app.UseAuthentication();

            // Add Session Middleware
            app.UseSession();

            app.UseAuthorization();

            // Endpoint Routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
