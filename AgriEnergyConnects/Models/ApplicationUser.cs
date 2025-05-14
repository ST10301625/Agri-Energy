using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnects.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Users first and last name
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

    }
}
