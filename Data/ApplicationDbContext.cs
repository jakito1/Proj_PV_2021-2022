#nullable disable
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

        public DbSet<NutriFitWeb.Models.Client> Client { get; set; }
        public DbSet<NutriFitWeb.Models.Nutritionist> Nutritionist { get; set; }
        public DbSet<NutriFitWeb.Models.Trainer> Trainer { get; set; }
        public DbSet<NutriFitWeb.Models.Gym> Gym { get; set; }
        public DbSet<NutriFitWeb.Models.TrainingPlan> TrainingPlan { get; set; }
        public DbSet<NutriFitWeb.Models.Exercise> Exercise { get; set; }
        public DbSet<NutriFitWeb.Models.NutritionPlan> NutritionPlan { get; set; }
        public DbSet<NutriFitWeb.Models.Meal> Meal { get; set; }
        public DbSet<NutriFitWeb.Models.Photo> Photos { get; set; }
        public DbSet<NutriFitWeb.Models.TrainingPlanNewRequest> TrainingPlanNewRequests { get; set; }
        public DbSet<NutriFitWeb.Models.TrainingPlanEditRequest> TrainingPlanEditRequests { get; set; }
        public DbSet<NutriFitWeb.Models.NutritionPlanNewRequest> NutritionPlanNewRequests { get; set; }
        public DbSet<NutriFitWeb.Models.NutritionPlanEditRequest> NutritionPlanEditRequests { get; set; }
        public DbSet<NutriFitWeb.Models.Notification> Notifications { get; set; }

        public DbSet<NutriFitWeb.Models.Machine> Machines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            modelBuilder.Entity<Client>().HasMany(a => a.TrainingPlans).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NutritionPlan>().HasMany(a => a.Meals).WithOne(a => a.NutritionPlan)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>().HasMany(a => a.NutritionPlans).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>().HasMany(a => a.TrainingPlanRequests).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Client>().HasMany(a => a.NutritionPlanRequests).WithOne(a => a.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Machine>().HasMany(a => a.MachineExercises).WithOne(a => a.Machine)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Gym>().HasMany(a => a.Machines).WithOne(a => a.MachineGym)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}