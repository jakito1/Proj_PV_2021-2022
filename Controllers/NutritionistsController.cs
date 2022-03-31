using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class NutritionistsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        public NutritionistsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId,
            IPhotoManagement photoManagement)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists(string? searchString, string? currentFilter, int? pageNumber)
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

            IOrderedQueryable<Nutritionist>? nutritionists = _context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym);

            if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                nutritionists = _context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
        ***REMOVED***

            int pageSize = 3;
            return View(await PaginatedList<Nutritionist>.CreateAsync(nutritionists.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeNutritionistGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.NutritionPlans).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            nutritionist.Gym = (nutritionist.Gym is null) ? gym : null;
            if (nutritionist.Gym is null)
            ***REMOVED***
                nutritionist.Clients = null;
                nutritionist.NutritionPlans = null;
        ***REMOVED***
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowNutritionists", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

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

        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Nutritionist? nutritionist = await GetNutritionist(id);
            if (nutritionist is not null && nutritionist.NutritionistProfilePhoto is not null)
            ***REMOVED***
                nutritionist.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
        ***REMOVED***

            if (nutritionist is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(nutritionist);
    ***REMOVED***

        [HttpPost, ActionName("EditNutritionistSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, nutritionist")]
        public async Task<IActionResult> EditNutritionistSettingsPost(string? id, IFormFile? formFile)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist? nutritionistToUpdate = await GetNutritionist(id);

            Photo? oldPhoto = null;
            if (nutritionistToUpdate is not null && nutritionistToUpdate.NutritionistProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = nutritionistToUpdate.NutritionistProfilePhoto;
        ***REMOVED***
            if (nutritionistToUpdate is not null)
            ***REMOVED***
                nutritionistToUpdate.NutritionistProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
        ***REMOVED***

            if (await TryUpdateModelAsync<Nutritionist>(nutritionistToUpdate, "",
                n => n.NutritionistFirstName, n => n.NutritionistLastName, n => n.NutritionistProfilePhoto))
            ***REMOVED***
                if (oldPhoto is not null && nutritionistToUpdate.NutritionistProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (nutritionistToUpdate.NutritionistProfilePhoto is null)
                ***REMOVED***
                    nutritionistToUpdate.NutritionistProfilePhoto = oldPhoto;
            ***REMOVED***

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                if (nutritionistToUpdate.NutritionistProfilePhoto is not null)
                ***REMOVED***
                    nutritionistToUpdate.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                return View(nutritionistToUpdate);
        ***REMOVED***
            return View(nutritionistToUpdate);
    ***REMOVED***

        private async Task<Nutritionist> GetNutritionist(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***

***REMOVED***
***REMOVED***
