using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    [Authorize(Roles = "client, trainer")]
    public class TrainingPlansController : Controller
    {
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
        {
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
            SessionKeyExercises = "_Exercises";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentTrainer = "_CurrentTrainer";
            SessionKeyTrainingPlanNewRequestId = "_TrainingPlanNewRequestId";
        }

        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            HttpContext.Session.Clear();
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = null;

            if (trainer is not null && string.IsNullOrEmpty(searchString))
            {
                plans = _context.TrainingPlan.Where(a => a.Trainer != null && a.Trainer.TrainerId == trainer.TrainerId).Include(a => a.Client!.UserAccountModel);
            }
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Trainer != null && a.Trainer.TrainerId == trainer.TrainerId)
                    .Where(a => a.TrainingPlanName != null && a.TrainingPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client!.UserAccountModel);
            }
            else if (client is not null && string.IsNullOrEmpty(searchString))
            {
                plans = _context.TrainingPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId);
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                plans = _context.TrainingPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId)
                    .Where(a => a.TrainingPlanName != null && a.TrainingPlanName.Contains(searchString));
            }

            if (plans is not null)
            {
                int pageSize = 5;
                return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }

        public async Task<IActionResult> TrainingPlanDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .Include(a => a.Trainer!.UserAccountModel)
                .Include(a => a.Client!.UserAccountModel)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            {
                return NotFound();
            }
            trainingPlan.Exercises = exercises;
            return View(trainingPlan);
        }

        public async Task<IActionResult> CreateTrainingPlan(int? trainingPlanNewRequestId)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentTrainer, trainer);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync());
            if (trainingPlanNewRequestId is not null)
            {
                ViewData["ClientEmail"] = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId)
                    .Select(a => a.Client!.UserAccountModel.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyTrainingPlanNewRequestId, trainingPlanNewRequestId);
            }
            return View();
        }

        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription,ClientEmail")] TrainingPlan trainingPlan)
        {
            if (ModelState.IsValid && User.Identity is not null)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                int? trainingPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyTrainingPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(trainingPlan.ClientEmail))
                {
                    clientAccount = await _userManager.FindByEmailAsync(trainingPlan.ClientEmail);
                }
                else if (trainingPlanNewRequestId is not null)
                {
                    clientAccount = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId).Select(a => a.Client!.UserAccountModel).FirstOrDefaultAsync();
                }

                if (trainer is not null && clientAccount is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
                }

                List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Clear();

                if (trainingPlanNewRequestId is not null)
                {
                    TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FirstOrDefaultAsync(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId);
                    if (trainingPlanNewRequest is not null)
                    {
                        trainingPlan.TrainingPlanNewRequestId = trainingPlanNewRequestId;
                        trainingPlanNewRequest.TrainingPlanNewRequestDone = true;
                    }
                }

                if (client is null)
                {
                    return NotFound();
                }

                await _interactNotification.Create($"O seu novo plano de treino está pronto.", client.UserAccountModel);
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

            TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (trainingPlan is null)
            {
                return NotFound();
            }
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            trainingPlan.Exercises = exercises;
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

            TrainingPlan? trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises)
                .Include(a => a.Client!.UserAccountModel).FirstOrDefaultAsync(a => a.TrainingPlanId == id);

            if (trainingPlanToUpdate is null)
            {
                return NotFound();
            }

            TrainingPlanEditRequest? trainingPlanEditRequest = null;

            trainingPlanEditRequest = await _context.TrainingPlanEditRequests.OrderByDescending(a => a.TrainingPlanEditRequestDate).
                FirstOrDefaultAsync(a => a.TrainingPlan == trainingPlanToUpdate);


            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName!, u => u.TrainingPlanDescription!))
            {
                List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Remove(SessionKeyExercises);
                if (exercises is not null && exercises.Any() && trainingPlanToUpdate.Exercises is not null)
                {
                    HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                    IEnumerable<Exercise>? missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));
                    _context.Exercise.RemoveRange(missingRows);
                }

                trainingPlanToUpdate.Exercises = exercises;
                trainingPlanToUpdate.ToBeEdited = false;

                if (trainingPlanEditRequest is not null && trainingPlanToUpdate.Client is not null)
                {
                    trainingPlanEditRequest.TrainingPlanEditRequestDone = true;
                    await _interactNotification.Create($"O seu plano de treino foi editado com sucesso.", trainingPlanToUpdate.Client.UserAccountModel);
                }

                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
            TrainingPlan? trainingPlan = await _context.TrainingPlan.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (trainer is not null && trainingPlan is not null && trainer.TrainingPlans is not null && trainer.TrainingPlans.Contains(trainingPlan))
            {
                trainingPlan.Trainer = null;
                await _context.SaveChangesAsync();
            }

            if (client is not null && trainingPlan is not null)
            {
                _context.TrainingPlan.Remove(trainingPlan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ShowTrainingPlans");
        }

        public async Task<IActionResult> VerifyClientEmail(string? clientEmail)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Trainer? trainer = HttpContext.Session.Get<Trainer>(SessionKeyCurrentTrainer);
            Client? client = null;
            if (clientsUsersAccounts is not null)
            {
                client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == clientEmail);
            }

            if (clientsUsersAccounts is null || trainer is null)
            {
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                clientsUsersAccounts = await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync();
                trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            }

            if (client is not null)
            {
                return Json(true);
            }

            return Json($"O email inserido não pertence a um dos seus clientes.");
        }
    }
}
