using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Areas.Identity.Data;

namespace NutriFitWeb.Data;

public class NutriFitWebContext : IdentityDbContext<UserAccount>
***REMOVED***
    public NutriFitWebContext(DbContextOptions<NutriFitWebContext> options)
        : base(options)
    ***REMOVED***
***REMOVED***

    protected override void OnModelCreating(ModelBuilder builder)
    ***REMOVED***
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
***REMOVED***
***REMOVED***
