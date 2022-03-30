using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainersController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;

        public TrainersController(ApplicationDbContext context,
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
        public async Task<IActionResult> ShowTrainers(string? searchString, string? currentFilter, int? pageNumber)
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

            IOrderedQueryable<Trainer>? trainers = _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                OrderByDescending(a => a.Gym);

            if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                trainers = _context.Trainer.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
        ***REMOVED***

            int pageSize = 3;
            return View(await PaginatedList<Trainer>.CreateAsync(trainers.AsNoTracking(), pageNumber ?? 1, pageSize));

    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeTrainerGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.TrainingPlans).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            trainer.Gym = (trainer.Gym is null) ? gym : null;
            if (trainer.Gym is null)
            ***REMOVED***
                trainer.Clients = null;
                trainer.TrainingPlans = null;
        ***REMOVED***
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowTrainers", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        public async Task<IActionResult> TrainerDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync());
    ***REMOVED***

        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Trainer? trainer = await GetTrainer(id);
            if (trainer is not null && trainer.TrainerProfilePhoto is not null)
            ***REMOVED***
                trainer.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
        ***REMOVED***

            if (trainer is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(trainer);
    ***REMOVED***

        [HttpPost, ActionName("EditTrainerSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettingsPost(string? id, IFormFile? formFile)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            Photo? oldPhoto = null;
            if (trainerToUpdate is not null && trainerToUpdate.TrainerProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = trainerToUpdate.TrainerProfilePhoto;
        ***REMOVED***
            if (trainerToUpdate is not null)
            ***REMOVED***
                trainerToUpdate.TrainerProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
        ***REMOVED***

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName, t => t.TrainerLastName, t => t.TrainerProfilePhoto))
            ***REMOVED***
                if (oldPhoto is not null && trainerToUpdate.TrainerProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (trainerToUpdate.TrainerProfilePhoto is null)
                ***REMOVED***
                    trainerToUpdate.TrainerProfilePhoto = oldPhoto;
            ***REMOVED***

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                if (trainerToUpdate.TrainerProfilePhoto is not null)
                ***REMOVED***
                    trainerToUpdate.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                return View(trainerToUpdate);
        ***REMOVED***
            return View(trainerToUpdate);
    ***REMOVED***

        private async Task<Trainer> GetTrainer(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***
***REMOVED***
***REMOVED***
