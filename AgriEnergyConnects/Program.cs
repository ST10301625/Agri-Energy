using AgriEnergyConnects.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnects.Models;

namespace AgriEnergyConnects
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext (BEFORE building the app)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register default Identity with ApplicationUser (not IdentityUser)
            builder.Services.AddDefaultIdentity<ApplicationUser>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()  // If you're using roles
                .AddEntityFrameworkStores<ApplicationDbContext>(); // Ensure correct DbContext is used

            // Add Identity services explicitly for SignInManager and UserManager
            builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Enable static file serving (for CSS, JS, and image files)
            app.UseStaticFiles();

            // Map Razor Pages (for Identity)
            app.MapRazorPages();

            // Configure default route for controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
