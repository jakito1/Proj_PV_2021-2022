using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainingPlansController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***

            if (searchString != null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId);

            if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).Where(a => a.TrainingPlanName.Contains(searchString));
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        // GET: TrainingPlans/Details/5
        public async Task<IActionResult> TrainingPlanDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlan = await _context.TrainingPlan.Include(a=>a.Exercises)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlan);
    ***REMOVED***

        // GET: TrainingPlans/Create
        public IActionResult CreateTrainingPlan()
        ***REMOVED***
            return View();
    ***REMOVED***

        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription")] TrainingPlan trainingPlan)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);

                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***

        public async Task<IActionResult> EditTrainingPlan(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlan = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);           
            if (trainingPlan == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, trainingPlan.Exercises);
            return View(trainingPlan);
    ***REMOVED***


        [HttpPost, ActionName("EditTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanPost(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName, u => u.TrainingPlanDescription))
            ***REMOVED***
                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);

                var excludedIDs = new HashSet<int>(exercises.Select(a => a.ExerciseId));
                var missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                _context.Exercise.RemoveRange(missingRows);

                trainingPlanToUpdate.Exercises = exercises;
                _context.SaveChanges();
                return RedirectToAction("ShowTrainingPlans");
        ***REMOVED***
            return View(trainingPlanToUpdate);
    ***REMOVED***

        // GET: TrainingPlans/Delete/5
        public async Task<IActionResult> DeleteTrainingPlan(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlan = await _context.TrainingPlan
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlan);
    ***REMOVED***

        // POST: TrainingPlans/Delete/5
        [HttpPost, ActionName("DeleteTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int id)
        ***REMOVED***
            var trainingPlan = await _context.TrainingPlan.FindAsync(id);
            _context.TrainingPlan.Remove(trainingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***
***REMOVED***
***REMOVED***
