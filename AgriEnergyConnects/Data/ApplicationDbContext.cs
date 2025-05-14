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
