using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class Statistics : IStatistics
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public Statistics(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
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

        public async Task<string> GetTrainerGym(string loggedIn)
        ***REMOVED***
            Gym? gym = await _context.Trainer.Where(a => a.UserAccountModel.Id == loggedIn).Select(a => a.Gym).FirstOrDefaultAsync();
            return (gym is null) ? "" : gym.GymName;
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

***REMOVED***
***REMOVED***
