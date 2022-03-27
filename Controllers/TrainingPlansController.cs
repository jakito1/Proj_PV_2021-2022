using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class TrainingPlansController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentTrainer;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentTrainer = "_CurrentTrainer";
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        {

            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            HttpContext.Session.Remove(SessionKeyExercises);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = null;

            if (trainer is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).Include(a => a.Client.UserAccountModel);
            }

            if (client is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId);
            }

            if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId)
                    .Where(a => a.TrainingPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client.UserAccountModel);
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId).Where(a => a.TrainingPlanName.Contains(searchString));
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> TrainingPlanDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .Include(a => a.Exercises)
                .Include(a => a.Trainer.UserAccountModel)
                .Include(a => a.Client.UserAccountModel)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            {
                return NotFound();
            }

            return View(trainingPlan);
        }

        public async Task<IActionResult> CreateTrainingPlan()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set<Trainer>(SessionKeyCurrentTrainer, trainer);
            HttpContext.Session.Set<List<Client>>(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync());
            return View();
        }

        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription,ClientEmail")] TrainingPlan trainingPlan)
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

                if (trainer is not null && clientAccount is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
                }

                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Clear();

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
            if (id is null)
            {
                return NotFound();
            }

            TrainingPlan? trainingPlan = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (trainingPlan is null)
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
            if (id is null)
            {
                return NotFound();
            }

            TrainingPlan? trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName, u => u.TrainingPlanDescription))
            {
                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Remove(SessionKeyExercises);

                HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                IEnumerable<Exercise>? missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                _context.Exercise.RemoveRange(missingRows);

                trainingPlanToUpdate.Exercises = exercises;
                _context.SaveChanges();
                return RedirectToAction("ShowTrainingPlans");
            }
            return View(trainingPlanToUpdate);
        }

        public async Task<IActionResult> DeleteTrainingPlan(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            {
                return NotFound();
            }

            return View(trainingPlan);
        }

        [HttpPost, ActionName("DeleteTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int id)
        {
            TrainingPlan? trainingPlan = await _context.TrainingPlan.FindAsync(id);
            _context.TrainingPlan.Remove(trainingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowTrainingPlans");
        }
    }
}
