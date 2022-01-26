using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;

namespace NutriFitWeb.Data;

/// <summary>
/// NutriFitWebContext class, derives from IdentityDbContext using the UserAccountModel as the model
/// </summary>
public class NutriFitWebContext : IdentityDbContext<UserAccountModel>
{
    /// <summary>
    /// Build the NutriFitWebContext.
    /// </summary>
    /// <param name="options">Options to be used by a DbContext</param>
    public NutriFitWebContext(DbContextOptions<NutriFitWebContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
