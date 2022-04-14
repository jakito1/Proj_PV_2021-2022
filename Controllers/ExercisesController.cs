using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// ExercisesController class, derives from Controller.
    /// </summary>
    public class ExercisesController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoManagement">Photo management API</param>
        public ExercisesController(IPhotoManagement photoManagement)
        ***REMOVED***
            SessionKeyExercises = "_Exercises";
            _photoManagement = photoManagement;
    ***REMOVED***

        /// <summary>
        /// Displays a partial view with a list of exercises.
        /// </summary>
        /// <returns>An action result</returns>
        public IActionResult ShowExercisesList()
        ***REMOVED***
            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            ***REMOVED***
                exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                return PartialView("_ShowExercisesPartial", exercises);
        ***REMOVED***

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
    ***REMOVED***

        /// <summary>
        /// Http POST method on the API to create an exercise.
        /// </summary>
        /// <param name="exercise"></param>
        /// <param name="formFile"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateExercise([Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise,
            IFormFile? formFile)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                List<Exercise>? exercises;
                exercise.ExercisePhoto = _photoManagement.UploadProfilePhoto(formFile);
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() ***REMOVED*** exercise ***REMOVED***);
            ***REMOVED***
                else
                ***REMOVED***
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    exercises.Add(exercise);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Displays a partial view to edit an exercise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
        public IActionResult EditExercise(int id)
        ***REMOVED***

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Exercise exercise = exercises[id];
            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            return PartialView("_CreateExercisePartial", exercise);

    ***REMOVED***

        /// <summary>
        /// Displays a partial view to delete an exercise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An action result</returns>
        public IActionResult DeleteExercise(int id)
        ***REMOVED***

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            ***REMOVED***
                exercises.RemoveAt(id);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

                return PartialView("_ShowExercisesPartial", exercises);
        ***REMOVED***

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
    ***REMOVED***
        /// <summary>
        /// Displays a partial view to create an exercise.
        /// </summary>
        /// <returns>An action result</returns>
        public IActionResult GetCleanCreateExercisePartial()
        ***REMOVED***
            return PartialView("_CreateExercisePartial");
    ***REMOVED***

***REMOVED***
***REMOVED***
