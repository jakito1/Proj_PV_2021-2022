using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class NutritionPlansController : Controller
    ***REMOVED***
        private readonly string SessionKeyMeals;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentNutritionist;
        private readonly string SessionKeyNutritionPlanNewRequestId;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public NutritionPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            SessionKeyMeals = "_Meals";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentNutritionist = "_CurrentNutritionist";
            SessionKeyNutritionPlanNewRequestId = "_NutritionPlanNewRequestId";
    ***REMOVED***

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlans(string? searchString, string? currentFilter, int? pageNumber)
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
            Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<NutritionPlan>? plans = null;

            if (nutritionist is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Nutritionist.NutritionistId == nutritionist.NutritionistId).Include(a => a.Client.UserAccountModel);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Client.ClientId == client.ClientId);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Nutritionist.NutritionistId == nutritionist.NutritionistId)
                    .Where(a => a.NutritionPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client.UserAccountModel);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                plans = _context.NutritionPlan.Where(a => a.Client.ClientId == client.ClientId).Where(a => a.NutritionPlanName.Contains(searchString));
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<NutritionPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        public async Task<IActionResult> NutritionPlanDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .Include(a => a.Meals)
                .Include(a => a.Nutritionist.UserAccountModel)
                .Include(a => a.Client.UserAccountModel)
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(nutritionPlan);
    ***REMOVED***

        public async Task<IActionResult> CreateNutritionPlan(int? nutritionPlanNewRequestId)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentNutritionist, nutritionist);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync());
            if (nutritionPlanNewRequestId is not null)
            ***REMOVED***
                ViewBag.ClientEmail = _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId).Select(a => a.Client.UserAccountModel.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyNutritionPlanNewRequestId, nutritionPlanNewRequestId);
        ***REMOVED***
            return View();
    ***REMOVED***

        [HttpPost, ActionName("CreateNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanPost([Bind("NutritionPlanId,NutritionPlanName,NutritionPlanDescription,ClientEmail")] NutritionPlan nutritionPlan)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                int? nutritionPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyNutritionPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(nutritionPlan.ClientEmail))
                ***REMOVED***
                    clientAccount = await _userManager.FindByEmailAsync(nutritionPlan.ClientEmail);
            ***REMOVED***
                else if (nutritionPlanNewRequestId is not null)
                ***REMOVED***
                    clientAccount = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId).Select(a => a.Client.UserAccountModel).FirstOrDefaultAsync();
            ***REMOVED***

                if (nutritionist is not null && clientAccount is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
            ***REMOVED***

                List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Clear();

                if (nutritionPlanNewRequestId is not null)
                ***REMOVED***
                    nutritionPlan.NutritionPlanNewRequestId = nutritionPlanNewRequestId;
                    NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests.FirstOrDefaultAsync(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId);
                    if (nutritionPlanNewRequest is not null)
                    ***REMOVED***
                        nutritionPlanNewRequest.NutritionPlanNewRequestDone = true;
                ***REMOVED***
            ***REMOVED***
                nutritionPlan.Meals = meals;
                nutritionPlan.Nutritionist = nutritionist;
                nutritionPlan.Client = client;
                _context.Add(nutritionPlan);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowNutritionPlans");
    ***REMOVED***

        public async Task<IActionResult> EditNutritionPlan(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlan = await _context.NutritionPlan.Include(a => a.Meals).FirstOrDefaultAsync(a => a.NutritionPlanId == id);
            if (nutritionPlan is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, nutritionPlan.Meals);
            return View(nutritionPlan);
    ***REMOVED***


        [HttpPost, ActionName("EditNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNutritionPlanPost(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            NutritionPlan? nutritionPlanToUpdate = await _context.NutritionPlan.Include(a => a.Meals).FirstOrDefaultAsync(a => a.NutritionPlanId == id);

            NutritionPlanEditRequest? nutritionPlanEditRequest = null;
            if (nutritionPlanToUpdate is not null)
            ***REMOVED***
                nutritionPlanEditRequest = await _context.NutritionPlanEditRequests.OrderByDescending(a => a.NutritionPlanEditRequestDate).
                    FirstOrDefaultAsync(a => a.NutritionPlan == nutritionPlanToUpdate);
        ***REMOVED***

            if (await TryUpdateModelAsync<NutritionPlan>(nutritionPlanToUpdate, "",
                u => u.NutritionPlanName, u => u.NutritionPlanDescription))
            ***REMOVED***
                List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Remove(SessionKeyMeals);

                HashSet<int>? excludedIDs = new(meals.Select(a => a.MealId));
                IEnumerable<Meal>? missingRows = nutritionPlanToUpdate.Meals.Where(a => !excludedIDs.Contains(a.MealId));

                _context.Meal.RemoveRange(missingRows);

                nutritionPlanToUpdate.Meals = meals;
                nutritionPlanToUpdate.ToBeEdited = false;

                if (nutritionPlanEditRequest is not null)
                ***REMOVED***
                    nutritionPlanEditRequest.NutritionPlanEditRequestDone = true;
            ***REMOVED***

                _context.SaveChanges();
                return RedirectToAction("ShowNutritionPlans");
        ***REMOVED***
            return View(nutritionPlanToUpdate);
    ***REMOVED***

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

        [HttpPost, ActionName("DeleteNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanConfirmed(int id)
        ***REMOVED***
            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (nutritionist is not null && nutritionPlan is not null && nutritionist.NutritionPlans.Contains(nutritionPlan))
            ***REMOVED***
                nutritionPlan.Nutritionist = null;
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowNutritionPlans");
    ***REMOVED***

        public async Task<IActionResult> VerifyClientEmail([Bind("ClientEmail")] NutritionPlan nutritionPlan)
        ***REMOVED***
            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Nutritionist? nutritionist = HttpContext.Session.Get<Nutritionist>(SessionKeyCurrentNutritionist);
            Client? client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == nutritionPlan.ClientEmail);

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

            return Json($"O email: ***REMOVED***nutritionPlan.ClientEmail***REMOVED*** não pertence a um dos seus clientes.");
    ***REMOVED***
***REMOVED***
***REMOVED***
