using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    [Authorize(Roles = "client, trainer")]
    public class ExercisesController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly IPhotoManagement _photoManagement;

        public ExercisesController(IPhotoManagement photoManagement)
        {
            SessionKeyExercises = "_Exercises";
            _photoManagement = photoManagement;
        }

        public IActionResult ShowExercisesList()
        {
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            {
                exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                return PartialView("_ShowExercisesPartial", exercises);
            }

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateExercise([Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise,
            IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                List<Exercise>? exercises;
                exercise.ExercisePhoto = _photoManagement.UploadProfilePhoto(formFile);
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) is null)
                {
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() { exercise });
                }
                else
                {
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    if (exercises is not null)
                    {
                        exercises.Add(exercise);
                        HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
                    }
                }
            }
        }

        public IActionResult EditExercise(int id)
        {
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            {
                return NotFound();
            }

            Exercise exercise = exercises[id];
            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            return PartialView("_CreateExercisePartial", exercise);

        }

        public IActionResult DeleteExercise(int id)
        {
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            {
                exercises.RemoveAt(id);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

                return PartialView("_ShowExercisesPartial", exercises);
            }

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
        }
        public IActionResult GetCleanCreateExercisePartial()
        {
            return PartialView("_CreateExercisePartial");
        }

    }
}
