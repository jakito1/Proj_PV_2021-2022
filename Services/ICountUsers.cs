using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface ICountUsers
    {
        IEnumerable<UserAccountModel> UserCount(string userType, string loggedIn);
    }
}
