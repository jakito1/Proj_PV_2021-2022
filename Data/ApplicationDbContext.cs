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
        public DbSet<NutriFitWeb.Models.Gym>? Gym ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
    //this is another comment
***REMOVED***