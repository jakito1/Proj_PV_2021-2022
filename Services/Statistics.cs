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

        public IEnumerable<UserAccountModel>? GetUsersForGym(string? userType, string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.UserName == userName select a.GymId;
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

        public IEnumerable<UserAccountModel>? GetUsersForTrainer(string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.UserName == userName select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);
    ***REMOVED***
        public IEnumerable<UserAccountModel>? GetUsersForNutritionist(string? userName)
        ***REMOVED***
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.UserName == userName select a.NutritionistId;
            return _context.Client.Where(a => a.Nutritionist.NutritionistId == loggedInNutritionist.FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);
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
                return (double)(client.Weight / (tempHeight * tempHeight));
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientLeanMass(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.FatMass is not null)
            ***REMOVED***
                return (double)client.FatMass;
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<double> GetClientFatMass(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.LeanMass is not null)
            ***REMOVED***
                return (double)client.LeanMass;
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

        public async Task<double> GetClientsAvgHeight(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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

        public async Task<double> GetClientsAvgWeight(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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

        public async Task<double> GetClientsAvgLeanMass(string? userName)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            ***REMOVED***
                return AvgLeanMass(trainer.Clients);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return AvgLeanMass(nutritionist.Clients);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return AvgLeanMass(gym.Clients);
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
                return AvgFatMass(trainer.Clients);
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            ***REMOVED***
                return AvgFatMass(nutritionist.Clients);
        ***REMOVED***
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            ***REMOVED***
                return AvgFatMass(gym.Clients);
        ***REMOVED***
            return 0;
    ***REMOVED***

        public async Task<string> ClientBMICompared(string? userName)
        ***REMOVED***
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null)
            ***REMOVED***
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                double gymBMIAvg = await GetClientsAvgBMI(gym.UserAccountModel.UserName);
                double clientCurrentBMI = await GetClientBMI(userName);
                double BMIDiff = gymBMIAvg - clientCurrentBMI;
                string BMIDiffString = string.Format("***REMOVED***0:0.00***REMOVED***", Math.Abs(BMIDiff));
                if (BMIDiff > 0)
                ***REMOVED***
                    return $"O seu IMC está ***REMOVED***BMIDiffString***REMOVED*** pontos abaixo da média do ginásio.";
            ***REMOVED***
                else if (BMIDiff < 0)
                ***REMOVED***
                    return $"O seu IMC está ***REMOVED***BMIDiffString***REMOVED*** pontos acima da média do ginásio.";
            ***REMOVED***
                else if (BMIDiff == 0)
                ***REMOVED***
                    return "O seu IMC está na média do ginásio.";
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

                double gymLeanAvgMass = await GetClientsAvgLeanMass(gym.UserAccountModel.UserName);
                double LeanMassDiff = gymLeanAvgMass - (double)client.LeanMass;
                string LeanMassDiffString = string.Format("***REMOVED***0:0.00***REMOVED***", Math.Abs(LeanMassDiff));
                if (LeanMassDiff > 0)
                ***REMOVED***
                    return $"A sua massa magra está ***REMOVED***LeanMassDiffString***REMOVED*** pontos percentuais abaixo da média do ginásio.";
            ***REMOVED***
                else if (LeanMassDiff < 0)
                ***REMOVED***
                    return $"A sua massa magra está ***REMOVED***LeanMassDiffString***REMOVED*** pontos percentuais acima da média do ginásio.";
            ***REMOVED***
                else if (LeanMassDiff == 0)
                ***REMOVED***
                    return "A sua massa magra está na média do ginásio.";
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

                double gymFatAvgMass = await GetClientsAvgFatMass(gym.UserAccountModel.UserName);
                double FatMassDiff = gymFatAvgMass - (double)client.FatMass;
                string FatMassDiffString = string.Format("***REMOVED***0:0.00***REMOVED***", Math.Abs(FatMassDiff));
                if (FatMassDiff > 0)
                ***REMOVED***
                    return $"A sua massa gorda está ***REMOVED***FatMassDiffString***REMOVED*** pontos percentuais abaixo da média do ginásio.";
            ***REMOVED***
                else if (FatMassDiff < 0)
                ***REMOVED***
                    return $"A sua massa gorda está ***REMOVED***FatMassDiffString***REMOVED*** pontos percentuais acima da média do ginásio.";
            ***REMOVED***
                else if (FatMassDiff == 0)
                ***REMOVED***
                    return "A sua massa gorda está na média do ginásio.";
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
