using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
{
    public class AdminsControllerTest
    {
        [Fact]
        public async Task ShowAllUsers_ReturnsViewResult()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new AdminsController(context);

                var result = await controller.ShowAllUsers(null);

                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new AdminsController(context);

                var result = await controller.DeleteUserAccount(null, null);

                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
