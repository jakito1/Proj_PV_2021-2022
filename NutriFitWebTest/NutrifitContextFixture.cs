using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

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
***REMOVED***

