using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class GetUsersForGym : IGetUsersForGym
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public GetUsersForGym(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public IEnumerable<UserAccountModel> GetUsers(string userType, string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccountModel.Id == loggedIn select a.GymId;
            IQueryable<string> ? role = from a in _context.Roles where a.Name == userType select a.Id;
            if (userType == "client")
            ***REMOVED***
                return _context.Client.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).OrderByDescending(a => a.RowVersion).Select(a => a.UserAccountModel);
        ***REMOVED*** else if (userType == "trainer")
            ***REMOVED***
                return _context.Trainer.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);
        ***REMOVED*** else if (userType == "nutritionist")
            ***REMOVED***
                return _context.Nutritionist.Where(a => a.Gym.GymId == loggedInGym.FirstOrDefault()).Select(a => a.UserAccountModel);               
        ***REMOVED***
            return null;
    ***REMOVED***

***REMOVED***
***REMOVED***
