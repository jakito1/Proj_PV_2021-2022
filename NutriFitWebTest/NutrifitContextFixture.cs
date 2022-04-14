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
            SqliteConnection? connection = new("Datasource=:memory:");
            connection.Open();
            DbContextOptions<ApplicationDbContext>? options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();
        }
    }
}

