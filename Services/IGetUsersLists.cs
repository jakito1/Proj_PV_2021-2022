using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IGetUsersLists
    ***REMOVED***
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        string GetTrainerGym(string loggedIn);
***REMOVED***
***REMOVED***
