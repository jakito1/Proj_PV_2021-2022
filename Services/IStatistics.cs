using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IStatistics
    {
        IEnumerable<UserAccountModel>? GetUsersForGym(string userType, string userName);
        IEnumerable<UserAccountModel>? GetUsersForTrainer(string userName);
        IEnumerable<UserAccountModel>? GetUsersForNutritionist(string userName);
        Task<string> GetUserGym(string userName);
        Task<double> GetClientBMI(string? userName);
        Task<double> GetClientsAvgBMI(string? userName);
        Task<double> GetClientsAvgHeight(string? userName);
        Task<double> GetClientsAvgWeight(string? userName);
        Task<double> GetClientsAvgLeanMass(string? userName);
        Task<double> GetClientsAvgFatMass(string? userName);
    }
}
