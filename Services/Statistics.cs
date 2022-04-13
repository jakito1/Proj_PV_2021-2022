using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class Statistics : IStatistics
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public Statistics(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<string> GetUserGym(string loggedIn)
        {
            UserAccountModel user = await _userManager.FindByNameAsync(loggedIn);
            Gym? gym = null;
            if (user is not null)
            {
                Client client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);
                Trainer trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);
                Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);

                if (client is not null && client.Gym is not null)
                {
                    gym = client.Gym;
                }
                else if (trainer is not null && trainer.Gym is not null)
                {
                    gym = trainer.Gym;
                }
                else if (nutritionist is not null && nutritionist.Gym is not null)
                {
                    gym = nutritionist.Gym;
                }
            }
            return (gym is null || string.IsNullOrEmpty(gym.GymName)) ? "" : gym.GymName;
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

        public async Task<double> GetClientsAvgHeight(string? loggedIn)
        {
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            {
                return AvgHeight(trainer.Clients);
            }
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            {
                return AvgHeight(nutritionist.Clients);
            }
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            {
                return AvgHeight(gym.Clients);
            }
            return 0;
        }

        public async Task<double> GetClientsAvgWeight(string? loggedIn)
        {
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            {
                return AvgWeight(trainer.Clients);
            }
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            {
                return AvgWeight(nutritionist.Clients);
            }
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            {
                return AvgWeight(gym.Clients);
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
        private static double AvgHeight(List<Client>? clients)
        {
            double avgHeight = 0;
            if (clients is not null && clients.Any())
            {
                foreach (Client client in clients)
                {
                    if (client.Height is not null && client.Height > 0)
                    {
                        avgHeight += (double)client.Height;
                    }
                }
                avgHeight /= clients.Count;
            }
            return avgHeight;
        }
        private static double AvgWeight(List<Client>? clients)
        {
            double avgWeight = 0;
            if (clients is not null && clients.Any())
            {
                foreach (Client client in clients)
                {
                    if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
                    {
                        avgWeight += (double)client.Weight;
                    }
                }
                avgWeight /= clients.Count;
            }
            return avgWeight;
        }


    }
}
