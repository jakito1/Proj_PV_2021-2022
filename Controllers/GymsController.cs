using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// GymsController class, derives from Controller
    /// </summary>
    public class GymsController : Controller
    ***REMOVED***
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
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
            _interactNotification = interactNotification;
    ***REMOVED***

        /// <summary>
        /// Displays the page to edit the gym settings.
        /// Only accessible to Administrator and Gym roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "administrator, gym")]
        public async Task<IActionResult> EditGymSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Gym? gym = await GetGym(id);
            if (gym is not null && gym.GymProfilePhoto is not null)
            ***REMOVED***
                gym.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
        ***REMOVED***

            if (gym is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(gym);
    ***REMOVED***

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
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gymToUpdate = await GetGym(id);

            Photo? oldPhoto = null;
            if (gymToUpdate is not null && gymToUpdate.GymProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = gymToUpdate.GymProfilePhoto;
        ***REMOVED***
            if (gymToUpdate is not null)
            ***REMOVED***
                gymToUpdate.GymProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
        ***REMOVED***

            if (await TryUpdateModelAsync<Gym>(gymToUpdate, "",
                g => g.GymName, g => g.GymProfilePhoto))
            ***REMOVED***
                if (oldPhoto is not null && gymToUpdate.GymProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (gymToUpdate.GymProfilePhoto is null)
                ***REMOVED***
                    gymToUpdate.GymProfilePhoto = oldPhoto;
            ***REMOVED***

                if (gymToUpdate.GymProfilePhoto is not null)
                ***REMOVED***
                    gymToUpdate.GymProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", gymToUpdate.UserAccountModel);
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                await _context.SaveChangesAsync();
                return View(gymToUpdate);
        ***REMOVED***
            return View(gymToUpdate);
    ***REMOVED***

        /// <summary>
        /// Get a gym by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query with the Gym details</returns>
        private async Task<Gym> GetGym(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Gym
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.GymProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Gym
                .Include(a => a.UserAccountModel)
                .Include(a => a.GymProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***
***REMOVED***
***REMOVED***
