using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IGetUsersLists
    {
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        public string GetTrainerGym(string loggedIn);
    }
}
