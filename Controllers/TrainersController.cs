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
    /// TrainersController class, derives from Controller
    /// </summary>
    public class TrainersController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="inRoleByUserId"></param>
        /// <param name="photoManagement"></param>
        public TrainersController(ApplicationDbContext context,
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
        /// Renders a view to display all the Trainers.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowTrainers(string? searchString, string? currentFilter, int? pageNumber)
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
            IOrderedQueryable<Trainer>? trainers = null;

            if (string.IsNullOrEmpty(searchString))
            ***REMOVED***
                trainers = _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym!.UserAccountModel).
                OrderByDescending(a => a.Gym);
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                trainers = _context.Trainer.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym!.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
        ***REMOVED***

            if (trainers is not null)
            ***REMOVED***
                int pageSize = 3;
                return View(await PaginatedList<Trainer>.CreateAsync(trainers.AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***


        /// <summary>
        /// Action to change the current gym status of a Trainer and redirect to an action.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeTrainerGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.TrainingPlans).
                Include(a => a.UserAccountModel).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            if (trainer is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            trainer.Gym = (trainer.Gym is null) ? gym : null;
            if (trainer.Gym is null)
            ***REMOVED***
                trainer.Clients = null;
                trainer.TrainingPlans = null;
                await _interactNotification.Create("Foi removido do seu ginásio.", trainer.UserAccountModel);
        ***REMOVED***
            else
            ***REMOVED***
                await _interactNotification.Create($"Foi adicionado ao ginásio ***REMOVED***trainer.Gym.GymName***REMOVED***.", trainer.UserAccountModel);
        ***REMOVED***
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowTrainers", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        /// <summary>
        /// Renders a view to display a Trainer's details, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> TrainerDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.TrainerProfilePhoto).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync());
    ***REMOVED***

        /// <summary>
        /// Renders a view to edit a Trainer's settings.
        /// Only accessible to Administrator and Trainer roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id) || User.Identity is null)
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

        /// <summary>
        /// HTTP POST method on the API to edit Trainer's settings.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formFile"></param>
        /// <returns>A View result</returns>
        [HttpPost, ActionName("EditTrainerSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettingsPost(string? id, IFormFile? formFile)
        ***REMOVED***
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            if (trainerToUpdate is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Photo? oldPhoto = null;
            if (trainerToUpdate.TrainerProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = trainerToUpdate.TrainerProfilePhoto;
        ***REMOVED***
            trainerToUpdate.TrainerProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName!, t => t.TrainerLastName!, t => t.TrainerProfilePhoto!))
            ***REMOVED***
                if (oldPhoto is not null && trainerToUpdate.TrainerProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (trainerToUpdate.TrainerProfilePhoto is null)
                ***REMOVED***
                    trainerToUpdate.TrainerProfilePhoto = oldPhoto;
            ***REMOVED***

                if (trainerToUpdate.TrainerProfilePhoto is not null)
                ***REMOVED***
                    trainerToUpdate.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", trainerToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***

                await _context.SaveChangesAsync();
        ***REMOVED***
            return View(trainerToUpdate);
    ***REMOVED***


        /// <summary>
        /// Returns a query result with the found Trainer given de id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query result</returns>
        private async Task<Trainer?> GetTrainer(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Trainer
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.TrainerProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer
                .Include(a => a.UserAccountModel)
                .Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***
***REMOVED***
***REMOVED***
