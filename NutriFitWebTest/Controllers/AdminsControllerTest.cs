using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class NutrifitContextFixture
    ***REMOVED***
        public ApplicationDbContext DbContext ***REMOVED*** get; private set; ***REMOVED***

        public NutrifitContextFixture()
        ***REMOVED***
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();
    ***REMOVED***
***REMOVED***
    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private ApplicationDbContext _context;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;
    ***REMOVED***

        [Fact]
        public async Task ShowAllUsers_ReturnsViewResult()
        ***REMOVED***
                var controller = new AdminsController(_context);

                var result = await controller.ShowAllUsers(null);

                Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        ***REMOVED***
                var controller = new AdminsController(_context);

                var result = await controller.DeleteUserAccount(null, null);

                Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact (Skip = "Can't add data to context")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        ***REMOVED***
            var controller = new AdminsController(_context);
            Trainer mockTrainer = new()
            ***REMOVED***
                TrainerId = 1,
                TrainerFirstName = "Luis",
                TrainerLastName = "Carvalho",
                Gym = null,
                UserAccountModel = new UserAccountModel ***REMOVED*** UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true ***REMOVED***,
                Clients = null,
        ***REMOVED***;

            string? trainerId = mockTrainer.UserAccountModel.Id;

             _context.Trainer?.Add(mockTrainer);

            var result = await controller.DeleteUserAccount(trainerId, "~/home");
    ***REMOVED***
***REMOVED***
***REMOVED***
