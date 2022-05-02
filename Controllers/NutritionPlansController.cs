using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    [Authorize(Roles = "client, nutritionist")]
    /// <summary>
    /// NutritionPlansController class, derives from Controller
    /// </summary>
    public class NutritionPlansController : Controller
    {
        private readonly string SessionKeyMeals;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentNutritionist;
        private readonly string SessionKeyNutritionPlanNewRequestId;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User manager API from Entity framework</param>
        public NutritionPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
            SessionKeyMeals = "_Meals";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentNutritionist = "_CurrentNutritionist";
            SessionKeyNutritionPlanNewRequestId = "_NutritionPlanNewRequestId";
        }

        /// <summary>
        /// Renders a paginated view to display all the new Nutrition Plans.
        /// Only accessible to Client and Nutritionist roles.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlans(string? searchString, string? currentFilter, int? pageNumber)
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
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<NutritionPlan>? plans = null;

            if (nutritionist is not null && string.IsNullOrEmpty(searchString))
            {
                plans = _context.NutritionPlan.Where(a => a.Nutritionist != null && a.Nutritionist.NutritionistId == nutritionist.NutritionistId).Include(a => a.Client!.UserAccountModel);
            }
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Nutritionist != null && a.Nutritionist.NutritionistId == nutritionist.NutritionistId)
                    .Where(a => a.NutritionPlanName != null && a.NutritionPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel != null && a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client!.UserAccountModel);
            }
            else if (client is not null && string.IsNullOrEmpty(searchString))
            {
                plans = _context.NutritionPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId);
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId)
                    .Where(a => a.NutritionPlanName != null && a.NutritionPlanName.Contains(searchString));
            }

            if (plans is not null)
            {
                int pageSize = 5;
                return View(await PaginatedList<NutritionPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }
        /// <summary>
        /// Renders a view with the details of a new Nutrition plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> NutritionPlanDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            List<Meal>? meals = await _context.Meal.Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanId == id)
                .Include(a => a.MealPhoto).ToListAsync();
            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .Include(a => a.Nutritionist!.UserAccountModel)
                .Include(a => a.Client!.UserAccountModel)
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            {
                return NotFound();
            }
            nutritionPlan.Meals = meals;
            return View(nutritionPlan);
        }

        /// <summary>
        /// Renders a view to create a new Nutrition Plan request. 
        /// </summary>
        /// <param name="nutritionPlanNewRequestId"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> CreateNutritionPlan(int? nutritionPlanNewRequestId)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentNutritionist, nutritionist);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync());
            if (nutritionPlanNewRequestId is not null)
            {
                ViewData["ClientEmail"] = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId)
                    .Select(a => a.Client!.UserAccountModel!.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyNutritionPlanNewRequestId, nutritionPlanNewRequestId);
            }
            return View();
        }

        /// <summary>
        /// HTTP POST action on the API to create a new Nutrition Plan.
        /// </summary>
        /// <param name="nutritionPlan"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("CreateNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanPost([Bind("NutritionPlanId,NutritionPlanName,NutritionPlanDescription,ClientEmail")] NutritionPlan nutritionPlan)
        {
            if (ModelState.IsValid && User.Identity is not null)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

                int? nutritionPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyNutritionPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(nutritionPlan.ClientEmail))
                {
                    clientAccount = await _userManager.FindByEmailAsync(nutritionPlan.ClientEmail);
                }
                else if (nutritionPlanNewRequestId is not null)
                {
                    clientAccount = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId)
                        .Select(a => a.Client!.UserAccountModel).FirstOrDefaultAsync();
                }

                if (nutritionist is not null && clientAccount is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
                }

                List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Clear();

                if (nutritionPlanNewRequestId is not null)
                {
                    NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests
                        .FirstOrDefaultAsync(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId);
                    if (nutritionPlanNewRequest is not null)
                    {
                        nutritionPlan.NutritionPlanNewRequestId = nutritionPlanNewRequestId;
                        nutritionPlanNewRequest.NutritionPlanNewRequestDone = true;
                    }
                }

                if (client is null)
                {
                    return NotFound();
                }

                await _interactNotification.Create($"O seu novo plano de nutrição está pronto.", client.UserAccountModel);
                nutritionPlan.Meals = meals;
                nutritionPlan.Nutritionist = nutritionist;
                nutritionPlan.Client = client;
                _context.Add(nutritionPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowNutritionPlans");
        }

        /// <summary>
        /// Renders a view to Edit a Nutrition Plan, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> EditNutritionPlan(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == id);
            if (nutritionPlan is null)
            {
                return NotFound();
            }
            List<Meal>? meals = await _context.Meal.Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanId == id)
                .Include(a => a.MealPhoto).ToListAsync();
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, nutritionPlan.Meals);
            return View(nutritionPlan);
        }

        /// <summary>
        /// HTTP POST method on the API to Edit a Nutrition plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("EditNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNutritionPlanPost(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlanToUpdate = await _context.NutritionPlan.Include(a => a.Meals)
                .Include(a => a.Client!.UserAccountModel).FirstOrDefaultAsync(a => a.NutritionPlanId == id);

            if (nutritionPlanToUpdate is null)
            {
                return NotFound();
            }

            NutritionPlanEditRequest? nutritionPlanEditRequest = null;
            nutritionPlanEditRequest = await _context.NutritionPlanEditRequests.OrderByDescending(a => a.NutritionPlanEditRequestDate).
                              FirstOrDefaultAsync(a => a.NutritionPlan == nutritionPlanToUpdate);


            if (await TryUpdateModelAsync<NutritionPlan>(nutritionPlanToUpdate, "",
                u => u.NutritionPlanName!, u => u.NutritionPlanDescription!))
            {
                List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Remove(SessionKeyMeals);

                if (meals is not null && meals.Any() && nutritionPlanToUpdate.Meals is not null)
                {
                    HashSet<int>? excludedIDs = new(meals.Select(a => a.MealId));
                    IEnumerable<Meal>? missingRows = nutritionPlanToUpdate.Meals.Where(a => !excludedIDs.Contains(a.MealId));
                    _context.Meal.RemoveRange(missingRows);
                }
                nutritionPlanToUpdate.Meals = meals;
                nutritionPlanToUpdate.ToBeEdited = false;

                if (nutritionPlanEditRequest is not null && nutritionPlanToUpdate.Client is not null)
                {
                    nutritionPlanEditRequest.NutritionPlanEditRequestDone = true;
                    await _interactNotification.Create($"O seu plano de nutrição foi editado com sucesso.", nutritionPlanToUpdate.Client.UserAccountModel);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlans");
            }
            return View(nutritionPlanToUpdate);
        }

        /// <summary>
        /// Renders a view to Delete a Nutrition plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> DeleteNutritionPlan(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            {
                return NotFound();
            }

            return View(nutritionPlan);
        }

        /// <summary>
        /// HTTP POST action on the API to Delete a Nutrition Plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [HttpPost, ActionName("DeleteNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanConfirmed(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FindAsync(id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (nutritionist is not null && nutritionPlan is not null &&
                nutritionist.NutritionPlans is not null && nutritionist.NutritionPlans.Contains(nutritionPlan))
            {
                nutritionPlan.Nutritionist = null;
                await _context.SaveChangesAsync();
            }

            if (client is not null && nutritionPlan is not null)
            {
                _context.NutritionPlan.Remove(nutritionPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowNutritionPlans");
        }

        public async Task<IActionResult> VerifyClientEmail(string? clientEmail)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }

            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Nutritionist? nutritionist = HttpContext.Session.Get<Nutritionist>(SessionKeyCurrentNutritionist);
            Client? client = null;
            if (clientsUsersAccounts is not null)
            {
                client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == clientEmail);
            }

            if (clientsUsersAccounts is null || nutritionist is null)
            {
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                clientsUsersAccounts = await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync();
                nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            }

            if (client is not null)
            {
                return Json(true);
            }

            return Json($"O email inserido não pertence a um dos seus clientes.");
        }
    }
}
