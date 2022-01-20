using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
{
    public static class SeedData
    {
        public static async Task Seed(UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var clientRole = new IdentityRole("client");
            if (!await roleManager.RoleExistsAsync(clientRole.Name))
            {
                await roleManager.CreateAsync(clientRole);
            }

            var gymRole = new IdentityRole("gym");
            if (!await roleManager.RoleExistsAsync(gymRole.Name))
            {
                await roleManager.CreateAsync(gymRole);
            }

            var trainerRole = new IdentityRole("trainer");
            if (!await roleManager.RoleExistsAsync(trainerRole.Name))
            {
                await roleManager.CreateAsync(trainerRole);
            }

            var nutritionistRole = new IdentityRole("nutritionist");
            if (!await roleManager.RoleExistsAsync(nutritionistRole.Name))
            {
                await roleManager.CreateAsync(nutritionistRole);
            }

            var adminRole = new IdentityRole("administrator");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }
        }

        private static async Task SeedUsersAsync(UserManager<UserAccount> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var admin = new UserAccount { UserName = "admin@admin.pt", Email = "admin@admin.pt", EmailConfirmed = true};
                var result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "administrator");
                }
            }
        }

    }
}
