using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Employee",
                "Donor"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Could not create role: {role}");
                    }
                }
            }
        }

        public static async Task SeedUsersAsync(
            UserManager<ApplicationUser> userManager)
        {
            await EnsureUserAsync(
                userManager,
                "employee@giftofthegivers.org",
                "Employee@123",
                "Employee");

            await EnsureUserAsync(
                userManager,
                "donor@example.com",
                "Donor@123",
                "Donor");
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult =
                    await userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new Exception(
                        $"Could not create {email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        $"Could not assign {email} to {role}.");
                }
            }
        }
    }
}