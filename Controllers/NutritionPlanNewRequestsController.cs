using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// NutritionPlanNewRequestsController class, derives from Controller
    /// </summary>
    public class NutritionPlanNewRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public NutritionPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
        }
        
        /// <summary>
        /// Renders a paginated view to display all the new Nutrition Plan Requests.
        /// Only accessible to Client and Nutritionist roles.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlanNewRequests(string? searchString, string? currentFilter, int? pageNumber)
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

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Clients).Include(a => a.NutritionPlans)
                .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.NutritionPlans)
                .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<NutritionPlanNewRequest>? requests = null;

            if (nutritionist is not null && nutritionist.Clients is not null && string.IsNullOrEmpty(searchString))
            {
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId))
                    .Where(a => a.NutritionPlanNewRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null && nutritionist.Clients is not null)
            {
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.NutritionPlanNewRequestName != null && a.NutritionPlanNewRequestName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel != null && a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.NutritionPlanNewRequestDone == false);
            }
            else if (client is not null && string.IsNullOrEmpty(searchString))
            {
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client == client).Where(a => a.NutritionPlanNewRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.NutritionPlanNewRequests.Where(a => a.Client == client)
                    .Where(a => a.NutritionPlanNewRequestName != null && a.NutritionPlanNewRequestName.Contains(searchString)).
                    Where(a => a.NutritionPlanNewRequestDone == false);
            }

            if (requests is not null)
            {
                int pageSize = 5;
                return View(await PaginatedList<NutritionPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.NutritionPlanNewRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }

        /// <summary>
        /// Renders a view with the details of a new Nutrition plan request.
        /// Only accessible to client and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> NutritionPlanNewRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NutritionPlanNewRequest? nutritionPlanNewRequests = await _context.NutritionPlanNewRequests
                .FirstOrDefaultAsync(m => m.NutritionPlanNewRequestId == id);
            if (nutritionPlanNewRequests == null)
            {
                return NotFound();
            }

            return View(nutritionPlanNewRequests);
        }

        /// <summary>
        /// Renders a view to create a new Nutrition Plan request.
        /// Only accessible to Client role.
        /// </summary>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        public IActionResult CreateNutritionPlanNewRequest()
        {
            return View();
        }

        /// <summary>
        /// HTTP POST action on the API to create a new Nutrition Plan request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="nutritionPlanNewRequest"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("CreateNutritionPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanNewRequestPost([Bind("NutritionPlanNewRequestId,NutritionPlanNewRequestName, NutritionPlanNewRequestDescription")]
            NutritionPlanNewRequest nutritionPlanNewRequest)
        {
            if (ModelState.IsValid && User.Identity is not null)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client
                    .Include(a => a.Nutritionist!.UserAccountModel)
                    .FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null && client.Nutritionist is not null && client.Nutritionist.UserAccountModel is not null)
                {
                    await _interactNotification.Create($"O utilizador {user.UserName} requisitou um novo plano de nutrição.", client.Nutritionist.UserAccountModel);
                    nutritionPlanNewRequest.Client = client;
                    nutritionPlanNewRequest.NutritionPlanNewRequestDate = DateTime.Now;
                    _context.Add(nutritionPlanNewRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowNutritionPlans", "NutritionPlans");
                }
            }
            return View(nutritionPlanNewRequest);
        }

        /// <summary>
        /// Renders a view to delete a new Nutrition Plan request.
        /// Only accesible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteNutritionPlanNewRequest(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return NotFound();
            }

            NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests
                .FirstOrDefaultAsync(m => m.NutritionPlanNewRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (nutritionPlanNewRequest is not null && client is not null && nutritionPlanNewRequest.Client == client)
            {
                return View(nutritionPlanNewRequest);
            }
            return NotFound();
        }

        /// <summary>
        /// HTTP POST action on the API to delete a new Nutrition Plan request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteNutritionPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanNewRequestConfirmed(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
            NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (nutritionPlanNewRequest is not null && client is not null && nutritionPlanNewRequest.Client == client)
            {
                _context.NutritionPlanNewRequests.Remove(nutritionPlanNewRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlanNewRequests");
            }
            return RedirectToAction("Index");
        }
    }
}
