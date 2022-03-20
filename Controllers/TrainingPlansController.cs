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
        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = null;

            if (trainer != null)
            {
                 plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId);
            }
            
            if(client != null)
            {
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId);
            }

            if (!string.IsNullOrEmpty(searchString) && trainer != null)
            {
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).Where(a => a.TrainingPlanName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(searchString) && client != null)
            {
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId).Where(a => a.TrainingPlanName.Contains(searchString));
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: TrainingPlans/Details/5
        public async Task<IActionResult> TrainingPlanDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlan.Include(a=>a.Exercises).Include(a => a.Trainer.UserAccountModel)
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

        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription, ClientEmail")] TrainingPlan trainingPlan)
        {
            if (ModelState.IsValid)
            {                

                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(trainingPlan.ClientEmail))
                {
                    clientAccount = await _userManager.FindByEmailAsync(trainingPlan.ClientEmail);
                }

                if (trainer != null && clientAccount != null)
                {                    
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
                }

                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);

                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                trainingPlan.Client = client;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowTrainingPlans");
        }

        public async Task<IActionResult> EditTrainingPlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);           
            if (trainingPlan == null)
            {
                return NotFound();
            }
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, trainingPlan.Exercises);
            return View(trainingPlan);
        }


        [HttpPost, ActionName("EditTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName, u => u.TrainingPlanDescription))
            {
                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, null);

                var excludedIDs = new HashSet<int>(exercises.Select(a => a.ExerciseId));
                var missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                _context.Exercise.RemoveRange(missingRows);

                trainingPlanToUpdate.Exercises = exercises;
                _context.SaveChanges();
                return RedirectToAction("ShowTrainingPlans");
            }
            return View(trainingPlanToUpdate);
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
            return RedirectToAction("ShowTrainingPlans");
        }
    }
}
