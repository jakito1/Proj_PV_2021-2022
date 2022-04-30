using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    [Authorize(Roles = "client, nutritionist")]
    /// <summary>
    /// NutritionPlansController class, derives from Controller
    /// </summary>
    public class NutritionPlansController : Controller
    ***REMOVED***
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
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
            SessionKeyMeals = "_Meals";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentNutritionist = "_CurrentNutritionist";
            SessionKeyNutritionPlanNewRequestId = "_NutritionPlanNewRequestId";
    ***REMOVED***

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
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<NutritionPlan>? plans = null;

            if (nutritionist is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Nutritionist != null && a.Nutritionist.NutritionistId == nutritionist.NutritionistId).Include(a => a.Client!.UserAccountModel);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Nutritionist != null && a.Nutritionist.NutritionistId == nutritionist.NutritionistId)
                    .Where(a => a.NutritionPlanName != null && a.NutritionPlanName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel != null && a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client!.UserAccountModel);
        ***REMOVED***
            else if (client is not null && string.IsNullOrEmpty(searchString))
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Client != null && a.Client.ClientId == client.ClientId)
                    .Where(a => a.NutritionPlanName != null && a.NutritionPlanName.Contains(searchString));
        ***REMOVED***

            if (plans is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<NutritionPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<NutritionPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***
        /// <summary>
        /// Renders a view with the details of a new Nutrition plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> NutritionPlanDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            List<Meal>? meals = await _context.Meal.Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanId == id)
                .Include(a => a.MealPhoto).ToListAsync();
            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .Include(a => a.Nutritionist!.UserAccountModel)
                .Include(a => a.Client!.UserAccountModel)
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            nutritionPlan.Meals = meals;
            return View(nutritionPlan);
    ***REMOVED***

        /// <summary>
        /// Renders a view to create a new Nutrition Plan request. 
        /// </summary>
        /// <param name="nutritionPlanNewRequestId"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> CreateNutritionPlan(int? nutritionPlanNewRequestId)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentNutritionist, nutritionist);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync());
            if (nutritionPlanNewRequestId is not null)
            ***REMOVED***
                ViewData["ClientEmail"] = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId)
                    .Select(a => a.Client!.UserAccountModel!.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyNutritionPlanNewRequestId, nutritionPlanNewRequestId);
        ***REMOVED***
            return View();
    ***REMOVED***

        /// <summary>
        /// HTTP POST action on the API to create a new Nutrition Plan.
        /// </summary>
        /// <param name="nutritionPlan"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("CreateNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanPost([Bind("NutritionPlanId,NutritionPlanName,NutritionPlanDescription,ClientEmail")] NutritionPlan nutritionPlan)
        ***REMOVED***
            if (ModelState.IsValid && User.Identity is not null)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

                int? nutritionPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyNutritionPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(nutritionPlan.ClientEmail))
                ***REMOVED***
                    clientAccount = await _userManager.FindByEmailAsync(nutritionPlan.ClientEmail);
            ***REMOVED***
                else if (nutritionPlanNewRequestId is not null)
                ***REMOVED***
                    clientAccount = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId)
                        .Select(a => a.Client!.UserAccountModel).FirstOrDefaultAsync();
            ***REMOVED***

                if (nutritionist is not null && clientAccount is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
            ***REMOVED***

                List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Clear();

                if (nutritionPlanNewRequestId is not null)
                ***REMOVED***
                    NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests
                        .FirstOrDefaultAsync(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId);
                    if (nutritionPlanNewRequest is not null)
                    ***REMOVED***
                        nutritionPlan.NutritionPlanNewRequestId = nutritionPlanNewRequestId;
                        nutritionPlanNewRequest.NutritionPlanNewRequestDone = true;
                ***REMOVED***
            ***REMOVED***

                if (client is null)
                ***REMOVED***
                    return NotFound();
            ***REMOVED***

                await _interactNotification.Create($"O seu novo plano de nutrição está pronto.", client.UserAccountModel);
                nutritionPlan.Meals = meals;
                nutritionPlan.Nutritionist = nutritionist;
                nutritionPlan.Client = client;
                _context.Add(nutritionPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowNutritionPlans");
    ***REMOVED***

        /// <summary>
        /// Renders a view to Edit a Nutrition Plan, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> EditNutritionPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FirstOrDefaultAsync(a => a.NutritionPlanId == id);
            if (nutritionPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            List<Meal>? meals = await _context.Meal.Where(a => a.NutritionPlan != null && a.NutritionPlan.NutritionPlanId == id)
                .Include(a => a.MealPhoto).ToListAsync();
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, nutritionPlan.Meals);
            return View(nutritionPlan);
    ***REMOVED***

        /// <summary>
        /// HTTP POST method on the API to Edit a Nutrition plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("EditNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNutritionPlanPost(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlanToUpdate = await _context.NutritionPlan.Include(a => a.Meals)
                .Include(a => a.Client!.UserAccountModel).FirstOrDefaultAsync(a => a.NutritionPlanId == id);

            if (nutritionPlanToUpdate is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlanEditRequest? nutritionPlanEditRequest = null;
            nutritionPlanEditRequest = await _context.NutritionPlanEditRequests.OrderByDescending(a => a.NutritionPlanEditRequestDate).
                              FirstOrDefaultAsync(a => a.NutritionPlan == nutritionPlanToUpdate);


            if (await TryUpdateModelAsync<NutritionPlan>(nutritionPlanToUpdate, "",
                u => u.NutritionPlanName!, u => u.NutritionPlanDescription!))
            ***REMOVED***
                List<Meal>? meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Remove(SessionKeyMeals);

                if (meals is not null && meals.Any() && nutritionPlanToUpdate.Meals is not null)
                ***REMOVED***
                    HashSet<int>? excludedIDs = new(meals.Select(a => a.MealId));
                    IEnumerable<Meal>? missingRows = nutritionPlanToUpdate.Meals.Where(a => !excludedIDs.Contains(a.MealId));
                    _context.Meal.RemoveRange(missingRows);
            ***REMOVED***
                nutritionPlanToUpdate.Meals = meals;
                nutritionPlanToUpdate.ToBeEdited = false;

                if (nutritionPlanEditRequest is not null && nutritionPlanToUpdate.Client is not null)
                ***REMOVED***
                    nutritionPlanEditRequest.NutritionPlanEditRequestDone = true;
                    await _interactNotification.Create($"O seu plano de nutrição foi editado com sucesso.", nutritionPlanToUpdate.Client.UserAccountModel);
            ***REMOVED***

                await _context.SaveChangesAsync();
                return RedirectToAction("ShowNutritionPlans");
        ***REMOVED***
            return View(nutritionPlanToUpdate);
    ***REMOVED***

        /// <summary>
        /// Renders a view to Delete a Nutrition plan.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> DeleteNutritionPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(nutritionPlan);
    ***REMOVED***

        /// <summary>
        /// HTTP POST action on the API to Delete a Nutrition Plan, by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [HttpPost, ActionName("DeleteNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanConfirmed(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FindAsync(id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (nutritionist is not null && nutritionPlan is not null &&
                nutritionist.NutritionPlans is not null && nutritionist.NutritionPlans.Contains(nutritionPlan))
            ***REMOVED***
                nutritionPlan.Nutritionist = null;
                await _context.SaveChangesAsync();
        ***REMOVED***

            if (client is not null && nutritionPlan is not null)
            ***REMOVED***
                _context.NutritionPlan.Remove(nutritionPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowNutritionPlans");
    ***REMOVED***

        /// <summary>
        /// Auxiliary method that confirms is a certain email belongs to a client and returns a JSON with the response.
        /// </summary>
        /// <param name="nutritionPlan"></param>
        /// <returns>A JSON result</returns>
        public async Task<IActionResult> VerifyClientEmail([Bind("ClientEmail")] NutritionPlan nutritionPlan)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Nutritionist? nutritionist = HttpContext.Session.Get<Nutritionist>(SessionKeyCurrentNutritionist);
            Client? client = null;
            if (clientsUsersAccounts is not null)
            ***REMOVED***
                client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == nutritionPlan.ClientEmail);
        ***REMOVED***

            if (clientsUsersAccounts is null || nutritionist is null)
            ***REMOVED***
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                clientsUsersAccounts = await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync();
                nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                return Json(true);
        ***REMOVED***

            return Json($"O email inserido não pertence a um dos seus clientes.");
    ***REMOVED***
***REMOVED***
***REMOVED***
