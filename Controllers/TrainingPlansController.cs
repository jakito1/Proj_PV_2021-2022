using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainingPlansController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentTrainer;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentTrainer = "_CurrentTrainer";
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***

            if (searchString is not null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            HttpContext.Session.Clear();
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = null;

            if (trainer is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId).Include(a => a.Client.UserAccountModel);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Trainer.TrainerId == trainer.TrainerId)
                    .Where(a => a.TrainingPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client.UserAccountModel);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Client.ClientId == client.ClientId).Where(a => a.TrainingPlanName.Contains(searchString));
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        public async Task<IActionResult> TrainingPlanDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .Include(a => a.Exercises)
                .Include(a => a.Trainer.UserAccountModel)
                .Include(a => a.Client.UserAccountModel)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlan);
    ***REMOVED***

        public async Task<IActionResult> CreateTrainingPlan()
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set<Trainer>(SessionKeyCurrentTrainer, trainer);
            HttpContext.Session.Set<List<Client>>(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync());
            return View();
    ***REMOVED***

        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription,ClientEmail")] TrainingPlan trainingPlan)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(trainingPlan.ClientEmail))
                ***REMOVED***
                    clientAccount = await _userManager.FindByEmailAsync(trainingPlan.ClientEmail);
            ***REMOVED***

                if (trainer is not null && clientAccount is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
            ***REMOVED***

                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Clear();

                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                trainingPlan.Client = client;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***

        public async Task<IActionResult> EditTrainingPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlan = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (trainingPlan is null)
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
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises).FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName, u => u.TrainingPlanDescription))
            ***REMOVED***
                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Remove(SessionKeyExercises);

                HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                IEnumerable<Exercise>? missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                _context.Exercise.RemoveRange(missingRows);

                trainingPlanToUpdate.Exercises = exercises;
                _context.SaveChanges();
                return RedirectToAction("ShowTrainingPlans");
        ***REMOVED***
            return View(trainingPlanToUpdate);
    ***REMOVED***

        public async Task<IActionResult> DeleteTrainingPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlan);
    ***REMOVED***

        [HttpPost, ActionName("DeleteTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int id)
        ***REMOVED***
            TrainingPlan? trainingPlan = await _context.TrainingPlan.FindAsync(id);
            _context.TrainingPlan.Remove(trainingPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***
        
        public async Task<IActionResult> VerifyClientEmail(string? clientEmail)
        ***REMOVED***
            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Trainer? trainer = HttpContext.Session.Get<Trainer>(SessionKeyCurrentTrainer);
            Client? client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == clientEmail);

            if (clientsUsersAccounts is null || trainer is null)
            ***REMOVED***
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                clientsUsersAccounts = await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync();
                trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                return Json(true);
        ***REMOVED***

            return Json($"O email: ***REMOVED***clientEmail***REMOVED*** não pertence a um dos seus clientes.");
    ***REMOVED***
***REMOVED***
***REMOVED***
