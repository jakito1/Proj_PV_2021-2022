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
        public static async Task Seed(UserManager<UserAccountModel> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        ***REMOVED***
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager, context);
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

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager, ApplicationDbContext context)
        ***REMOVED***
            if (userManager.FindByNameAsync("admin").Result == null)
            ***REMOVED***
                var admin = new UserAccountModel ***REMOVED*** UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true***REMOVED***;
                var result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var gymTest = new UserAccountModel ***REMOVED*** UserName = "gym", Email = "gym@gym.pt", EmailConfirmed = true ***REMOVED***;
                var result1 = await userManager.CreateAsync(gymTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest = new UserAccountModel ***REMOVED*** UserName = "client", Email = "client@client.pt", EmailConfirmed = true ***REMOVED***;
                var result2 = await userManager.CreateAsync(clientTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var nutritionistTest = new UserAccountModel ***REMOVED*** UserName = "nutritionist", Email = "nutritionist@nutritionist.pt", EmailConfirmed = true ***REMOVED***;
                var result3 = await userManager.CreateAsync(nutritionistTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var trainerTest = new UserAccountModel ***REMOVED*** UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true ***REMOVED***;
                var result4 = await userManager.CreateAsync(trainerTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var admin2 = new UserAccountModel ***REMOVED*** UserName = "admin2", Email = "admin2@admin2.pt", EmailConfirmed = true ***REMOVED***;
                var result5 = await userManager.CreateAsync(admin2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest2 = new UserAccountModel ***REMOVED*** UserName = "clientTest2", Email = "clientTest2@clientTest2.pt", EmailConfirmed = true ***REMOVED***;
                var result6 = await userManager.CreateAsync(clientTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest3 = new UserAccountModel ***REMOVED*** UserName = "clientTest3", Email = "clientTest3@clientTest3.pt", EmailConfirmed = true ***REMOVED***;
                var result7 = await userManager.CreateAsync(clientTest3, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest4 = new UserAccountModel ***REMOVED*** UserName = "clientTest4", Email = "clientTest4@clientTest4.pt", EmailConfirmed = true ***REMOVED***;
                var result8 = await userManager.CreateAsync(clientTest4, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest5 = new UserAccountModel ***REMOVED*** UserName = "clientTest5", Email = "clientTest5@clientTest5.pt", EmailConfirmed = true ***REMOVED***;
                var result9 = await userManager.CreateAsync(clientTest5, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest6 = new UserAccountModel ***REMOVED*** UserName = "clientTest6", Email = "clientTest6@clientTest6.pt", EmailConfirmed = true ***REMOVED***;
                var result10 = await userManager.CreateAsync(clientTest6, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var clientTest7 = new UserAccountModel ***REMOVED*** UserName = "clientTest7", Email = "clientTest7@clientTest7.pt", EmailConfirmed = true ***REMOVED***;
                var result11 = await userManager.CreateAsync(clientTest7, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var nutritionistTest2 = new UserAccountModel ***REMOVED*** UserName = "nutritionistTest2", Email = "nutritionistTest2@nutritionistTest2.pt", EmailConfirmed = true ***REMOVED***;
                var result12 = await userManager.CreateAsync(nutritionistTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                var trainerTest2 = new UserAccountModel ***REMOVED*** UserName = "trainerTest2", Email = "trainerTest2@trainerTest2.pt", EmailConfirmed = true ***REMOVED***;
                var result13 = await userManager.CreateAsync(trainerTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");

                Gym gym = new() ***REMOVED***GymName = "Teste", UserAccountModel = gymTest ***REMOVED***;
                Client client = new() ***REMOVED***Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"),UserAccountModel = clientTest, Gym = gym***REMOVED***;
                Client client2 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest2, Gym = gym ***REMOVED***;
                Client client3 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest3, Gym = gym ***REMOVED***;
                Client client4 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest4, Gym = gym ***REMOVED***;
                Client client5 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest5, Gym = gym ***REMOVED***;
                Client client6 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest6, Gym = gym ***REMOVED***;
                Client client7 = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest7, Gym = gym ***REMOVED***;
                Nutritionist nutritionist = new()
                ***REMOVED***
                    NutritionistFirstName = "Teste",
                    NutritionistLastName = "Teste2",
                    UserAccountModel = nutritionistTest,
                    Gym = gym,
                    Clients = new List<Client> ***REMOVED*** client ***REMOVED***
            ***REMOVED***;
                Trainer trainer = new()
                ***REMOVED***
                    TrainerFirstName = "Teste",
                    TrainerLastName = "Teste2",
                    UserAccountModel = trainerTest,
                    Gym = gym,
                    Clients = new List<Client> ***REMOVED*** client ***REMOVED***
            ***REMOVED***;
                Nutritionist nutritionist2 = new()
                ***REMOVED***
                    NutritionistFirstName = "Teste",
                    NutritionistLastName = "Teste2",
                    UserAccountModel = nutritionistTest2,
                    Gym = gym,
                    Clients = new List<Client> ***REMOVED*** client2 ***REMOVED***
            ***REMOVED***;
                Trainer trainer2 = new()
                ***REMOVED***
                    TrainerFirstName = "Teste",
                    TrainerLastName = "Teste2",
                    UserAccountModel = trainerTest2,
                    Gym = gym,
                    Clients = new List<Client> ***REMOVED*** client2 ***REMOVED***
            ***REMOVED***;

                if (result.Succeeded)
                ***REMOVED***
                    await userManager.AddToRoleAsync(admin, "administrator");
            ***REMOVED***
                if (result1.Succeeded)          
                ***REMOVED***                 
                    await context.Gym.AddAsync(gym);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(gymTest, "gym");
            ***REMOVED***
                if (result2.Succeeded)
                ***REMOVED***                   
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

            ***REMOVED***
                if (result3.Succeeded)
                ***REMOVED***
                    await context.Nutritionist.AddAsync(nutritionist);
                    await context.Nutritionist.AddAsync(nutritionist2);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(nutritionistTest, "nutritionist");
                    await userManager.AddToRoleAsync(nutritionistTest2, "nutritionist");
            ***REMOVED***
                if (result4.Succeeded)
                ***REMOVED***
                    await context.Trainer.AddAsync(trainer);
                    await context.Trainer.AddAsync(trainer2);
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(trainerTest, "trainer");
                    await userManager.AddToRoleAsync(trainerTest2, "trainer");
            ***REMOVED*** 
                if (result5.Succeeded)
                ***REMOVED***
                    await userManager.AddToRoleAsync(admin2, "administrator");
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
