using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IGetUsersForGym
    {
        IEnumerable<UserAccountModel> GetUsers(string userType, string loggedIn);
    }
}
