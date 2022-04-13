using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class Statistics : IStatistics
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public Statistics(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string>? role = from a in _context.Roles where a.Name == userType select a.Id;
            if (userType == "client")
            ***REMOVED***
                return _context.Client.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).OrderByDescending(a => a.DateAddedToGym).Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "trainer")
            ***REMOVED***
                return _context.Trainer.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "nutritionist")
            ***REMOVED***
                return _context.Nutritionist.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED***
            return null;
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.Id == loggedIn select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);
    ***REMOVED***
        public IEnumerable<UserAccountModel> GetUsersForNutritionist(string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.Id == loggedIn select a.NutritionistId;
            return _context.Client.Where(a => a.Nutritionist.NutritionistId == loggedInNutritionist.FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);
    ***REMOVED***

        public async Task<string> GetUserGym(string loggedIn)
        ***REMOVED***
            UserAccountModel user = await _userManager.FindByNameAsync(loggedIn);
            Gym? gym = null;
            if (user is not null)
            ***REMOVED***
                Client client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);
                Trainer trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);
                Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel == user);

                if (client is not null && client.Gym is not null)
                ***REMOVED***
                    gym = client.Gym;
            ***REMOVED***
                else if (trainer is not null && trainer.Gym is not null)
                ***REMOVED***
                    gym = trainer.Gym;
            ***REMOVED***
                else if (nutritionist is not null && nutritionist.Gym is not null)
                ***REMOVED***
                    gym = nutritionist.Gym;
            ***REMOVED***
        ***REMOVED***
            return (gym is null || string.IsNullOrEmpty(gym.GymName)) ? "" : gym.GymName;
    ***REMOVED***

        public async Task<double> GetClientBMI(string? loggedIn)
        ***REMOVED***
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
            ***REMOVED***
                return (double)(client.Weight / (client.Height * client.Height));
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgBMI(string? loggedIn)
        ***REMOVED***
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return AvgBMI(trainer.Clients);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return AvgBMI(nutritionist.Clients);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return AvgBMI(gym.Clients);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgHeight(string? loggedIn)
        ***REMOVED***
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return AvgHeight(trainer.Clients);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return AvgHeight(nutritionist.Clients);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return AvgHeight(gym.Clients);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgWeight(string? loggedIn)
        ***REMOVED***
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);
            Gym gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == loggedIn);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return AvgWeight(trainer.Clients);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return AvgWeight(nutritionist.Clients);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return AvgWeight(gym.Clients);
        ***REMOVED***
            return 0;
    ***REMOVED***

        private static double AvgBMI(List<Client>? clients)
        ***REMOVED***
            double avgBMI = 0;
            if (clients is not null && clients.Any())
            ***REMOVED***
                foreach (Client client in clients)
                ***REMOVED***
                    if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
                    ***REMOVED***
                        avgBMI += (double)(client.Weight / (client.Height * client.Height));
                ***REMOVED***
            ***REMOVED***
                avgBMI /= clients.Count;
        ***REMOVED***
            return avgBMI;
    ***REMOVED***
        private static double AvgHeight(List<Client>? clients)
        ***REMOVED***
            double avgHeight = 0;
            if (clients is not null && clients.Any())
            ***REMOVED***
                foreach (Client client in clients)
                ***REMOVED***
                    if (client.Height is not null && client.Height > 0)
                    ***REMOVED***
                        avgHeight += (double)client.Height;
                ***REMOVED***
            ***REMOVED***
                avgHeight /= clients.Count;
        ***REMOVED***
            return avgHeight;
    ***REMOVED***
        private static double AvgWeight(List<Client>? clients)
        ***REMOVED***
            double avgWeight = 0;
            if (clients is not null && clients.Any())
            ***REMOVED***
                foreach (Client client in clients)
                ***REMOVED***
                    if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
                    ***REMOVED***
                        avgWeight += (double)client.Weight;
                ***REMOVED***
            ***REMOVED***
                avgWeight /= clients.Count;
        ***REMOVED***
            return avgWeight;
    ***REMOVED***


***REMOVED***
***REMOVED***
