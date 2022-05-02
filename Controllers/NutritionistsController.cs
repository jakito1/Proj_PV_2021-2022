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
    /// NutritionistsController class, derives from Controler
    /// </summary>
    public class NutritionistsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;
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
        /// Renders a view to display all the nutritionists.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists(string? searchString, string? currentFilter, int? pageNumber)
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

            ViewData["CurrentFilter"] = searchString;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            IOrderedQueryable<Nutritionist>? nutritionists = null;

            if (string.IsNullOrEmpty(searchString))
            ***REMOVED***
                nutritionists = _context.Nutritionist.
                                    Include(a => a.UserAccountModel).
                                    Include(a => a.Gym).
                                    Include(a => a.Gym!.UserAccountModel).
                                    OrderByDescending(a => a.Gym);
        ***REMOVED***
            else
            ***REMOVED***
                nutritionists = _context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym!.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
        ***REMOVED***

            int pageSize = 3;
            return View(await PaginatedList<Nutritionist>.CreateAsync(nutritionists.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        /// <summary>
        /// Action to change the current gym status of a Nutritionist and redirect to an action.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeNutritionistGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.NutritionPlans).
                Include(a => a.UserAccountModel).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            if (nutritionist is null || nutritionist.UserAccountModel is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            nutritionist.Gym = (nutritionist.Gym is null) ? gym : null;
            if (nutritionist.Gym is null)
            ***REMOVED***
                nutritionist.Clients = null;
                nutritionist.NutritionPlans = null;
                await _interactNotification.Create("Foi removido do seu ginásio.", nutritionist.UserAccountModel);
        ***REMOVED***
            else
            ***REMOVED***
                await _interactNotification.Create($"Foi adicionado ao ginásio ***REMOVED***nutritionist.Gym.GymName***REMOVED***.", nutritionist.UserAccountModel);
        ***REMOVED***
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowNutritionists", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        /// <summary>
        /// Renders a view to display a Nutritionist's details, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> NutritionistDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Nutritionist.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.NutritionistProfilePhoto).
                FirstOrDefaultAsync(a => a.NutritionistId == id));
    ***REMOVED***

        /// <summary>
        /// Renders a view to edit a Nutritionist's settings.
        /// Only accessible to Administrator and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Nutritionist? nutritionist = await GetNutritionist(id);
            if (nutritionist is not null && nutritionist.NutritionistProfilePhoto is not null)
            ***REMOVED***
                nutritionist.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(nutritionist.UserAccountModel.UserName);
        ***REMOVED***

            if (nutritionist is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(nutritionist);
    ***REMOVED***

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
        ***REMOVED***
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionistToUpdate = await GetNutritionist(id);

            if (nutritionistToUpdate is null || nutritionistToUpdate.UserAccountModel is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Photo? oldPhoto = null;
            if (nutritionistToUpdate.NutritionistProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = nutritionistToUpdate.NutritionistProfilePhoto;
        ***REMOVED***

            nutritionistToUpdate.NutritionistProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);

            if (await TryUpdateModelAsync<Nutritionist>(nutritionistToUpdate, "",
                n => n.NutritionistFirstName!, n => n.NutritionistLastName!, n => n.NutritionistProfilePhoto!))
            ***REMOVED***
                if (oldPhoto is not null && nutritionistToUpdate.NutritionistProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (nutritionistToUpdate.NutritionistProfilePhoto is null)
                ***REMOVED***
                    nutritionistToUpdate.NutritionistProfilePhoto = oldPhoto;
            ***REMOVED***

                if (nutritionistToUpdate.NutritionistProfilePhoto is not null)
                ***REMOVED***
                    nutritionistToUpdate.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", nutritionistToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                await _context.SaveChangesAsync();
        ***REMOVED***
            return View(nutritionistToUpdate);
    ***REMOVED***


        /// <summary>
        /// Returns a query result with the found Nutritionist given de id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query result</returns>
        private async Task<Nutritionist?> GetNutritionist(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Nutritionist
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.NutritionistProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist
                .Include(a => a.UserAccountModel)
                .Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***

***REMOVED***
***REMOVED***
