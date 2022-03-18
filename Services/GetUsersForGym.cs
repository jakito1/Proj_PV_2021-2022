using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class GetUsersForGym : IGetUsersForGym
    {
        private readonly ApplicationDbContext _context;
        public GetUsersForGym(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAccountModel> GetUsers(string userType, string loggedIn)
        {
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string> ? role = from a in _context.Roles where a.Name == userType select a.Id;
            if (userType == "client")
            {
                return _context.Client.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).OrderByDescending(a => a.RowVersion).Select(a => a.UserAccountModel);
            } else if (userType == "trainer")
            {
                return _context.Trainer.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
            } else if (userType == "nutritionist")
            {
                return _context.Nutritionist.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);               
            }
            return null;
        }

    }
}
