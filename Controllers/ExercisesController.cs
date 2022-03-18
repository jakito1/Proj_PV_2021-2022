#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class ExercisesController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
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
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        ***REMOVED***
            return View(exercise);
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

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        ***REMOVED***
            var exercise = await _context.Exercise.FindAsync(id);
            _context.Exercise.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    ***REMOVED***

        private bool ExerciseExists(int id)
        ***REMOVED***
            return _context.Exercise.Any(e => e.ExerciseId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
