using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class GetUsersLists : IGetUsersLists
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public GetUsersLists(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string>? role = from a in _context.Roles where a.Name == userType select a.Id;
            if (userType == "client")
            ***REMOVED***
                return _context.Client.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).OrderByDescending(a => a.DateAddedToGym).Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "trainer")
            ***REMOVED***
                return _context.Trainer.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED***
            else if (userType == "nutritionist")
            ***REMOVED***
                return _context.Nutritionist.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED***
            return null;
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInTrainer = from a in _context.Trainer where a.UserAccountModel.Id == loggedIn select a.TrainerId;
            return _context.Client.Where(a => a.Trainer.TrainerId == loggedInTrainer.FirstOrDefault()).OrderByDescending(a => a.DateAddedToTrainer).Select(a => a.UserAccountModel);
    ***REMOVED***
        public IEnumerable<UserAccountModel> GetUsersForNutritionist(string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInNutritionist = from a in _context.Nutritionist where a.UserAccountModel.Id == loggedIn select a.NutritionistId;
            return _context.Client.Where(a => a.Nutritionist.NutritionistId == loggedInNutritionist.FirstOrDefault()).OrderByDescending(a => a.DateAddedToNutritionist).Select(a => a.UserAccountModel);
    ***REMOVED***

        public string GetTrainerGym(string loggedIn)
        ***REMOVED***
            Gym? gym = _context.Trainer.Where(a => a.UserAccountModel.Id == loggedIn).Select(a => a.Gym).FirstOrDefault();
            return (gym is null) ? "" : gym.GymName;
    ***REMOVED***

***REMOVED***
***REMOVED***
