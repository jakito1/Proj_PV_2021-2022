using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class GymsControllerTest
    ***REMOVED***
        [Fact]
        public void Index_ReturnsViewResult()
        ***REMOVED***
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            ***REMOVED***
                context.Database.EnsureCreated();
                context.SaveChanges();
        ***REMOVED***

            using (var context = new ApplicationDbContext(options))
            ***REMOVED***
                var controller = new GymsController(context);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
        ***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
