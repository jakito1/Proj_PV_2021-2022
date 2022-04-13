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

        public IEnumerable<UserAccountModel>? GetUsersForGym(string? userType, string? userName)
        {
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.UserName == userName select a.GymId;
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

        public IEnumerable<UserAccountModel>? GetUsersForTrainer(string? userName)
        {
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.UserName == userName select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);
        }
        public IEnumerable<UserAccountModel>? GetUsersForNutritionist(string? userName)
        {
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.UserName == userName select a.NutritionistId;
            return _context.Client.Where(a => a.Nutritionist.NutritionistId == loggedInNutritionist.FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);
        }

        public async Task<string> GetUserGym(string? userName)
        {
            Gym? gym = null;
            Client? client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Trainer? trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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
            return (gym is null || string.IsNullOrEmpty(gym.GymName)) ? "" : gym.GymName;
        }

        public async Task<double> GetClientBMI(string? userName)
        {
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Weight is not null && client.Weight > 0 &&
                    client.Height is not null && client.Height > 0)
            {
                double tempHeight = (double)client.Height / 100;
                return (double)(client.Weight / (tempHeight * tempHeight));
            }
            return 0;
        }

        public async Task<double> GetClientsAvgBMI(string? userName)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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

        public async Task<double> GetClientsAvgHeight(string? userName)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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

        public async Task<double> GetClientsAvgWeight(string? userName)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

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

        public async Task<double> GetClientsAvgLeanMass(string? userName)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            {
                return AvgLeanMass(trainer.Clients);
            }
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            {
                return AvgLeanMass(nutritionist.Clients);
            }
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            {
                return AvgLeanMass(gym.Clients);
            }
            return 0;
        }

        public async Task<double> GetClientsAvgFatMass(string? userName)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            Gym? gym = await _context.Gym.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);

            if (trainer is not null && trainer.Clients is not null && trainer.Clients.Any())
            {
                return AvgFatMass(trainer.Clients);
            }
            if (nutritionist is not null && nutritionist.Clients is not null && nutritionist.Clients.Any())
            {
                return AvgFatMass(nutritionist.Clients);
            }
            if (gym is not null && gym.Clients is not null && gym.Clients.Any())
            {
                return AvgFatMass(gym.Clients);
            }
            return 0;
        }

        public async Task<string> ClientBMICompared(string? userName)
        {
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null)
            {
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                double gymBMIAvg = await GetClientsAvgBMI(gym.UserAccountModel.UserName);
                double clientCurrentBMI = await GetClientBMI(userName);
                double BMIDiff = gymBMIAvg - clientCurrentBMI;
                string BMIDiffString = string.Format("{0:0.00}", Math.Abs(BMIDiff));
                if (BMIDiff > 0)
                {
                    return $"O seu IMC está {BMIDiffString} pontos abaixo da média do ginásio.";
                }
                else if (BMIDiff < 0)
                {
                    return $"O seu IMC está {BMIDiffString} pontos acima da média do ginásio.";
                }
                else if (BMIDiff == 0)
                {
                    return "O seu IMC está na média do ginásio.";
                }
            }
            return string.Empty;
        }

        public async Task<string> ClientLeanMassCompared(string? userName)
        {
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null && client.LeanMass is not null)
            {
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                double gymLeanAvgMass = await GetClientsAvgLeanMass(gym.UserAccountModel.UserName);
                double LeanMassDiff = gymLeanAvgMass - (double)client.LeanMass;
                string LeanMassDiffString = string.Format("{0:0.00}", Math.Abs(LeanMassDiff));
                if (LeanMassDiff > 0)
                {
                    return $"A sua massa magra está {LeanMassDiffString} pontos percentuais abaixo da média do ginásio.";
                }
                else if (LeanMassDiff < 0)
                {
                    return $"A sua massa magra está {LeanMassDiffString} pontos percentuais acima da média do ginásio.";
                }
                else if (LeanMassDiff == 0)
                {
                    return "A sua massa magra está na média do ginásio.";
                }
            }
            return string.Empty;
        }

        public async Task<string> ClientFatMassCompared(string? userName)
        {
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.UserName == userName);
            if (client is not null && client.Gym is not null && client.FatMass is not null)
            {
                Gym? gym = await _context.Gym.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.GymId == client.Gym.GymId);

                double gymFatAvgMass = await GetClientsAvgFatMass(gym.UserAccountModel.UserName);
                double FatMassDiff = gymFatAvgMass - (double)client.FatMass;
                string FatMassDiffString = string.Format("{0:0.00}", Math.Abs(FatMassDiff));
                if (FatMassDiff > 0)
                {
                    return $"A sua massa gorda está {FatMassDiffString} pontos percentuais abaixo da média do ginásio.";
                }
                else if (FatMassDiff < 0)
                {
                    return $"A sua massa gorda está {FatMassDiffString} pontos percentuais acima da média do ginásio.";
                }
                else if (FatMassDiff == 0)
                {
                    return "A sua massa gorda está na média do ginásio.";
                }
            }
            return string.Empty;
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
                        double tempHeight = (double)client.Height / 100;
                        avgBMI += (double)(client.Weight / (tempHeight * tempHeight));
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

        private static double AvgLeanMass(List<Client>? clients)
        {
            double avgLeanMass = 0;
            if (clients is not null && clients.Any())
            {
                foreach (Client client in clients)
                {
                    if (client is not null && client.LeanMass is not null && client.LeanMass > 0)
                    {
                        avgLeanMass += (double)client.LeanMass;
                    }
                }
                avgLeanMass /= clients.Count;
            }
            return avgLeanMass;
        }
        private static double AvgFatMass(List<Client>? clients)
        {
            double avgFatMass = 0;
            if (clients is not null && clients.Any())
            {
                foreach (Client client in clients)
                {
                    if (client is not null && client.FatMass is not null && client.FatMass > 0)
                    {
                        avgFatMass += (double)client.FatMass;
                    }
                }
                avgFatMass /= clients.Count;
            }
            return avgFatMass;
        }
    }
}
