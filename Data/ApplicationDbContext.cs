using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
***REMOVED***
    /// <summary>
    /// ApplicationDbContext class, derives from IdentityDbContext using the UserAccountModel as the model
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<UserAccountModel>
    ***REMOVED***
        /// <summary>
        /// Build the ApplicationDbContext.
        /// </summary>
        /// <param name="options">Options to be used by a DbContext</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        ***REMOVED***
    ***REMOVED***

        public DbSet<NutriFitWeb.Models.Client>? Client ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Nutritionist>? Nutritionist ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Trainer>? Trainer ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Gym>? Gym ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.TrainingPlan>? TrainingPlan ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Exercise>? Exercise ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.NutritionPlan>? NutritionPlan ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Meal>? Meal ***REMOVED*** get; set; ***REMOVED***
        public DbSet<NutriFitWeb.Models.Picture>? Picture ***REMOVED*** get; set; ***REMOVED***

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        ***REMOVED***
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Gym>().HasMany(a => a.Clients).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Gym>().HasMany(a => a.Nutritionists).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Gym>().HasMany(a => a.Trainers).WithOne(a => a.Gym)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Trainer>().HasMany(a => a.Clients).WithOne(a => a.Trainer)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Nutritionist>().HasMany(a => a.Clients).WithOne(a => a.Nutritionist)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Trainer>().HasMany(a => a.TrainingPlans).WithOne(a => a.Trainer)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Nutritionist>().HasMany(a => a.NutritionPlans).WithOne(a => a.Nutritionist)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TrainingPlan>().HasMany(a => a.Exercises).WithOne(a => a.TrainingPlan)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Exercise>().HasMany(a => a.Pictures).WithOne(a => a.Exercise)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>().HasMany(a => a.TrainingPlans).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NutritionPlan>().HasMany(a => a.Meals).WithOne(a => a.NutritionPlan)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Meal>().HasMany(a => a.Pictures).WithOne(a => a.Meal)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>().HasMany(a => a.NutritionPlans).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);

    ***REMOVED***
***REMOVED***
***REMOVED***