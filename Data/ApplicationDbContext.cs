using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
{
    /// <summary>
    /// ApplicationDbContext class, derives from IdentityDbContext using the UserAccountModel as the model
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<UserAccountModel>
    {
        /// <summary>
        /// Build the ApplicationDbContext.
        /// </summary>
        /// <param name="options">Options to be used by a DbContext</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NutriFitWeb.Models.Client>? Client { get; set; }
        public DbSet<NutriFitWeb.Models.Gym>? Gym { get; set; }

        //this is another comment
    }
}