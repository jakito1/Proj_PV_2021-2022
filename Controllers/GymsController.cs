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
        private readonly IPhotoManagement _photoManagement;

        public GymsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId,
            IPhotoManagement photoManagement)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
        }

        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Gym? gym = await GetGym(id);
            if (gym is not null && gym.GymProfilePhoto is not null)
            {
                gym.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            }

            if (gym is null)
            {
                return NotFound();
            }
            return View(gym);
        }

        [HttpPost, ActionName("EditGymSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettingsPost(string? id, IFormFile? formFile)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gymToUpdate = await GetGym(id);

            Photo? oldPhoto = null;
            if (gymToUpdate is not null && gymToUpdate.GymProfilePhoto is not null)
            {
                oldPhoto = gymToUpdate.GymProfilePhoto;
            }
            if (gymToUpdate is not null)
            {
                gymToUpdate.GymProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
            }

            if (await TryUpdateModelAsync<Gym>(gymToUpdate, "",
                g => g.GymName, g => g.GymProfilePhoto))
            {
                if (oldPhoto is not null && gymToUpdate.GymProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (gymToUpdate.GymProfilePhoto is null)
                {
                    gymToUpdate.GymProfilePhoto = oldPhoto;
                }

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                if (gymToUpdate.GymProfilePhoto is not null)
                {
                    gymToUpdate.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                return View(gymToUpdate);
            }
            return View(gymToUpdate);
        }

        private async Task<Gym> GetGym(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Gym.Include(a => a.GymProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Gym.Include(a => a.GymProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
