using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IIsUserInRoleByUserId
    {
        Task<bool> IsUserInRoleByUserIdAsync(string? userId, string? userType);
    }
}