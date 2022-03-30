using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class NutritionistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public NutritionistsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            IOrderedQueryable<Nutritionist>? nutritionists = _context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym);

            if (!string.IsNullOrEmpty(searchString))
            {
                nutritionists = _context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
            }

            int pageSize = 3;
            return View(await PaginatedList<Nutritionist>.CreateAsync(nutritionists.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeNutritionistGymStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.NutritionPlans).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            nutritionist.Gym = (nutritionist.Gym is null) ? gym : null;
            if (nutritionist.Gym is null)
            {
                nutritionist.Clients = null;
                nutritionist.NutritionPlans = null;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowNutritionists", new { pageNumber, currentFilter });
        }

        public async Task<IActionResult> NutritionistDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Nutritionist.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id));
        }

        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Nutritionist? nutritionist = await GetNutritionist(id);

            if (nutritionist is null)
            {
                return NotFound();
            }
            return View(nutritionist);
        }

        [HttpPost, ActionName("EditNutritionistSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettingsPost(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionistToUpdate = await GetNutritionist(id);


            if (await TryUpdateModelAsync<Nutritionist>(nutritionistToUpdate, "",
                n => n.NutritionistFirstName, n => n.NutritionistLastName))
            {
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                return LocalRedirect(Url.Content("~/"));
            }
            return View(nutritionistToUpdate);
        }

        private async Task<Nutritionist> GetNutritionist(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Nutritionist.FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }

    }
}
