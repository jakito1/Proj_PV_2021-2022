using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

namespace NutriFitWeb.Services
{
    public class IsUserInRoleById : IIsUserInRoleByUserId
    {
        private readonly ApplicationDbContext _context;
        public IsUserInRoleById(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserInRoleByUserIdAsync(string? userId, string? userType)
        {
            IdentityRole? role = await _context.Roles.FirstOrDefaultAsync(a => a.Name == userType);
            return _context.UserRoles.Where(a => a.RoleId == role.Id && a.UserId == userId).Any();
        }
    }
}
