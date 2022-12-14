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
    /// NutritionPlanEditRequestsController class, derives from Controller
    /// </summary>
    public class NutritionPlanEditRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User manager API with Entity framework</param>
        public NutritionPlanEditRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
        }

        /// <summary>
        /// Renders a paginated view with all the Nutrition plan edit requests.
        /// Only accessible to Client and Nutritionist role.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlanEditRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            Client? client = await _context.Client.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<NutritionPlanEditRequest>? requests = null;

            if (nutritionist is not null && nutritionist.Clients is not null && string.IsNullOrEmpty(searchString))
            {
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan)
                    .Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).Where(a => a.NutritionPlanEditRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null && nutritionist.Clients is not null)
            {
                clientIDs = new(nutritionist.Clients.Select(a => a.ClientId));
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanName != null && a.NutritionPlan.NutritionPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel != null && a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.NutritionPlanEditRequestDone == false);
            }
            else if (client is not null && string.IsNullOrEmpty(searchString))
            {
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => a.Client == client).Where(a => a.NutritionPlanEditRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.NutritionPlanEditRequests.Include(a => a.NutritionPlan).Where(a => a.Client == client).
                    Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanName != null && a.NutritionPlan.NutritionPlanName.Contains(searchString)).
                    Where(a => a.NutritionPlanEditRequestDone == false);
            }

            if (requests is not null)
            {
                int pageSize = 5;
                return View(await PaginatedList<NutritionPlanEditRequest>.CreateAsync(requests.OrderByDescending(a => a.NutritionPlanEditRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }

        /// <summary>
        /// Renders a view to edit the nutrition plan request, given the id.
        /// Only accessible to the Client and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> NutritionPlanEditRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests
                .Include(t => t.NutritionPlan)
                .FirstOrDefaultAsync(m => m.NutritionPlanEditRequestId == id);
            if (trainingPlanEditRequest == null)
            {
                return NotFound();
            }

            return View(trainingPlanEditRequest);
        }

        /// <summary>
        /// HTTP POST action on the API to create a new Nutrition plan edit request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="trainingPlanEditRequest"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanEditRequest([Bind("NutritionPlanEditRequestDescription,NutritionPlanId")] NutritionPlanEditRequest trainingPlanEditRequest)
        {
            if (ModelState.IsValid && User.Identity is not null)
            {
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.NutritionPlans)
                    .Include(a => a.Nutritionist!.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                NutritionPlan? trainingPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == trainingPlanEditRequest.NutritionPlanId);
                IQueryable<NutritionPlanEditRequest>? amountEditRequests = null;
                if (trainingPlan is not null)
                {
                    amountEditRequests = _context.NutritionPlanEditRequests.Where(a => a.NutritionPlan == trainingPlan).Where(a => a.NutritionPlanEditRequestDone == false);
                }

                if (client is not null && client.Nutritionist is not null && client.Nutritionist.UserAccountModel is not null && trainingPlan is not null &&
                    client.NutritionPlans is not null && client.NutritionPlans.Contains(trainingPlan) &&
                    amountEditRequests is not null && !amountEditRequests.Any())
                {
                    trainingPlan.ToBeEdited = true;
                    trainingPlanEditRequest.NutritionPlanEditRequestDate = DateTime.Now;
                    trainingPlanEditRequest.Client = client;
                    _context.Add(trainingPlanEditRequest);
                    await _interactNotification.Create($"O utilizador {user.UserName} requisitou a edição de um plano de nutrição.", client.Nutritionist.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("NutritionPlanDetails", "NutritionPlans", new { id = trainingPlanEditRequest.NutritionPlanId });
                }
            }
            return RedirectToAction("ShowNutritionPlans", "NutritionPlans");
        }

        /// <summary>
        /// Renders a view to Delete a Nutrition plan edit request, given the id.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View request</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteNutritionPlanEditRequest(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }

            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests
                .Include(t => t.NutritionPlan)
                .FirstOrDefaultAsync(m => m.NutritionPlanEditRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            {
                return View(trainingPlanEditRequest);
            }
            return NotFound();


        }

        /// <summary>
        /// HTTP POST method on the API to delete a nutrition plan edit request and redirect to another page.
        /// Only accessible for the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteNutritionPlanEditRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanEditRequestConfirmed(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
            NutritionPlanEditRequest? trainingPlanEditRequest = await _context.NutritionPlanEditRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            {
                NutritionPlan? trainingPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == trainingPlanEditRequest.NutritionPlanId);
                if (trainingPlan is not null)
                {
                    trainingPlan.ToBeEdited = false;
                }
                _context.NutritionPlanEditRequests.Remove(trainingPlanEditRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlanEditRequests");
            }
            return RedirectToAction("Index");
        }
    }
}
