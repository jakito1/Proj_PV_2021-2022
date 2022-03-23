#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public ExercisesController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exercise.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Exercise exercise = await _context.Exercise
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise is null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult CreateExercise()
        {
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateExercise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExercisePost([Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {

                List<Exercise> exercises = new();
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) is null)
                {
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, new List<Exercise>() { exercise });
                    exercises.Add(exercise);
                }
                else
                {
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    exercises.Add(exercise);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);
                }


                string _CreateExercise = await ViewRenderService.RenderViewToStringAsync(this, "_CreateExercisePartial", null);

                string _ShowExercises = await ViewRenderService.RenderViewToStringAsync(this, "_ShowExercisesPartial", exercises);
                JsonResult json = Json(new { _CreateExercise, _ShowExercises });
                return json;
            }

            return BadRequest();
        }

        public async Task<IActionResult> EditExercise(int id)
        {

            List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);

            if (exercises is null)
            {
                return NotFound();
            }

            string _CreateExercise = await ViewRenderService.RenderViewToStringAsync(this, "_CreateExercisePartial", exercises[id]);

            exercises.RemoveAt(id);
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, exercises);

            string _ShowExercises = await ViewRenderService.RenderViewToStringAsync(this, "_ShowExercisesPartial", exercises);
            JsonResult json = Json(new { _CreateExercise, _ShowExercises });
            return json;
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExercise(int id, [Bind("ExerciseId,ExerciseName,ExerciseDescription,ExerciseDuration,ExerciseRepetitions,ExerciseURL,ExerciseType,ExerciseMuscles")] Exercise exercise)
        {
            if (id != exercise.ExerciseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.ExerciseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

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

        private bool ExerciseExists(int id)
        {
            return _context.Exercise.Any(e => e.ExerciseId == id);
        }
    }
}
