using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists(string? email)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (email == null)
            {
                return View(_context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym));
            }

            return View(_context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(email)).
                    OrderByDescending(a => a.Gym));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveNutritionistFromGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            if(nutritionist.Gym == gym)
            {
                nutritionist.Gym = null;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
            }
          
            return LocalRedirect(Url.Content(url));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddNutritionistToGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            if(nutritionist.Gym == null)
            {
                nutritionist.Gym = gym;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
            }
           
            return LocalRedirect(Url.Content(url));
        }

        public async Task<IActionResult> NutritionistDetails(int? id)
        {
            if (id == null)
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

            if (nutritionist == null)
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

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }

    }
}
