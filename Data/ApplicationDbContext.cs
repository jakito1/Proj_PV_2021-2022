using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data
***REMOVED***
    public class ApplicationDbContext : IdentityDbContext<UserAccount>
    ***REMOVED***
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        ***REMOVED***
    ***REMOVED***
***REMOVED***
    //this is another comment
***REMOVED***