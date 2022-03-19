using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class GetExercisesForCurrentUser : IGetExercisesForCurrentUser
    ***REMOVED***

        private readonly ApplicationDbContext _context;
        public GetExercisesForCurrentUser(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public IEnumerable<Exercise> GetExercises(string? loggedIn)
        ***REMOVED***
            return _context.Exercise.Where(a => a.UserAccount.Id == loggedIn);
    ***REMOVED***
***REMOVED***
***REMOVED***
