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
{
    public class TrainingPlansController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlans()
        {
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            return View(await _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).ToListAsync());
        }

        // GET: TrainingPlans/Details/5
        public async Task<IActionResult> TrainingPlanDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlan
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan == null)
            {
                return NotFound();
            }

            return View(trainingPlan);
        }

        // GET: TrainingPlans/Create
        public IActionResult CreateTrainingPlan()
        {
            return View();
        }

        // POST: TrainingPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription")] TrainingPlan trainingPlan)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                //var exercises = _getExercisesForCurrentUser.GetExercises(user.Id).ToList();

                //foreach (var exercise in exercises)
                //{
                //exercise.UserAccount = null;
                //}
                List<Exercise> exercises = null;
                if (HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises) != null)
                {
                    exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);
                }

                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowTrainingPlans");
        }

        // GET: TrainingPlans/Edit/5
        public async Task<IActionResult> EditTrainingPlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlan.FindAsync(id);
            if (trainingPlan == null)
            {
                return NotFound();
            }
            return View(trainingPlan);
        }

        // POST: TrainingPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("EditTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanPost(int id, [Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription")] TrainingPlan trainingPlan)
        {
            if (id != trainingPlan.TrainingPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingPlanExists(trainingPlan.TrainingPlanId))
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
            return View(trainingPlan);
        }

        // GET: TrainingPlans/Delete/5
        public async Task<IActionResult> DeleteTrainingPlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlan
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan == null)
            {
                return NotFound();
            }

            return View(trainingPlan);
        }

        // POST: TrainingPlans/Delete/5
        [HttpPost, ActionName("DeleteTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int id)
        {
            var trainingPlan = await _context.TrainingPlan.FindAsync(id);
            _context.TrainingPlan.Remove(trainingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingPlanExists(int id)
        {
            return _context.TrainingPlan.Any(e => e.TrainingPlanId == id);
        }
    }
}
