using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnects.Models;

namespace AgriEnergyConnects.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Ensure ApplicationUser is used here
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

//Sameer Saini(2024). ASP.NET Core MVC CRUD Operations using .NET 8 and Entity Framework Core - MVC For Beginners Tutorial. [online]
//YouTube.Available at: <https://www.youtube.com/watch?v=_uSw8sh7xKs> [Accessed 8 May 2024].
