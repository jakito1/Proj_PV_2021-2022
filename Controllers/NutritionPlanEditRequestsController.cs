using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class NutritionPlanEditRequestsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        public NutritionPlanEditRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
    ***REMOVED***

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlanEditRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<NutritionPlanEditRequest>? requests = null;

            if (nutritionist is not null && nutritionist.Clients is not null)
            ***REMOVED***
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => a.NutritionPlanEditRequestDone == false);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => a.Client == client).Where(a => a.NutritionPlanEditRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && nutritionist is not null && nutritionist.Clients is not null)
            ***REMOVED***
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.NutritionPlan.NutritionPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.NutritionPlanEditRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => a.Client == client).
                    Where(a => a.NutritionPlan.NutritionPlanName.Contains(searchString)).
                    Where(a => a.NutritionPlanEditRequestDone == false);
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<NutritionPlanEditRequest>.CreateAsync(requests.OrderByDescending(a => a.NutritionPlanEditRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> NutritionPlanEditRequestDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests
                .Include(t => t.NutritionPlan)
                .FirstOrDefaultAsync(m => m.NutritionPlanEditRequestId == id);
            if (trainingPlanEditRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanEditRequest);
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanEditRequest([Bind("NutritionPlanEditRequestDescription,NutritionPlanId")] NutritionPlanEditRequest trainingPlanEditRequest)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.NutritionPlans).Include(a => a.Nutritionist.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                NutritionPlan? trainingPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == trainingPlanEditRequest.NutritionPlanId);
                IQueryable<NutritionPlanEditRequest>? amountEditRequests = null;
                if (trainingPlan is not null)
                ***REMOVED***
                    amountEditRequests = _context.NutritionPlanEditRequests.Where(a => a.NutritionPlan == trainingPlan).Where(a => a.NutritionPlanEditRequestDone == false);
            ***REMOVED***

                if (client is not null && client.Nutritionist is not null && trainingPlan is not null &&
                    client.NutritionPlans.Contains(trainingPlan) &&
                    amountEditRequests is not null && !amountEditRequests.Any())
                ***REMOVED***
                    trainingPlan.ToBeEdited = true;
                    trainingPlanEditRequest.NutritionPlanEditRequestDate = DateTime.Now;
                    trainingPlanEditRequest.Client = client;
                    _context.Add(trainingPlanEditRequest);
                    await _interactNotification.Create($"O utilizador ***REMOVED***user.UserName***REMOVED*** requisitou a edição de um plano de nutrição.", client.Nutritionist.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("NutritionPlanDetails", "NutritionPlans", new ***REMOVED*** id = trainingPlanEditRequest.NutritionPlanId ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return RedirectToAction("ShowNutritionPlans", "NutritionPlans");
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteNutritionPlanEditRequest(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests
                .Include(t => t.NutritionPlan)
                .FirstOrDefaultAsync(m => m.NutritionPlanEditRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            ***REMOVED***
                return View(trainingPlanEditRequest);
        ***REMOVED***
            return NotFound();


    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteNutritionPlanEditRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanEditRequestConfirmed(int id)
        ***REMOVED***
            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            ***REMOVED***
                NutritionPlan? trainingPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == trainingPlanEditRequest.NutritionPlanId);
                if (trainingPlan is not null)
                ***REMOVED***
                    trainingPlan.ToBeEdited = false;
            ***REMOVED***
                _context.NutritionPlanEditRequests.Remove(trainingPlanEditRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlanEditRequests");
        ***REMOVED***
            return RedirectToAction("Index");
    ***REMOVED***
***REMOVED***
***REMOVED***
