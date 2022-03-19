#nullable disable
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IGetExercisesForCurrentUser _getExercisesForCurrentUser;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IGetExercisesForCurrentUser getExercisesForCurrentUser)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _getExercisesForCurrentUser = getExercisesForCurrentUser;
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlans()
        ***REMOVED***
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            return View(await _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).ToListAsync());
    ***REMOVED***

        // GET: TrainingPlans/Details/5
        public async Task<IActionResult> TrainingPlanDetails(int? id)
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

        // GET: TrainingPlans/Create
        public IActionResult CreateTrainingPlan()
        ***REMOVED***
            return View();
    ***REMOVED***

        // POST: TrainingPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription")] TrainingPlan trainingPlan)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                var exercises = _getExercisesForCurrentUser.GetExercises(user.Id).ToList();
                foreach (var exercise in exercises)
                ***REMOVED***
                    exercise.UserAccount = null;
            ***REMOVED***
                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***

        // GET: TrainingPlans/Edit/5
        public async Task<IActionResult> EditTrainingPlan(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlan = await _context.TrainingPlan.FindAsync(id);
            if (trainingPlan == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(trainingPlan);
    ***REMOVED***

        // POST: TrainingPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("EditTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanPost(int id, [Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription")] TrainingPlan trainingPlan)
        ***REMOVED***
            if (id != trainingPlan.TrainingPlanId)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    _context.Update(trainingPlan);
                    await _context.SaveChangesAsync();
            ***REMOVED***
                catch (DbUpdateConcurrencyException)
                ***REMOVED***
                    if (!TrainingPlanExists(trainingPlan.TrainingPlanId))
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
            return View(trainingPlan);
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
            return RedirectToAction(nameof(Index));
    ***REMOVED***

        private bool TrainingPlanExists(int id)
        ***REMOVED***
            return _context.TrainingPlan.Any(e => e.TrainingPlanId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
