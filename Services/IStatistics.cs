using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IStatistics
    {
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForNutritionist(string loggedIn);
        Task<string> GetUserGym(string loggedIn);
        Task<double> GetClientBMI(string? loggedIn);
        Task<double> GetClientsAvgBMI(string? loggedIn);
        Task<double> GetClientsAvgHeight(string? loggedIn);
        Task<double> GetClientsAvgWeight(string? loggedIn);
        Task<double> GetClientsAvgLeanMass(string? loggedIn);
        Task<double> GetClientsAvgFatMass(string? loggedIn);
    }
}
