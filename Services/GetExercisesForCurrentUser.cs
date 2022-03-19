using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class GetExercisesForCurrentUser : IGetExercisesForCurrentUser
    {

        private readonly ApplicationDbContext _context;
        public GetExercisesForCurrentUser(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Exercise> GetExercises(string? loggedIn)
        {
            return _context.Exercise.Where(a => a.UserAccount.Id == loggedIn);
        }
    }
}
