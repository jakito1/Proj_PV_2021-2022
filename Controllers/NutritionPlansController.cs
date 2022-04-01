using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class NutritionPlansController : Controller
    {
        private readonly string SessionKeyMeals;
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentNutritionist;
        private readonly string SessionKeyNutritionPlanNewRequestId;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public NutritionPlansController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
            SessionKeyMeals = "_Meals";
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentNutritionist = "_CurrentNutritionist";
            SessionKeyNutritionPlanNewRequestId = "_NutritionPlanNewRequestId";
        }

        [Authorize(Roles = "client, nutritionist")]
        public async Task<IActionResult> ShowNutritionPlans(string? searchString, string? currentFilter, int? pageNumber)
        {

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
            Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<NutritionPlan>? plans = null;

            if (nutritionist is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Nutritionist.NutritionistId == nutritionist.NutritionistId).Include(a => a.Client.UserAccountModel);
            }

            if (client is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Client.ClientId == client.ClientId);
            }

            if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Nutritionist.NutritionistId == nutritionist.NutritionistId)
                    .Where(a => a.NutritionPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString))
                    .Include(a => a.Client.UserAccountModel);
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                plans = _context.NutritionPlan.Where(a => a.Client.ClientId == client.ClientId).Where(a => a.NutritionPlanName.Contains(searchString));
            }

            int pageSize = 5;
            return View(await PaginatedList<NutritionPlan>.CreateAsync(plans.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> NutritionPlanDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlan = await _context.NutritionPlan
                .Include(a => a.Meals)
                .Include(a => a.Nutritionist.UserAccountModel)
                .Include(a => a.Client.UserAccountModel)
                .FirstOrDefaultAsync(m => m.NutritionPlanId == id);
            if (nutritionPlan is null)
            {
                return NotFound();
            }

            return View(nutritionPlan);
        }

        public async Task<IActionResult> CreateNutritionPlan(int? nutritionPlanNewRequestId)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            HttpContext.Session.Set(SessionKeyCurrentNutritionist, nutritionist);
            HttpContext.Session.Set(SessionKeyClientsUserAccounts,
                await _context.Client.Where(a => a.Nutritionist == nutritionist).Include(a => a.UserAccountModel).ToListAsync());
            if (nutritionPlanNewRequestId is not null)
            {
                ViewBag.ClientEmail = _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId).Select(a => a.Client.UserAccountModel.Email).FirstOrDefaultAsync();
                HttpContext.Session.Set(SessionKeyNutritionPlanNewRequestId, nutritionPlanNewRequestId);
            }
            return View();
        }

        [HttpPost, ActionName("CreateNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNutritionPlanPost([Bind("NutritionPlanId,NutritionPlanName,NutritionPlanDescription,ClientEmail")] NutritionPlan nutritionPlan)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                int? nutritionPlanNewRequestId = HttpContext.Session.Get<int?>(SessionKeyNutritionPlanNewRequestId);

                UserAccountModel? clientAccount = null;

                if (!string.IsNullOrEmpty(nutritionPlan.ClientEmail))
                {
                    clientAccount = await _userManager.FindByEmailAsync(nutritionPlan.ClientEmail);
                }
                else if (nutritionPlanNewRequestId is not null)
                {
                    clientAccount = await _context.NutritionPlanNewRequests.Where(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId).Select(a => a.Client.UserAccountModel).FirstOrDefaultAsync();
                }

                if (nutritionist is not null && clientAccount is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == clientAccount);
                }

                List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Clear();

                if (nutritionPlanNewRequestId is not null)
                {
                    nutritionPlan.NutritionPlanNewRequestId = nutritionPlanNewRequestId;
                    NutritionPlanNewRequest? nutritionPlanNewRequest = await _context.NutritionPlanNewRequests.FirstOrDefaultAsync(a => a.NutritionPlanNewRequestId == nutritionPlanNewRequestId);
                    if (nutritionPlanNewRequest is not null)
                    {
                        nutritionPlanNewRequest.NutritionPlanNewRequestDone = true;
                    }
                }
                nutritionPlan.Meals = meals;
                nutritionPlan.Nutritionist = nutritionist;
                nutritionPlan.Client = client;
                _context.Add(nutritionPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowNutritionPlans");
        }

        public async Task<IActionResult> EditNutritionPlan(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlan = await _context.NutritionPlan.Include(a => a.Meals).FirstOrDefaultAsync(a => a.NutritionPlanId == id);
            if (nutritionPlan is null)
            {
                return NotFound();
            }
            HttpContext.Session.Set<List<Meal>>(SessionKeyMeals, nutritionPlan.Meals);
            return View(nutritionPlan);
        }


        [HttpPost, ActionName("EditNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNutritionPlanPost(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            NutritionPlan? nutritionPlanToUpdate = await _context.NutritionPlan.Include(a => a.Meals).FirstOrDefaultAsync(a => a.NutritionPlanId == id);

            NutritionPlanEditRequest? nutritionPlanEditRequest = null;
            if (nutritionPlanToUpdate is not null)
            {
                nutritionPlanEditRequest = await _context.NutritionPlanEditRequests.OrderByDescending(a => a.NutritionPlanEditRequestDate).
                    FirstOrDefaultAsync(a => a.NutritionPlan == nutritionPlanToUpdate);
            }

            if (await TryUpdateModelAsync<NutritionPlan>(nutritionPlanToUpdate, "",
                u => u.NutritionPlanName, u => u.NutritionPlanDescription))
            {
                List<Meal> meals = HttpContext.Session.Get<List<Meal>>(SessionKeyMeals);
                HttpContext.Session.Remove(SessionKeyMeals);

                HashSet<int>? excludedIDs = new(meals.Select(a => a.MealId));
                IEnumerable<Meal>? missingRows = nutritionPlanToUpdate.Meals.Where(a => !excludedIDs.Contains(a.MealId));

                _context.Meal.RemoveRange(missingRows);

                nutritionPlanToUpdate.Meals = meals;
                nutritionPlanToUpdate.ToBeEdited = false;

                if (nutritionPlanEditRequest is not null)
                {
                    nutritionPlanEditRequest.NutritionPlanEditRequestDone = true;
                }

                _context.SaveChanges();
                return RedirectToAction("ShowNutritionPlans");
            }
            return View(nutritionPlanToUpdate);
        }

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

        [HttpPost, ActionName("DeleteNutritionPlan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNutritionPlanConfirmed(int id)
        {
            NutritionPlan? nutritionPlan = await _context.NutritionPlan.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.NutritionPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (nutritionist is not null && nutritionPlan is not null && nutritionist.NutritionPlans.Contains(nutritionPlan))
            {
                nutritionPlan.Nutritionist = null;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowNutritionPlans");
        }

        public async Task<IActionResult> VerifyClientEmail([Bind("ClientEmail")] NutritionPlan nutritionPlan)
        {
            List<Client>? clientsUsersAccounts = HttpContext.Session.Get<List<Client>>(SessionKeyClientsUserAccounts);
            Nutritionist? nutritionist = HttpContext.Session.Get<Nutritionist>(SessionKeyCurrentNutritionist);
            Client? client = clientsUsersAccounts.Find(a => a.UserAccountModel.Email == nutritionPlan.ClientEmail);

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

            return Json($"O email: {nutritionPlan.ClientEmail} não pertence a um dos seus clientes.");
        }
    }
}
