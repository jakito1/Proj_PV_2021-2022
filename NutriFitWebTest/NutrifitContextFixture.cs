using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

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
}

