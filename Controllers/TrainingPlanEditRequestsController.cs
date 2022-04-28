using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainingPlanEditRequestsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        public TrainingPlanEditRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanEditRequests(string? searchString, string? currentFilter, int? pageNumber)
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

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanEditRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanName != null && a.TrainingPlan.TrainingPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***
            else if (client is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client == client).Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client == client).
                    Where(a => a.TrainingPlan != null && a.TrainingPlan.TrainingPlanName != null && a.TrainingPlan.TrainingPlanName.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***

            if (requests is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<TrainingPlanEditRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanEditRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> TrainingPlanEditRequestDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequests
                .Include(t => t.TrainingPlan)
                .FirstOrDefaultAsync(m => m.TrainingPlanEditRequestId == id);
            if (trainingPlanEditRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanEditRequest);
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanEditRequest([Bind("TrainingPlanEditRequestDescription,TrainingPlanId")] TrainingPlanEditRequest trainingPlanEditRequest)
        ***REMOVED***
            if (ModelState.IsValid && User.Identity is not null)
            ***REMOVED***
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.TrainingPlans).Include(a => a.Trainer!.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == trainingPlanEditRequest.TrainingPlanId);
                IQueryable<TrainingPlanEditRequest>? amountEditRequests = null;
                if (trainingPlan is not null)
                ***REMOVED***
                    amountEditRequests = _context.TrainingPlanEditRequests.Where(a => a.TrainingPlan == trainingPlan).Where(a => a.TrainingPlanEditRequestDone == false);
            ***REMOVED***

                if (client is not null && trainingPlan is not null && client.TrainingPlans is not null &&
                    client.Trainer is not null && client.TrainingPlans.Contains(trainingPlan) &&
                    amountEditRequests is not null && !amountEditRequests.Any())
                ***REMOVED***
                    trainingPlan.ToBeEdited = true;
                    trainingPlanEditRequest.TrainingPlanEditRequestDate = DateTime.Now;
                    trainingPlanEditRequest.Client = client;
                    _context.Add(trainingPlanEditRequest);
                    await _interactNotification.Create($"O utilizador ***REMOVED***user.UserName***REMOVED*** requisitou a edição de um plano de treino.", client.Trainer.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TrainingPlanDetails", "TrainingPlans", new ***REMOVED*** id = trainingPlanEditRequest.TrainingPlanId ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanEditRequest(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequests
                .Include(t => t.TrainingPlan)
                .FirstOrDefaultAsync(m => m.TrainingPlanEditRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            ***REMOVED***
                return View(trainingPlanEditRequest);
        ***REMOVED***
            return NotFound();


    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanEditRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanEditRequestConfirmed(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            ***REMOVED***
                TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == trainingPlanEditRequest.TrainingPlanId);
                if (trainingPlan is not null)
                ***REMOVED***
                    trainingPlan.ToBeEdited = false;
            ***REMOVED***
                _context.TrainingPlanEditRequests.Remove(trainingPlanEditRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowTrainingPlanEditRequests");
        ***REMOVED***
            return RedirectToAction("Index");
    ***REMOVED***
***REMOVED***
***REMOVED***
