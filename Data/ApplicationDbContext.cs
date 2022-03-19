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
        public DbSet<NutriFitWeb.Models.Nutritionist>? Nutritionist { get; set; }
        public DbSet<NutriFitWeb.Models.Trainer>? Trainer { get; set; }
        public DbSet<NutriFitWeb.Models.Gym>? Gym { get; set; }
        public DbSet<NutriFitWeb.Models.TrainingPlan>? TrainingPlan { get; set; }
        public DbSet<NutriFitWeb.Models.Exercise>? Exercise { get; set; }
        public DbSet<NutriFitWeb.Models.Picture>? Picture { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Gym>().HasMany(a => a.Clients).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Gym>().HasMany(a => a.Nutritionists).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Gym>().HasMany(a => a.Trainers).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity
        }
    }
}