namespace NutriFitWeb.Services
{
    /// <summary>
    /// IIsUserInRoleByUserId interface
    /// </summary>
    public interface IIsUserInRoleByUserId
    {
        /// <summary>
        /// Check if the user belongs to a role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns>A boolean Task result</returns>
        Task<bool> IsUserInRoleByUserIdAsync(string? userId, string? userType);
    }
}