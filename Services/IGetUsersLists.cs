using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IGetUsersLists
    ***REMOVED***
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        public string GetTrainerGym(string loggedIn);
***REMOVED***
***REMOVED***
