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
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;
        public NutritionistsController(ApplicationDbContext context,
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
                Include(a => a.UserAccountModel).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            nutritionist.Gym = (nutritionist.Gym is null) ? gym : null;
            if (nutritionist.Gym is null)
            {
                nutritionist.Clients = null;
                nutritionist.NutritionPlans = null;
                await _interactNotification.Create("Foi removido do seu ginásio.", nutritionist.UserAccountModel);
            }
            else
            {
                await _interactNotification.Create($"Foi adicionado ao ginásio {nutritionist.Gym.GymName}.", nutritionist.UserAccountModel);
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
                Include(a => a.NutritionistProfilePhoto).
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

                if (nutritionistToUpdate.NutritionistProfilePhoto is not null)
                {
                    nutritionistToUpdate.NutritionistProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", nutritionistToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                await _context.SaveChangesAsync();
            }
            return View(nutritionistToUpdate);
        }

        private async Task<Nutritionist> GetNutritionist(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Nutritionist
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.NutritionistProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Nutritionist
                .Include(a => a.UserAccountModel)
                .Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }

    }
}
