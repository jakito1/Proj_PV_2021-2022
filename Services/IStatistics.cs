using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IStatistics
    ***REMOVED***
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForNutritionist(string loggedIn);
        Task<string> GetTrainerGym(string loggedIn);
        Task<double> GetClientBMI(string? loggedIn);
        Task<double> GetClientsAvgBMI(string? loggedIn)
***REMOVED***
***REMOVED***
