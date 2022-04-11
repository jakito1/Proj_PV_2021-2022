using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    [Authorize(Roles = "client, trainer")]
    public class TrainingPlansController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentTrainer;
        private readonly string SessionKeyTrainingPlanNewRequestId;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        public TrainingPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
            SessionKeyExercises = "_Exercises";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentTrainer = "_CurrentTrainer";
            SessionKeyTrainingPlanNewRequestId = "_TrainingPlanNewRequestId";
    ***REMOVED***

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
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .Include(a => a.Trainer.UserAccountModel)
                .Include(a => a.Client.UserAccountModel)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            trainingPlan.Exercises = exercises;
            return View(trainingPlan);
    ***REMOVED***

        public async Task<IActionResult> CreateTrainingPlan(int? trainingPlanNewRequestId)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentTrainer, trainer);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync());
            if (trainingPlanNewRequestId is not null)
            ***REMOVED***
                @ViewData["ClientEmail"] = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId).Select(a => a.Client.UserAccountModel.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyTrainingPlanNewRequestId, trainingPlanNewRequestId);
        ***REMOVED***
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
                Client client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                int? trainingPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyTrainingPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(trainingPlan.ClientEmail))
                ***REMOVED***
                    clientAccount = await _userManager.FindByEmailAsync(trainingPlan.ClientEmail);
            ***REMOVED***
                else if (trainingPlanNewRequestId is not null)
                ***REMOVED***
                    clientAccount = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId).Select(a => a.Client.UserAccountModel).FirstOrDefaultAsync();
            ***REMOVED***

                if (trainer is not null && clientAccount is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
            ***REMOVED***

                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Clear();

                if (trainingPlanNewRequestId is not null)
                ***REMOVED***
                    TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FirstOrDefaultAsync(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId);
                    if (trainingPlanNewRequest is not null)
                    ***REMOVED***
                        trainingPlan.TrainingPlanNewRequestId = trainingPlanNewRequestId;
                        trainingPlanNewRequest.TrainingPlanNewRequestDone = true;
                        await _interactNotification.Create($"O seu novo plano de treino está pronto.", client.UserAccountModel);
                ***REMOVED***
            ***REMOVED***
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

            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            trainingPlan.Exercises = exercises;
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

            TrainingPlan? trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises).Include(a => a.Client.UserAccountModel).FirstOrDefaultAsync(a => a.TrainingPlanId == id);

            TrainingPlanEditRequest? trainingPlanEditRequest = null;
            if (trainingPlanToUpdate is not null)
            ***REMOVED***
                trainingPlanEditRequest = await _context.TrainingPlanEditRequests.OrderByDescending(a => a.TrainingPlanEditRequestDate).
                    FirstOrDefaultAsync(a => a.TrainingPlan == trainingPlanToUpdate);
        ***REMOVED***

            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName, u => u.TrainingPlanDescription))
            ***REMOVED***
                List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Remove(SessionKeyExercises);

                HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                IEnumerable<Exercise>? missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                _context.Exercise.RemoveRange(missingRows);

                trainingPlanToUpdate.Exercises = exercises;
                trainingPlanToUpdate.ToBeEdited = false;

                if (trainingPlanEditRequest is not null)
                ***REMOVED***
                    trainingPlanEditRequest.TrainingPlanEditRequestDone = true;
                    await _interactNotification.Create($"O seu plano de treino foi editado com sucesso.", trainingPlanToUpdate.Client.UserAccountModel);
            ***REMOVED***

                await _context.SaveChangesAsync();
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
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (trainer is not null && trainingPlan is not null && trainer.TrainingPlans.Contains(trainingPlan))
            ***REMOVED***
                trainingPlan.Trainer = null;
                await _context.SaveChangesAsync();
        ***REMOVED***

            if (client is not null && trainingPlan is not null)
            ***REMOVED***
                _context.TrainingPlan.Remove(trainingPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***

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
