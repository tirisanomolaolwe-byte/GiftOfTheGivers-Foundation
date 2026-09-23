using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReliefProject> ReliefProjects { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Donation> Donations { get; set; }
    }
}