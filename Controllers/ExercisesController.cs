#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var exercise = await _context.Exercise
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise == null)
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
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);

                //exercise.UserAccount = user;
                //_context.Add(exercise);
                //await _context.SaveChangesAsync();
                List<Exercise> lista = new();
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) == null)
                ***REMOVED***
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() ***REMOVED*** exercise ***REMOVED***);
                    lista.Add(exercise);
            ***REMOVED*** else
                ***REMOVED***
                    lista = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    lista.Add(exercise);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, lista);
            ***REMOVED***
                //TempData["ExercisePlan"] = exercise;
                return PartialView("_ShowExercisesPartial", lista);
        ***REMOVED***
           
            return BadRequest();
    ***REMOVED***

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(exercise);
    ***REMOVED***

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise)
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
            if (exercises != null)
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
