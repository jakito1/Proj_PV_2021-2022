using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class GetUsersLists : IGetUsersLists
    {
        private readonly ApplicationDbContext _context;
        public GetUsersLists(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn)
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

        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn)
        {
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.Id == loggedIn select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.RowVersion).Select(a => a.UserAccountModel);
        }

        public string GetTrainerGym(string loggedIn)
        {
            IQueryable<Gym>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.Id == loggedIn select a.Gym;
            IQueryable<string>? trainerGym = from a in _context.Gym where a.GymId == loggedInTrainer.FirstOrDefault().GymId select a.GymName;
            return trainerGym.FirstOrDefault();
        }

    }
}
