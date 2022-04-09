using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using Xunit;

namespace NutriFitWebTest;

public class AdminsControllerTest
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
    ***REMOVED***

        using (var context = new ApplicationDbContext(options))
        ***REMOVED***
            var controller = new AdminsController(context);

            var result = controller.ShowAllUsers(null);

            Assert.IsType<Task<IActionResult>>(result);

            var result2 = controller.ShowAllUsers("random@email.com");
            
            Assert.IsType<Task<IActionResult>>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***