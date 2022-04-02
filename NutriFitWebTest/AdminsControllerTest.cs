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
***REMOVED***
    public class AdminsControllerTest
    ***REMOVED***
        [Fact]
        public async Task ShowAllUsers_ReturnsViewResult()
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

                var result = await controller.ShowAllUsers(null);

                Assert.IsType<ViewResult>(result);
        ***REMOVED***
    ***REMOVED***

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
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

                var result = await controller.DeleteUserAccount(null, null);

                Assert.IsType<NotFoundResult>(result);
        ***REMOVED***
    ***REMOVED***
***REMOVED***
***REMOVED***
