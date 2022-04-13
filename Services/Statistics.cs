using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class Statistics : IStatistics
    {
        private readonly ApplicationDbContext _context;
        public Statistics(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn)
        {
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string>? role = from a in _context.Roles where a.Name == userType select a.Id;
            if (userType == "client")
            {
                return _context.Client.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).OrderByDescending(a => a.DateAddedToGym).Select(a => a.UserAccountModel);
            }
            else if (userType == "trainer")
            {
                return _context.Trainer.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
            }
            else if (userType == "nutritionist")
            {
                return _context.Nutritionist.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
            }
            return null;
        }

        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn)
        {
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.Id == loggedIn select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);
        }
        public IEnumerable<UserAccountModel> GetUsersForNutritionist(string loggedIn)
        {
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.Id == loggedIn select a.NutritionistId;
            return _context.Client.Where(a => a.Nutritionist.NutritionistId == loggedInNutritionist.FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);
        }

        public async Task<string> GetTrainerGym(string loggedIn)
        {
            Gym? gym = await _context.Trainer.Where(a => a.UserAccountModel.Id == loggedIn).Select(a => a.Gym).FirstOrDefaultAsync();
            return (gym is null) ? "" : gym.GymName;
        }

        public async Task<double> GetClientBMI(string? loggedIn)
        {
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
            {
                return (double)(client.Weight / (client.Height * client.Height));
            }
            return 0;
        }

        public async Task<double> GetClientsAvgBMI(string? loggedIn)
        {
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            {
                return AvgBMI(trainer.Clients);
            }
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            {
                return AvgBMI(nutritionist.Clients);
            }
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            {
                return AvgBMI(gym.Clients);
            }
            return 0;
        }

        private static double AvgBMI(List<Client>? clients)
        {
            double avgBMI = 0;
            if (clients is not null && clients.Any())
            {
                foreach (Client client in clients)
                {
                    if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
                    {
                        avgBMI += (double)(client.Weight / (client.Height * client.Height));
                    }
                }
                avgBMI /= clients.Count;
            }
            return avgBMI;
        }

    }
}
