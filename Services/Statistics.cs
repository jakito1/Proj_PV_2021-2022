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

        public IEnumerable<UserAccountModel> GetUsersForGym(string? userType, string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.UserName == userName select a.GymId;
            IQueryable<string>? role = from a in _context.Roles where a.Name == userType select a.Id;
            IQueryable<UserAccountModel>? users = null;
            if (userType == "client")
            ***REMOVED***
                users = _context.Client.Where(a => a.Gym != null && a.Gym.GymId == loggedInGym.FirstOrDefault())
                    .OrderByDescending(a => a.DateAddedToGym).Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "trainer")
            ***REMOVED***
                users = _context.Trainer.Where(a => a.Gym != null && a.Gym.GymId == loggedInGym.FirstOrDefault())
                    .Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "nutritionist")
            ***REMOVED***
                users = _context.Nutritionist.Where(a => a.Gym != null && a.Gym.GymId == loggedInGym
                .FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED***

            if (users is not null)
            ***REMOVED***
                return users;
        ***REMOVED***
            return Enumerable.Empty<UserAccountModel>();
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsersForTrainer(string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.UserName == userName select a.TrainerId;
            IQueryable<UserAccountModel>? users = _context.Client.Where(a => a.Trainer != null && a.Trainer.TrainerId == loggedInTrainer
            .FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);

            if (users is not null)
            ***REMOVED***
                return users;
        ***REMOVED***
            return Enumerable.Empty<UserAccountModel>();
    ***REMOVED***
        public IEnumerable<UserAccountModel> GetUsersForNutritionist(string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.UserName == userName select a.NutritionistId;
            IQueryable<UserAccountModel>? users = _context.Client.Where(a => a.Nutritionist != null && a.Nutritionist.NutritionistId == loggedInNutritionist
            .FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);

            if (users is not null)
            ***REMOVED***
                return users;
        ***REMOVED***
            return Enumerable.Empty<UserAccountModel>();
    ***REMOVED***

        public async Task<string> GetUserGym(string? userName)
        ***REMOVED***
            Gym? gym = null;
            Client? client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Trainer? trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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
            return (gym is null || string.IsNullOrEmpty(gym.GymName)) ? "" : gym.GymName;
    ***REMOVED***

        public async Task<double> GetClientBMI(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
            ***REMOVED***
                double tempHeight = (double)client.Height / 100;
                return Math.Round((double)(client.Weight / (tempHeight * tempHeight)), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientLeanMass(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.LeanMass is not null)
            ***REMOVED***
                return (double)client.LeanMass;
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientFatMass(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.FatMass is not null)
            ***REMOVED***
                return (double)client.FatMass;
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgBMI(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgBMI(trainer.Clients), 2);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgBMI(nutritionist.Clients), 2);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgBMI(gym.Clients), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgHeight(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgHeight(trainer.Clients), 2);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgHeight(nutritionist.Clients), 2);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgHeight(gym.Clients), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgWeight(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgWeight(trainer.Clients), 2);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgWeight(nutritionist.Clients), 2);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgWeight(gym.Clients), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgLeanMass(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgLeanMass(trainer.Clients), 2);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgLeanMass(nutritionist.Clients), 2);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgLeanMass(gym.Clients), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientsAvgFatMass(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgFatMass(trainer.Clients), 2);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgFatMass(nutritionist.Clients), 2);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return Math.Round(AvgFatMass(gym.Clients), 2);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<string> ClientBMICompared(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null)
            ***REMOVED***
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                if (gym is not null)
                ***REMOVED***
                    double gymBMIAvg = await GetClientsAvgBMI(gym.UserAccountModel.UserName);
                    double clientCurrentBMI = await GetClientBMI(userName);
                    double BMIDiff = gymBMIAvg - clientCurrentBMI;
                    if (BMIDiff > 0)
                    ***REMOVED***
                        return $"O seu IMC está ***REMOVED***Math.Round(BMIDiff, 2)***REMOVED*** pontos abaixo da média do ginásio.";
                ***REMOVED***
                    else if (BMIDiff < 0)
                    ***REMOVED***
                        return $"O seu IMC está ***REMOVED***Math.Round(Math.Abs(BMIDiff), 2)***REMOVED*** pontos acima da média do ginásio.";
                ***REMOVED***
                    else if (BMIDiff == 0)
                    ***REMOVED***
                        return "O seu IMC está na média do ginásio.";
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***
            return string.Empty;
    ***REMOVED***

        public async Task<string> ClientLeanMassCompared(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null && client.LeanMass is not null)
            ***REMOVED***
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                if (gym is not null)
                ***REMOVED***
                    double gymLeanAvgMass = await GetClientsAvgLeanMass(gym.UserAccountModel.UserName);
                    double leanMassDiff = gymLeanAvgMass - (double)client.LeanMass;
                    if (leanMassDiff > 0)
                    ***REMOVED***
                        return $"A sua massa magra está ***REMOVED***Math.Round(leanMassDiff, 2)***REMOVED*** pontos percentuais abaixo da média do ginásio.";
                ***REMOVED***
                    else if (leanMassDiff < 0)
                    ***REMOVED***
                        return $"A sua massa magra está ***REMOVED***Math.Round(Math.Abs(leanMassDiff), 2)***REMOVED*** pontos percentuais acima da média do ginásio.";
                ***REMOVED***
                    else if (leanMassDiff == 0)
                    ***REMOVED***
                        return "A sua massa magra está na média do ginásio.";
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***
            return string.Empty;
    ***REMOVED***

        public async Task<string> ClientFatMassCompared(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null && client.FatMass is not null)
            ***REMOVED***
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                if (gym is not null)
                ***REMOVED***
                    double gymFatAvgMass = await GetClientsAvgFatMass(gym.UserAccountModel.UserName);
                    double fatMassDiff = gymFatAvgMass - (double)client.FatMass;
                    if (fatMassDiff > 0)
                    ***REMOVED***
                        return $"A sua massa gorda está ***REMOVED***Math.Round(fatMassDiff, 2)***REMOVED*** pontos percentuais abaixo da média do ginásio.";
                ***REMOVED***
                    else if (fatMassDiff < 0)
                    ***REMOVED***
                        return $"A sua massa gorda está ***REMOVED***Math.Round(Math.Abs(fatMassDiff), 2)***REMOVED*** pontos percentuais acima da média do ginásio.";
                ***REMOVED***
                    else if (fatMassDiff == 0)
                    ***REMOVED***
                        return "A sua massa gorda está na média do ginásio.";
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***
            return string.Empty;
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
                        double tempHeight = (double)client.Height / 100;
                        avgBMI += (double)(client.Weight / (tempHeight * tempHeight));
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

        private static double AvgLeanMass(List<Client>? clients)
        ***REMOVED***
            double avgLeanMass = 0;
            if (clients is not null && clients.Any())
            ***REMOVED***
                foreach (Client client in clients)
                ***REMOVED***
                    if (client is not null && client.LeanMass is not null && client.LeanMass > 0)
                    ***REMOVED***
                        avgLeanMass += (double)client.LeanMass;
                ***REMOVED***
            ***REMOVED***
                avgLeanMass /= clients.Count;
        ***REMOVED***
            return avgLeanMass;
    ***REMOVED***
        private static double AvgFatMass(List<Client>? clients)
        ***REMOVED***
            double avgFatMass = 0;
            if (clients is not null && clients.Any())
            ***REMOVED***
                foreach (Client client in clients)
                ***REMOVED***
                    if (client is not null && client.FatMass is not null && client.FatMass > 0)
                    ***REMOVED***
                        avgFatMass += (double)client.FatMass;
                ***REMOVED***
            ***REMOVED***
                avgFatMass /= clients.Count;
        ***REMOVED***
            return avgFatMass;
    ***REMOVED***
***REMOVED***
***REMOVED***
