using BibleSearchApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BibleSearchApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the database context
            builder.Services.AddDbContext<BibleDbContext>(options =>
            options.UseMySql(
            builder.Configuration.GetConnectionString("BibleDatabase"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BibleDatabase"))
            ));

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

            app.UseAuthorization();

            // Set default route to SearchController
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Search}/{action=SearchByKeyword}/{id?}");

            app.Run();
        }
    }
}
