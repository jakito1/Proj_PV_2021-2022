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
            IdentityRole? clientRole = new("client");
            if (!await roleManager.RoleExistsAsync(clientRole.Name))
            {
                await roleManager.CreateAsync(clientRole);
            }

            IdentityRole? gymRole = new("gym");
            if (!await roleManager.RoleExistsAsync(gymRole.Name))
            {
                await roleManager.CreateAsync(gymRole);
            }

            IdentityRole? trainerRole = new("trainer");
            if (!await roleManager.RoleExistsAsync(trainerRole.Name))
            {
                await roleManager.CreateAsync(trainerRole);
            }

            IdentityRole? nutritionistRole = new("nutritionist");
            if (!await roleManager.RoleExistsAsync(nutritionistRole.Name))
            {
                await roleManager.CreateAsync(nutritionistRole);
            }

            IdentityRole? adminRole = new("administrator");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }
        }

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager, ApplicationDbContext context)
        {
            if (userManager.FindByNameAsync("admin").Result is null)
            {
                UserAccountModel? admin = new() { UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true };
                IdentityResult? result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? gymTest = new() { UserName = "gym", Email = "gym@gym.pt", EmailConfirmed = true };
                IdentityResult? result1 = await userManager.CreateAsync(gymTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest = new() { UserName = "client", Email = "client@client.pt", EmailConfirmed = true };
                IdentityResult? result2 = await userManager.CreateAsync(clientTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? nutritionistTest = new() { UserName = "nutritionist", Email = "nutritionist@nutritionist.pt", EmailConfirmed = true };
                IdentityResult? result3 = await userManager.CreateAsync(nutritionistTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? trainerTest = new() { UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true };
                IdentityResult? result4 = await userManager.CreateAsync(trainerTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? admin2 = new() { UserName = "admin2", Email = "admin2@admin2.pt", EmailConfirmed = true };
                IdentityResult? result5 = await userManager.CreateAsync(admin2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest2 = new() { UserName = "clientTest2", Email = "clientTest2@clientTest2.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest3 = new() { UserName = "clientTest3", Email = "clientTest3@clientTest3.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest3, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest4 = new() { UserName = "clientTest4", Email = "clientTest4@clientTest4.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest4, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest5 = new() { UserName = "clientTest5", Email = "clientTest5@clientTest5.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest5, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest6 = new() { UserName = "clientTest6", Email = "clientTest6@clientTest6.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest6, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest7 = new() { UserName = "clientTest7", Email = "clientTest7@clientTest7.pt", EmailConfirmed = true };
                await userManager.CreateAsync(clientTest7, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? nutritionistTest2 = new() { UserName = "nutritionistTest2", Email = "nutritionistTest2@nutritionistTest2.pt", EmailConfirmed = true };
                await userManager.CreateAsync(nutritionistTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? trainerTest2 = new() { UserName = "trainerTest2", Email = "trainerTest2@trainerTest2.pt", EmailConfirmed = true };
                await userManager.CreateAsync(trainerTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");

                Gym gym = new() { GymName = "Teste", UserAccountModel = gymTest };
                Client client = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest, Gym = gym };
                Client client2 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest2, Gym = gym };
                Client client3 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest3, Gym = gym };
                Client client4 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest4, Gym = gym };
                Client client5 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest5, Gym = gym };
                Client client6 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest6, Gym = gym };
                Client client7 = new() { Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest7, Gym = gym };
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
                Nutritionist nutritionist2 = new()
                {
                    NutritionistFirstName = "Teste",
                    NutritionistLastName = "Teste2",
                    UserAccountModel = nutritionistTest2,
                    Gym = gym,
                    Clients = new List<Client> { client2 }
                };
                Trainer trainer2 = new()
                {
                    TrainerFirstName = "Teste",
                    TrainerLastName = "Teste2",
                    UserAccountModel = trainerTest2,
                    Gym = gym,
                    Clients = new List<Client> { client2 }
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
                    await context.Client.AddAsync(client2);
                    await context.Client.AddAsync(client3);
                    await context.Client.AddAsync(client4);
                    await context.Client.AddAsync(client5);
                    await context.Client.AddAsync(client6);
                    await context.Client.AddAsync(client7);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(clientTest, "client");
                    await userManager.AddToRoleAsync(clientTest2, "client");
                    await userManager.AddToRoleAsync(clientTest3, "client");
                    await userManager.AddToRoleAsync(clientTest4, "client");
                    await userManager.AddToRoleAsync(clientTest5, "client");
                    await userManager.AddToRoleAsync(clientTest6, "client");
                    await userManager.AddToRoleAsync(clientTest7, "client");

                }
                if (result3.Succeeded)
                {
                    await context.Nutritionist.AddAsync(nutritionist);
                    await context.Nutritionist.AddAsync(nutritionist2);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(nutritionistTest, "nutritionist");
                    await userManager.AddToRoleAsync(nutritionistTest2, "nutritionist");
                }
                if (result4.Succeeded)
                {
                    await context.Trainer.AddAsync(trainer);
                    await context.Trainer.AddAsync(trainer2);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(trainerTest, "trainer");
                    await userManager.AddToRoleAsync(trainerTest2, "trainer");
                }
                if (result5.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin2, "administrator");
                }
            }
        }
    }
}
