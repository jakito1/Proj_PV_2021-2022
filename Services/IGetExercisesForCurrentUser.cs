using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IGetExercisesForCurrentUser
    {
        IEnumerable<Exercise> GetExercises(string? loggedIn);
    }
}
