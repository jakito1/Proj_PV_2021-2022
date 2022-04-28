using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

namespace NutriFitWeb.Services
***REMOVED***
    public class IsUserInRoleByUserId : IIsUserInRoleByUserId
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public IsUserInRoleByUserId(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public async Task<bool> IsUserInRoleByUserIdAsync(string? userId, string? userType)
        ***REMOVED***
            IdentityRole? role = await _context.Roles.FirstOrDefaultAsync(a => a.Name == userType);
            if (role is not null)
            ***REMOVED***
                return _context.UserRoles.Where(a => a.RoleId == role.Id && a.UserId == userId).Any();
        ***REMOVED***
            return false;
    ***REMOVED***
***REMOVED***
***REMOVED***
