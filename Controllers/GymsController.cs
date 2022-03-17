using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class GymsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public GymsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
        }

        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Gym? gym = await GetGym(id);

            if (gym == null)
            {
                return NotFound();
            }
            return View(gym);
        }

        [HttpPost, ActionName("EditGymSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettingsPost(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gymToUpdate = await GetGym(id);

            if (await TryUpdateModelAsync<Gym>(gymToUpdate, "",
                g => g.GymName))
            {
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                return LocalRedirect(Url.Content("~/"));
            }
            return View(gymToUpdate);
        }

        private async Task<Gym> GetGym(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Gym.FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
