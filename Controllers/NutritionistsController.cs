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
    /// NutritionistsController class, derives from Controler
    /// </summary>
    public class NutritionistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User manager API with Entity framework</param>
        /// <param name="inRoleByUserId">Helper interface for roles</param>
        /// <param name="photoManagement">Photo management Interface</param>
        public NutritionistsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId,
            IPhotoManagement photoManagement)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
        }

        /// <summary>
        /// Renders a view to display all the nutritionists.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>An Action result</returns>
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

        /// <summary>
        /// Action to change the current gym status of a Nutritionist and redirect to an action.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>A RedirectToAction result</returns>
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

        /// <summary>
        /// Renders a view to display a Nutritionist's details, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> NutritionistDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Nutritionist.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.NutritionistProfilePhoto).
                FirstOrDefaultAsync(a => a.NutritionistId == id));
        }

        /// <summary>
        /// Renders a view to edit a Nutritionist's settings.
        /// Only accessible to Administrator and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Nutritionist? nutritionist = await GetNutritionist(id);
            if (nutritionist is not null && nutritionist.NutritionistProfilePhoto is not null)
            {
                nutritionist.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            }

            if (nutritionist is null)
            {
                return NotFound();
            }
            return View(nutritionist);
        }

        /// <summary>
        /// HTTP POST method on the API to edit Nutritionist's settings.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formFile"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("EditNutritionistSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettingsPost(string? id, IFormFile? formFile)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionistToUpdate = await GetNutritionist(id);

            Photo? oldPhoto = null;
            if (nutritionistToUpdate is not null && nutritionistToUpdate.NutritionistProfilePhoto is not null)
            {
                oldPhoto = nutritionistToUpdate.NutritionistProfilePhoto;
            }
            if (nutritionistToUpdate is not null)
            {
                nutritionistToUpdate.NutritionistProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
            }

            if (await TryUpdateModelAsync<Nutritionist>(nutritionistToUpdate, "",
                n => n.NutritionistFirstName, n => n.NutritionistLastName, n => n.NutritionistProfilePhoto))
            {
                if (oldPhoto is not null && nutritionistToUpdate.NutritionistProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (nutritionistToUpdate.NutritionistProfilePhoto is null)
                {
                    nutritionistToUpdate.NutritionistProfilePhoto = oldPhoto;
                }

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                if (nutritionistToUpdate.NutritionistProfilePhoto is not null)
                {
                    nutritionistToUpdate.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                return View(nutritionistToUpdate);
            }
            return View(nutritionistToUpdate);
        }

        /// <summary>
        /// Returns a query result with the found Nutritionist given de id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query result</returns>
        private async Task<Nutritionist> GetNutritionist(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }

    }
}
