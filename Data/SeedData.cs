using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
***REMOVED***
    /// <summary>
    /// SeedData class
    /// </summary>
    public static class SeedData
    ***REMOVED***
        /// <summary>
        /// Tries to create the roles and the admin
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="roleManager">Provides the APIs for managing roles in a persistence store.</param>
        /// <returns></returns>
        public static async Task Seed(UserManager<UserAccountModel> userManager, RoleManager<IdentityRole> roleManager)
        ***REMOVED***
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
    ***REMOVED***

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        ***REMOVED***
            var clientRole = new IdentityRole("client");
            if (!await roleManager.RoleExistsAsync(clientRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(clientRole);
        ***REMOVED***

            var gymRole = new IdentityRole("gym");
            if (!await roleManager.RoleExistsAsync(gymRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(gymRole);
        ***REMOVED***

            var trainerRole = new IdentityRole("trainer");
            if (!await roleManager.RoleExistsAsync(trainerRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(trainerRole);
        ***REMOVED***

            var nutritionistRole = new IdentityRole("nutritionist");
            if (!await roleManager.RoleExistsAsync(nutritionistRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(nutritionistRole);
        ***REMOVED***

            var adminRole = new IdentityRole("administrator");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(adminRole);
        ***REMOVED***
    ***REMOVED***

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager)
        ***REMOVED***
            if (userManager.FindByNameAsync("admin").Result == null)
            ***REMOVED***
                var admin = new UserAccountModel ***REMOVED*** UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true***REMOVED***;
                var result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                if (result.Succeeded)
                ***REMOVED***
                    await userManager.AddToRoleAsync(admin, "administrator");
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

***REMOVED***
***REMOVED***
