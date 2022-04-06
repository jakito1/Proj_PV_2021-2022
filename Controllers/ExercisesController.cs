using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// ExercisesController class, derives from Controller.
    /// </summary>
    public class ExercisesController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoManagement">Photo management API</param>
        public ExercisesController(IPhotoManagement photoManagement)
        {
            SessionKeyExercises = "_Exercises";
            _photoManagement = photoManagement;
        }

        /// <summary>
        /// Displays a partial view with a list of exercises.
        /// </summary>
        /// <returns>An action result</returns>
        public IActionResult ShowExercisesList()
        {
            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            {
                exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                return PartialView("_ShowExercisesPartial", exercises);
            }

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
        }

        /// <summary>
        /// Http POST method on the API to create an exercise.
        /// </summary>
        /// <param name="exercise"></param>
        /// <param name="formFile"></param>
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
                    exercises.Add(exercise);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
                }
            }
        }

        /// <summary>
        /// Displays a partial view to edit an exercise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
        public IActionResult EditExercise(int id)
        {

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            {
                return NotFound();
            }

            Exercise exercise = exercises[id];
            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            return PartialView("_CreateExercisePartial", exercise);

        }

        /// <summary>
        /// Displays a partial view to delete an exercise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An action result</returns>
        public IActionResult DeleteExercise(int id)
        {

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            {
                exercises.RemoveAt(id);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

                return PartialView("_ShowExercisesPartial", exercises);
            }

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
        }
        /// <summary>
        /// Displays a partial view to create an exercise.
        /// </summary>
        /// <returns>An action result</returns>
        public IActionResult GetCleanCreateExercisePartial()
        {
            return PartialView("_CreateExercisePartial");
        }

    }
}
