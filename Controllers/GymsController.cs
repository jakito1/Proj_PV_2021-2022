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
    /// GymsController class, derives from Controller
    /// </summary>
    public class GymsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context API</param>
        /// <param name="userManager">User manager API</param>
        /// <param name="inRoleByUserId">Interface for roles by user id</param>
        /// <param name="photoManagement">Photo management Interface</param>
        public GymsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId,
            IPhotoManagement photoManagement,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
            _interactNotification = interactNotification;
        }

        /// <summary>
        /// Displays the page to edit the gym settings.
        /// Only accessible to Administrator and Gym roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettings(string? id)
        {
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            {
                return BadRequest();
            }

            Gym? gym = await GetGym(id);
            if (gym is not null && gym.GymProfilePhoto is not null)
            {
                gym.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(gym.UserAccountModel.UserName);
            }

            if (gym is null)
            {
                return NotFound();
            }
            return View(gym);
        }

        /// <summary>
        /// HTTP Post method on the API to edit the gym settings.
        /// Only accessible to Administrator and Gym roles.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formFile"></param>
        /// <returns>View result</returns>
        [HttpPost, ActionName("EditGymSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettingsPost(string? id, IFormFile? formFile)
        {
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gymToUpdate = await GetGym(id);

            Photo? oldPhoto = null;

            if (gymToUpdate is null || gymToUpdate.UserAccountModel is null)
            {
                return NotFound();
            }

            if (gymToUpdate.GymProfilePhoto is not null)
            {
                oldPhoto = gymToUpdate.GymProfilePhoto;
            }
            gymToUpdate.GymProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);

            if (await TryUpdateModelAsync<Gym>(gymToUpdate, "",
                g => g.GymName!, g => g.GymProfilePhoto!))
            {
                if (oldPhoto is not null && gymToUpdate.GymProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (gymToUpdate.GymProfilePhoto is null)
                {
                    gymToUpdate.GymProfilePhoto = oldPhoto;
                }

                if (gymToUpdate.GymProfilePhoto is not null)
                {
                    gymToUpdate.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", gymToUpdate.UserAccountModel);
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                await _context.SaveChangesAsync();
                return View(gymToUpdate);
            }
            return View(gymToUpdate);
        }

        /// <summary>
        /// Get a gym by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query with the Gym details</returns>
        private async Task<Gym> GetGym(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Gym
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.GymProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Gym
                .Include(a => a.UserAccountModel)
                .Include(a => a.GymProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
