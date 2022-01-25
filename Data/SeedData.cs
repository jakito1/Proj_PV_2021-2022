using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
{
    /// <summary>
    /// SeedData class
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Tries to create the roles and the admin
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="roleManager">Provides the APIs for managing roles in a persistence store.</param>
        /// <returns></returns>
        public static async Task Seed(UserManager<UserAccountModel> userManager, RoleManager<IdentityRole> roleManager)
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

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var admin = new UserAccountModel { UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true};
                var result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "administrator");
                }
            }
        }

    }
}
