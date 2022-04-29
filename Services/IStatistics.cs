using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IStatistics
    ***REMOVED***
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string userName);
        IEnumerable<UserAccountModel> GetUsersForTrainer(string userName);
        IEnumerable<UserAccountModel> GetUsersForNutritionist(string userName);
        Task<string> GetUserGym(string userName);
        Task<double> GetClientBMI(string? userName);
        Task<double> GetClientLeanMass(string? userName);
        Task<double> GetClientFatMass(string? userName);
        Task<double> GetClientsAvgBMI(string? userName);
        Task<double> GetClientsAvgHeight(string? userName);
        Task<double> GetClientsAvgWeight(string? userName);
        Task<double> GetClientsAvgLeanMass(string? userName);
        Task<double> GetClientsAvgFatMass(string? userName);
        Task<string> ClientBMICompared(string? userName);
        Task<string> ClientLeanMassCompared(string? userName);
        Task<string> ClientFatMassCompared(string? userName);
***REMOVED***
***REMOVED***
