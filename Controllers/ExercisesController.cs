﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    [Authorize(Roles = "client, trainer, gym")]
    public class ExercisesController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly string SessionKeyPhoto;
        private readonly IPhotoManagement _photoManagement;

        public ExercisesController(IPhotoManagement photoManagement)
        ***REMOVED***
            SessionKeyExercises = "_Exercises";
            SessionKeyPhoto = "_Photo";
            _photoManagement = photoManagement;
    ***REMOVED***

        public IActionResult ShowExercisesList()
        ***REMOVED***
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            ***REMOVED***
                exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                return PartialView("_ShowExercisesPartial", exercises);
        ***REMOVED***

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
    ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateExercise([Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise,
            IFormFile? formFile)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                List<Exercise>? exercises;
                Photo? oldPhoto = HttpContext.Session.Get<Photo>(SessionKeyPhoto);
                Photo? newPhoto = _photoManagement.UploadProfilePhoto(formFile);
                if ((oldPhoto is not null && newPhoto is not null) || (newPhoto is not null && oldPhoto is null))
                ***REMOVED***
                    exercise.ExercisePhoto = newPhoto;
            ***REMOVED***
                else
                ***REMOVED***
                    exercise.ExercisePhoto = oldPhoto;
            ***REMOVED***

                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() ***REMOVED*** exercise ***REMOVED***);
            ***REMOVED***
                else
                ***REMOVED***
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    if (exercises is not null)
                    ***REMOVED***
                        exercises.Add(exercise);
                        HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***
    ***REMOVED***

        public IActionResult EditExercise(int id)
        ***REMOVED***
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Exercise exercise = exercises[id];
            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            if (exercise.ExercisePhoto is not null)
            ***REMOVED***
                HttpContext.Session.Set<Photo>(SessionKeyPhoto, exercise.ExercisePhoto);
        ***REMOVED***
            return PartialView("_CreateExercisePartial", exercise);

    ***REMOVED***

        public IActionResult DeleteExercise(int id)
        ***REMOVED***
            List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
            if (exercises is not null)
            ***REMOVED***
                exercises.RemoveAt(id);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

                return PartialView("_ShowExercisesPartial", exercises);
        ***REMOVED***

            return PartialView("_ShowExercisesPartial", new List<Exercise>());
    ***REMOVED***
        public IActionResult GetCleanCreateExercisePartial()
        ***REMOVED***
            return PartialView("_CreateExercisePartial");
    ***REMOVED***

***REMOVED***
***REMOVED***
