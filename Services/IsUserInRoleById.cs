using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

namespace NutriFitWeb.Services
***REMOVED***
    public class IsUserInRoleById : IIsUserInRoleByUserId
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public IsUserInRoleById(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public async Task<bool> IsUserInRoleByUserIdAsync(string? userId, string? userType)
        ***REMOVED***
            IdentityRole? role = await _context.Roles.FirstOrDefaultAsync(a => a.Name == userType);
            return _context.UserRoles.Where(a => a.RoleId == role.Id && a.UserId == userId).Any();
    ***REMOVED***
***REMOVED***
***REMOVED***
