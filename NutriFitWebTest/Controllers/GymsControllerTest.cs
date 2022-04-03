using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using Xunit;

namespace NutriFitWebTest
{
    public class GymsControllerTest
    {
        [Fact]
        public void Index_ReturnsViewResult()
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
                var controller = new GymsController(context);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
    }
}
