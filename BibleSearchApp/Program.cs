/*!
 * BibleSearchApp
 * 
 * File: Program.cs
 * Description: Configures and initializes the ASP.NET Core web application.
 *              Sets up services, middleware, and routing for handling HTTP requests.
 *              Registers the BibleDbContext for database interactions using MySQL.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

using BibleSearchApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BibleSearchApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize the WebApplicationBuilder with command-line arguments.
            var builder = WebApplication.CreateBuilder(args);

            // =============================================
            // 1. Register Services to the Dependency Injection (DI) Container
            // =============================================

            // Add MVC services to the container with support for controllers and views.
            builder.Services.AddControllersWithViews();

            // Register the BibleDbContext with the DI container using MySQL as the database provider.
            // Configuration settings (like connection strings) are retrieved from the appsettings.json file.
            builder.Services.AddDbContext<BibleDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("BibleDatabase"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BibleDatabase"))
                )
            );

            // =============================================
            // 2. Build the WebApplication
            // =============================================
            var app = builder.Build();

            // =============================================
            // 3. Configure the HTTP Request Pipeline
            // =============================================

            // If the application is not running in the Development environment, configure exception handling and HSTS.
            if (!app.Environment.IsDevelopment())
            {
                // Use a generic error handling page located at /Home/Error.
                app.UseExceptionHandler("/Home/Error");

                // Enforce the use of HTTPS by adding the Strict-Transport-Security header.
                app.UseHsts();
            }

            // Redirect all HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Enable serving static files (e.g., CSS, JavaScript, images) from the wwwroot folder.
            app.UseStaticFiles();

            // Enable routing capabilities.
            app.UseRouting();

            // Enable authorization middleware. Note: Authentication middleware should be added before this if authentication is required.
            app.UseAuthorization();

            // =============================================
            // 4. Define Endpoint Routing
            // =============================================

            // Map controller routes using the specified pattern.
            // The default route directs to the SearchController's SearchByKeyword action.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Search}/{action=SearchByKeyword}/{id?}"
            );

            // =============================================
            // 5. Run the Application
            // =============================================
            app.Run();
        }
    }
}
