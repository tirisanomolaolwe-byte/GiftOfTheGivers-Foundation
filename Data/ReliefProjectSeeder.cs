using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftOfTheGivers.Models;

namespace GiftOfTheGivers.Data
{
    public static class ReliefProjectSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.ReliefProjects.Any())
            {
                return;
            }

            var projects = new List<ReliefProject>
            {
                new ReliefProject { Name = "KwaZulu-Natal Flood Relief", Location = "KwaZulu-Natal", Description = "Disaster Response", Status = "Active" },
                new ReliefProject { Name = "Eastern Cape Food Relief Programme", Location = "Eastern Cape", Description = "Hunger Alleviation", Status = "Active" },
                new ReliefProject { Name = "Limpopo Water Access Initiative", Location = "Limpopo", Description = "Water Provision", Status = "Active" },
                new ReliefProject { Name = "Gauteng Community Healthcare Support", Location = "Gauteng", Description = "Healthcare", Status = "Active" },
                new ReliefProject { Name = "Western Cape School Support Programme", Location = "Western Cape", Description = "Education", Status = "Planned" },
                new ReliefProject { Name = "KwaZulu-Natal Community Development", Location = "KwaZulu-Natal", Description = "Human Development", Status = "Active" },
                new ReliefProject { Name = "Mpumalanga Emergency Food Distribution", Location = "Mpumalanga", Description = "Hunger Alleviation", Status = "Completed" },
                new ReliefProject { Name = "Eastern Cape Disaster Recovery", Location = "Eastern Cape", Description = "Disaster Response", Status = "Completed" }
            };

            context.ReliefProjects.AddRange(projects);
            await context.SaveChangesAsync();
        }
    }
}
