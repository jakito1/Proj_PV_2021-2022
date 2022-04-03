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
{
    public class NutrifitContextFixture
    {
        public ApplicationDbContext DbContext { get; private set; }

        public NutrifitContextFixture()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();
        }
    }
    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private ApplicationDbContext _context;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact]
        public async Task ShowAllUsers_ReturnsViewResult()
        {
                var controller = new AdminsController(_context);

                var result = await controller.ShowAllUsers(null);

                Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        {
                var controller = new AdminsController(_context);

                var result = await controller.DeleteUserAccount(null, null);

                Assert.IsType<NotFoundResult>(result);
        }

        [Fact (Skip = "Can't add data to context")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        {
            var controller = new AdminsController(_context);
            Trainer mockTrainer = new()
            {
                TrainerId = 1,
                TrainerFirstName = "Luis",
                TrainerLastName = "Carvalho",
                Gym = null,
                UserAccountModel = new UserAccountModel { UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true },
                Clients = null,
            };

            string? trainerId = mockTrainer.UserAccountModel.Id;

             _context.Trainer?.Add(mockTrainer);

            var result = await controller.DeleteUserAccount(trainerId, "~/home");
        }
    }
}
