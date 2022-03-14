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
        public static async Task Seed(UserManager<UserAccountModel> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager, context);
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

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager, ApplicationDbContext context)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var admin = new UserAccountModel { UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true};
                var result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var gymTest = new UserAccountModel { UserName = "gym", Email = "gym@gym.pt", EmailConfirmed = true };
                var result1 = await userManager.CreateAsync(gymTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest = new UserAccountModel { UserName = "client", Email = "client@client.pt", EmailConfirmed = true };
                var result2 = await userManager.CreateAsync(clientTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var nutritionistTest = new UserAccountModel { UserName = "nutritionist", Email = "nutritionist@nutritionist.pt", EmailConfirmed = true };
                var result3 = await userManager.CreateAsync(nutritionistTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var trainerTest = new UserAccountModel { UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true };
                var result4 = await userManager.CreateAsync(trainerTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var admin2 = new UserAccountModel { UserName = "admin2", Email = "admin2@admin2.pt", EmailConfirmed = true };
                var result5 = await userManager.CreateAsync(admin2, "4p^91S!Mpu&tZgrfmiA^fWT&L");

                Gym gym = new() {GymName = "Teste", UserAccountModel = gymTest };
                Client client = new() {Height = 100, Weight = 100, UserAccountModel = clientTest, Gym = gym};
                Nutritionist nutritionist = new()
                {
                    NutritionistFirstName = "Teste",
                    NutritionistLastName = "Teste2",
                    UserAccountModel = nutritionistTest,
                    Gym = gym,
                    Clients = new List<Client> { client }
                };
                Trainer trainer = new()
                {
                    TrainerFirstName = "Teste",
                    TrainerLastName = "Teste2",
                    UserAccountModel = trainerTest,
                    Gym = gym,
                    Clients = new List<Client> { client }
                };

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "administrator");
                }
                if (result1.Succeeded)          
                {                 
                    await context.Gym.AddAsync(gym);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(gymTest, "gym");
                }
                if (result2.Succeeded)
                {                   
                    await context.Client.AddAsync(client);
                    await context.SaveChangesAsync();                  
                    await userManager.AddToRoleAsync(clientTest, "client");
                }
                if (result3.Succeeded)
                {
                    await context.Nutritionist.AddAsync(nutritionist);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(clientTest, "nutritionist");
                }
                if (result4.Succeeded)
                {
                    await context.Trainer.AddAsync(trainer);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(clientTest, "trainer");
                } 
                if (result5.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin2, "administrator");
                }
            }
        }
    }
}
