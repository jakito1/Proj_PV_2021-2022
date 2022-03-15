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
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string> ? role = from a in _context.Roles where a.Name == userType select a.Id;
            IQueryable<UserAccountModel>? returnQuery = null;
            if (userType == "client")
            {
                returnQuery = from a in _context.Client where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
            } else if (userType == "trainer")
            {
                returnQuery = from a in _context.Trainer where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
            } else if (userType == "nutritionist")
            {
                returnQuery = from a in _context.Nutritionist where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
            }


            return returnQuery.ToList();
        }

    }
}
