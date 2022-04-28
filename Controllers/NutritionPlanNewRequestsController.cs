using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class NutritionPlanNewRequestsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        public NutritionPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
    ***REMOVED***

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlanNewRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).Include(a => a.NutritionPlans)
                .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.NutritionPlans)
                .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<NutritionPlanNewRequest>? requests = null;

            if (nutritionist is not null && nutritionist.Clients is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId))
                    .Where(a => a.NutritionPlanNewRequestDone == false);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null && nutritionist.Clients is not null)
            ***REMOVED***
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.NutritionPlanNewRequestName != null && a.NutritionPlanNewRequestName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel != null && a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.NutritionPlanNewRequestDone == false);
        ***REMOVED***
            else if (client is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client == client).Where(a => a.NutritionPlanNewRequestDone == false);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client == client)
                    .Where(a => a.NutritionPlanNewRequestName != null && a.NutritionPlanNewRequestName.Contains(searchString)).
                    Where(a => a.NutritionPlanNewRequestDone == false);
        ***REMOVED***

            if (requests is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<NutritionPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.NutritionPlanNewRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> NutritionPlanNewRequestDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlanNewRequest? nutritionPlanNewRequests = await _context.NutritionPlanNewRequests
                .FirstOrDefaultAsync(m => m.NutritionPlanNewRequestId == id);
            if (nutritionPlanNewRequests == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(nutritionPlanNewRequests);
    ***REMOVED***

        [Authorize(Roles = "client")]
        public IActionResult CreateNutritionPlanNewRequest()
        ***REMOVED***
            return View();
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("CreateNutritionPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanNewRequestPost([Bind("NutritionPlanNewRequestId,NutritionPlanNewRequestName, NutritionPlanNewRequestDescription")]
            NutritionPlanNewRequest nutritionPlanNewRequest)
        ***REMOVED***
            if (ModelState.IsValid && User.Identity is not null)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client
                    .Include(a => a.Nutritionist!.UserAccountModel)
                    .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null && client.Nutritionist is not null && client.Nutritionist.UserAccountModel is not null)
                ***REMOVED***
                    await _interactNotification.Create($"O utilizador ***REMOVED***user.UserName***REMOVED*** requisitou um novo plano de nutrição.", client.Nutritionist.UserAccountModel);
                    nutritionPlanNewRequest.Client = client;
                    nutritionPlanNewRequest.NutritionPlanNewRequestDate = DateTime.Now;
                    _context.Add(nutritionPlanNewRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowNutritionPlans", "NutritionPlans");
            ***REMOVED***
        ***REMOVED***
            return View(nutritionPlanNewRequest);
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteNutritionPlanNewRequest(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests
                .FirstOrDefaultAsync(m => m.NutritionPlanNewRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (nutritionPlanNewRequest is not null && client is not null && nutritionPlanNewRequest.Client == client)
            ***REMOVED***
                return View(nutritionPlanNewRequest);
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteNutritionPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanNewRequestConfirmed(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (nutritionPlanNewRequest is not null && client is not null && nutritionPlanNewRequest.Client == client)
            ***REMOVED***
                _context.NutritionPlanNewRequests.Remove(nutritionPlanNewRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlanNewRequests");
        ***REMOVED***
            return RedirectToAction("Index");
    ***REMOVED***
***REMOVED***
***REMOVED***
