#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class ExercisesController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public ExercisesController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
    ***REMOVED***

        // GET: Exercises
        public async Task<IActionResult> Index()
        ***REMOVED***
            return View(await _context.Exercise.ToListAsync());
    ***REMOVED***

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var exercise = await _context.Exercise
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(exercise);
    ***REMOVED***

        // GET: Exercises/Create
        public IActionResult CreateExercise()
        ***REMOVED***
            return View();
    ***REMOVED***

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateExercise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExercisePost([Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***

                List<Exercise> exercises = new();
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) is null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() ***REMOVED*** exercise ***REMOVED***);
                    exercises.Add(exercise);
            ***REMOVED***
                else
                ***REMOVED***
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    exercises.Add(exercise);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
            ***REMOVED***


                var _CreateExercise = await ViewRenderService.RenderViewToStringAsync(this, "_CreateExercisePartial", null);

                var _ShowExercises = await ViewRenderService.RenderViewToStringAsync(this, "_ShowExercisesPartial", exercises);
                var json = Json(new ***REMOVED*** _CreateExercise, _ShowExercises ***REMOVED***);
                return json;
        ***REMOVED***

            return BadRequest();
    ***REMOVED***

        public async Task<IActionResult> EditExercise(int id)
        ***REMOVED***

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var _CreateExercise = await ViewRenderService.RenderViewToStringAsync(this, "_CreateExercisePartial", exercises[id]);

            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

            var _ShowExercises = await ViewRenderService.RenderViewToStringAsync(this, "_ShowExercisesPartial", exercises);
            var json = Json(new ***REMOVED*** _CreateExercise, _ShowExercises ***REMOVED***);
            return json;
    ***REMOVED***

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExercise(int id, [Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise)
        ***REMOVED***
            if (id != exercise.ExerciseId)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
            ***REMOVED***
                catch (DbUpdateConcurrencyException)
                ***REMOVED***
                    if (!ExerciseExists(exercise.ExerciseId))
                    ***REMOVED***
                        return NotFound();
                ***REMOVED***
                    else
                    ***REMOVED***
                        throw;
                ***REMOVED***
            ***REMOVED***
                return RedirectToAction(nameof(Index));
        ***REMOVED***
            return View(exercise);
    ***REMOVED***

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

        private bool ExerciseExists(int id)
        ***REMOVED***
            return _context.Exercise.Any(e => e.ExerciseId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
