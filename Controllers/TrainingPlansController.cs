using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// TrainingPlansController class, derives from Controller.
    /// Only authorized Client and Trainer roles.
    /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User manager API from Entity framework</param>
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
        /// <summary>
        /// Renders a paginated view to display all the new Training Plans.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> ShowTrainingPlans(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
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
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TrainingPlan>? plans = null;

            if (trainer is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Trainer != null && a.Trainer.TrainerId == trainer.TrainerId).Include(a => a.Client!.UserAccountModel);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Trainer != null && a.Trainer.TrainerId == trainer.TrainerId)
                    .Where(a => a.TrainingPlanName != null && a.TrainingPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client!.UserAccountModel);
        ***REMOVED***
            else if (client is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                plans = _context.TrainingPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId)
                    .Where(a => a.TrainingPlanName != null && a.TrainingPlanName.Contains(searchString));
        ***REMOVED***

            if (plans is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<TrainingPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        /// <summary>
        /// Renders a view with the details of a new Training plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> TrainingPlanDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            TrainingPlan? trainingPlan = await _context.TrainingPlan
                .Include(a => a.Trainer!.UserAccountModel)
                .Include(a => a.Client!.UserAccountModel)
                .FirstOrDefaultAsync(m => m.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            trainingPlan.Exercises = exercises;
            return View(trainingPlan);
    ***REMOVED***

        /// <summary>
        /// Renders a view to create a new Nutrition Plan request. 
        /// </summary>
        /// <param name="trainingPlanNewRequestId"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> CreateTrainingPlan(int? trainingPlanNewRequestId)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentTrainer, trainer);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Trainer == trainer).Include(a => a.UserAccountModel).ToListAsync());
            if (trainingPlanNewRequestId is not null)
            ***REMOVED***
                ViewData["ClientEmail"] = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId)
                    .Select(a => a.Client!.UserAccountModel.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyTrainingPlanNewRequestId, trainingPlanNewRequestId);
        ***REMOVED***
            return View();
    ***REMOVED***

        /// <summary>
        /// HTTP POST action on the API to create a new Training Plan.
        /// </summary>
        /// <param name="trainingPlan"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("CreateTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanPost([Bind("TrainingPlanId,TrainingPlanName,TrainingPlanDescription,ClientEmail")] TrainingPlan trainingPlan)
        ***REMOVED***
            if (ModelState.IsValid && User.Identity is not null)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                int? trainingPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyTrainingPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(trainingPlan.ClientEmail))
                ***REMOVED***
                    clientAccount = await _userManager.FindByEmailAsync(trainingPlan.ClientEmail);
            ***REMOVED***
                else if (trainingPlanNewRequestId is not null)
                ***REMOVED***
                    clientAccount = await _context.TrainingPlanNewRequests.Where(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId).Select(a => a.Client!.UserAccountModel).FirstOrDefaultAsync();
            ***REMOVED***

                if (trainer is not null && clientAccount is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
            ***REMOVED***

                List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Clear();

                if (trainingPlanNewRequestId is not null)
                ***REMOVED***
                    TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FirstOrDefaultAsync(a => a.TrainingPlanNewRequestId == trainingPlanNewRequestId);
                    if (trainingPlanNewRequest is not null)
                    ***REMOVED***
                        trainingPlan.TrainingPlanNewRequestId = trainingPlanNewRequestId;
                        trainingPlanNewRequest.TrainingPlanNewRequestDone = true;
                ***REMOVED***
            ***REMOVED***

                if (client is null)
                ***REMOVED***
                    return NotFound();
            ***REMOVED***

                await _interactNotification.Create($"O seu novo plano de treino está pronto.", client.UserAccountModel);
                trainingPlan.Exercises = exercises;
                trainingPlan.Trainer = trainer;
                trainingPlan.Client = client;
                _context.Add(trainingPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowTrainingPlans");
    ***REMOVED***

        /// <summary>
        /// Renders a view to Edit a Training Plan, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> EditTrainingPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == id);
            if (trainingPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            trainingPlan.Exercises = exercises;
            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, trainingPlan.Exercises);
            return View(trainingPlan);
    ***REMOVED***

        /// <summary>
        /// HTTP POST method on the API to Edit a Training plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("EditTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanPost(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlan? trainingPlanToUpdate = await _context.TrainingPlan.Include(a => a.Exercises)
                .Include(a => a.Client!.UserAccountModel).FirstOrDefaultAsync(a => a.TrainingPlanId == id);

            if (trainingPlanToUpdate is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlanEditRequest? trainingPlanEditRequest = null;

            trainingPlanEditRequest = await _context.TrainingPlanEditRequests.OrderByDescending(a => a.TrainingPlanEditRequestDate).
                FirstOrDefaultAsync(a => a.TrainingPlan == trainingPlanToUpdate);


            if (await TryUpdateModelAsync<TrainingPlan>(trainingPlanToUpdate, "",
                u => u.TrainingPlanName!, u => u.TrainingPlanDescription!))
            ***REMOVED***
                List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                HttpContext.Session.Remove(SessionKeyExercises);
                if (exercises is not null && exercises.Any() && trainingPlanToUpdate.Exercises is not null)
                ***REMOVED***
                    HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                    IEnumerable<Exercise>? missingRows = trainingPlanToUpdate.Exercises.Where(a => !excludedIDs.Contains(a.ExerciseId));
                    _context.Exercise.RemoveRange(missingRows);
            ***REMOVED***

                trainingPlanToUpdate.Exercises = exercises;
                trainingPlanToUpdate.ToBeEdited = false;

                if (trainingPlanEditRequest is not null && trainingPlanToUpdate.Client is not null)
                ***REMOVED***
                    trainingPlanEditRequest.TrainingPlanEditRequestDone = true;
                    await _interactNotification.Create($"O seu plano de treino foi editado com sucesso.", trainingPlanToUpdate.Client.UserAccountModel);
            ***REMOVED***

                await _context.SaveChangesAsync();
                return RedirectToAction("ShowTrainingPlans");
        ***REMOVED***
            return View(trainingPlanToUpdate);
    ***REMOVED***

        /// <summary>
        /// Renders a view to Delete a Training plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
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

        /// <summary>
        /// HTTP POST action on the API to Delete a Training Plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [HttpPost, ActionName("DeleteTrainingPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanConfirmed(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            TrainingPlan? trainingPlan = await _context.TrainingPlan.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (trainer is not null && trainingPlan is not null && trainer.TrainingPlans is not null && trainer.TrainingPlans.Contains(trainingPlan))
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

        /// <summary>
        /// Auxiliary method that confirms is a certain email belongs to a client and returns a JSON with the response.
        /// </summary>
        /// <param name="nutritionPlan"></param>
        /// <returns>A JSON result</returns>
        public async Task<IActionResult> VerifyClientEmail(string? clientEmail)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Trainer? trainer = HttpContext.Session.Get<Trainer>(SessionKeyCurrentTrainer);
            Client? client = null;
            if (clientsUsersAccounts is not null)
            ***REMOVED***
                client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == clientEmail);
        ***REMOVED***

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

            return Json($"O email inserido não pertence a um dos seus clientes.");
    ***REMOVED***
***REMOVED***
***REMOVED***
