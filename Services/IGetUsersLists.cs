using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    /// <summary>
    /// IGetUsersLists interface
    /// </summary>
    public interface IGetUsersLists
    {
        /// <summary>
        /// Get all the users from a Gym by user type and if logged in.
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="loggedIn"></param>
        /// <returns>An enumerable UserAccountModel</returns>
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        /// <summary>
        /// Get all the users from a Trainer.
        /// </summary>
        /// <param name="loggedIn"></param>
        /// <returns>An enumerable UserAccountModel</returns>
        IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        /// <summary>
        /// Get the Gym associated with a Trainer.
        /// </summary>
        /// <param name="loggedIn"></param>
        /// <returns>The name of the gym that's associated with the trainer</returns>
        string GetTrainerGym(string loggedIn);
    }
}
