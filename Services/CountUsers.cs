using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class CountUsers : ICountUsers
    {
        private readonly ApplicationDbContext _context;
        public CountUsers(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAccountModel> UserCount(string userType, string loggedIn)
        {
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccount.Id == loggedIn select a.GymId;
            IQueryable<string> ? role = from a in _context.Roles where a.Name == userType select a.Id;
            IQueryable<string>? usersIDs = from a in _context.UserRoles where a.RoleId == role.First() select a.UserId;
            IQueryable<UserAccountModel>? returnQuery = from a in _context.Client where a.Gym.GymId == loggedInGym.First() select a.UserAccountModel;

            return returnQuery.ToList();
        }

    }
}
