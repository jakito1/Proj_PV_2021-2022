using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class CountUsers : ICountUsers
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public CountUsers(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public IEnumerable<UserAccountModel> UserCount(string userType, string loggedIn)
        ***REMOVED***
            IQueryable<int>? loggedInGym = from a in _context.Gym where a.UserAccount.Id == loggedIn select a.GymId;
            IQueryable<string> ? role = from a in _context.Roles where a.Name == userType select a.Id;
            IQueryable<UserAccountModel>? returnQuery = null;
            if (userType == "client")
            ***REMOVED***
                returnQuery = from a in _context.Client where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
        ***REMOVED*** else if (userType == "trainer")
            ***REMOVED***
                returnQuery = from a in _context.Trainer where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
        ***REMOVED*** else if (userType == "nutritionist")
            ***REMOVED***
                returnQuery = from a in _context.Nutritionist where a.Gym.GymId == loggedInGym.FirstOrDefault() select a.UserAccountModel;
        ***REMOVED***


            return returnQuery.ToList();
    ***REMOVED***

***REMOVED***
***REMOVED***
