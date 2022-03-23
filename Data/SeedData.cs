﻿using Microsoft.AspNetCore.Identity;
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
            IdentityRole? clientRole = new("client");
            if (!await roleManager.RoleExistsAsync(clientRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(clientRole);
        ***REMOVED***

            IdentityRole? gymRole = new("gym");
            if (!await roleManager.RoleExistsAsync(gymRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(gymRole);
        ***REMOVED***

            IdentityRole? trainerRole = new("trainer");
            if (!await roleManager.RoleExistsAsync(trainerRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(trainerRole);
        ***REMOVED***

            IdentityRole? nutritionistRole = new("nutritionist");
            if (!await roleManager.RoleExistsAsync(nutritionistRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(nutritionistRole);
        ***REMOVED***

            IdentityRole? adminRole = new("administrator");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            ***REMOVED***
                await roleManager.CreateAsync(adminRole);
        ***REMOVED***
    ***REMOVED***

        private static async Task SeedUsersAsync(UserManager<UserAccountModel> userManager, ApplicationDbContext context)
        ***REMOVED***
            if (userManager.FindByNameAsync("admin").Result is null)
            ***REMOVED***
                UserAccountModel? admin = new() ***REMOVED*** UserName = "admin", Email = "admin@admin.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result = await userManager.CreateAsync(admin, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? gymTest = new() ***REMOVED*** UserName = "gym", Email = "gym@gym.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result1 = await userManager.CreateAsync(gymTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest = new() ***REMOVED*** UserName = "client", Email = "client@client.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result2 = await userManager.CreateAsync(clientTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? nutritionistTest = new() ***REMOVED*** UserName = "nutritionist", Email = "nutritionist@nutritionist.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result3 = await userManager.CreateAsync(nutritionistTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? trainerTest = new() ***REMOVED*** UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result4 = await userManager.CreateAsync(trainerTest, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? admin2 = new() ***REMOVED*** UserName = "admin2", Email = "admin2@admin2.pt", EmailConfirmed = true ***REMOVED***;
                IdentityResult? result5 = await userManager.CreateAsync(admin2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest2 = new() ***REMOVED*** UserName = "clientTest2", Email = "clientTest2@clientTest2.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest3 = new() ***REMOVED*** UserName = "clientTest3", Email = "clientTest3@clientTest3.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest3, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest4 = new() ***REMOVED*** UserName = "clientTest4", Email = "clientTest4@clientTest4.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest4, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest5 = new() ***REMOVED*** UserName = "clientTest5", Email = "clientTest5@clientTest5.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest5, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest6 = new() ***REMOVED*** UserName = "clientTest6", Email = "clientTest6@clientTest6.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest6, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? clientTest7 = new() ***REMOVED*** UserName = "clientTest7", Email = "clientTest7@clientTest7.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(clientTest7, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? nutritionistTest2 = new() ***REMOVED*** UserName = "nutritionistTest2", Email = "nutritionistTest2@nutritionistTest2.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(nutritionistTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");
                UserAccountModel? trainerTest2 = new() ***REMOVED*** UserName = "trainerTest2", Email = "trainerTest2@trainerTest2.pt", EmailConfirmed = true ***REMOVED***;
                await userManager.CreateAsync(trainerTest2, "4p^91S!Mpu&tZgrfmiA^fWT&L");

                Gym gym = new() ***REMOVED*** GymName = "Teste", UserAccountModel = gymTest ***REMOVED***;
                Client client = new() ***REMOVED*** Height = 100, Weight = 100, ClientBirthday = DateTime.Parse("01/01/1990"), UserAccountModel = clientTest, Gym = gym ***REMOVED***;
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
