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
    /// TrainingPlanEditRequestsController class, derives from Controller
    /// </summary>
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


        /// <summary>
        /// Renders a paginated view with all the Training plan edit requests.
        /// Only accessible to Client and Trainer role.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanEditRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanEditRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client == client).Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlan.TrainingPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanEditRequests.Include(a => a.TrainingPlan).Where(a => a.Client == client).
                    Where(a => a.TrainingPlan.TrainingPlanName.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanEditRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanEditRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        /// <summary>
        /// Renders a view to edit the Training plan request, given the id.
        /// Only accessible to the Client and Trainer roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
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

        /// <summary>
        /// HTTP POST action on the API to create a new Training plan edit request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="trainingPlanEditRequest"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanEditRequest([Bind("TrainingPlanEditRequestDescription,TrainingPlanId")] TrainingPlanEditRequest trainingPlanEditRequest)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.TrainingPlans).Include(a => a.Trainer.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == trainingPlanEditRequest.TrainingPlanId);
                IQueryable<TrainingPlanEditRequest>? amountEditRequests = null;
                if (trainingPlan is not null)
                ***REMOVED***
                    amountEditRequests = _context.TrainingPlanEditRequests.Where(a => a.TrainingPlan == trainingPlan).Where(a => a.TrainingPlanEditRequestDone == false);
            ***REMOVED***

                if (client is not null && trainingPlan is not null && client.TrainingPlans.Contains(trainingPlan) &&
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

        /// <summary>
        /// Renders a view to Delete a Training plan edit request, given the id.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View request</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanEditRequest(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
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

        /// <summary>
        /// HTTP POST method on the API to delete a Training plan edit request and redirect to another page.
        /// Only accessible for the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanEditRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanEditRequestConfirmed(int id)
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
