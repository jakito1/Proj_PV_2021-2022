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
    /// TrainersController class, derives from Controller
    /// </summary>
    public class TrainersController : Controller
    {
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
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
            _interactNotification = interactNotification;
        }
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
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
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
            IOrderedQueryable<Trainer>? trainers = null;

            if (string.IsNullOrEmpty(searchString))
            {
                trainers = _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym!.UserAccountModel).
                OrderByDescending(a => a.Gym);
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                trainers = _context.Trainer.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym!.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
            }

            if (trainers is not null)
            {
                int pageSize = 3;
                return View(await PaginatedList<Trainer>.CreateAsync(trainers.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }


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
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
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
            {
                return NotFound();
            }

            trainer.Gym = (trainer.Gym is null) ? gym : null;
            if (trainer.Gym is null)
            {
                trainer.Clients = null;
                trainer.TrainingPlans = null;
                await _interactNotification.Create("Foi removido do seu ginásio.", trainer.UserAccountModel);
            }
            else
            {
                await _interactNotification.Create($"Foi adicionado ao ginásio {trainer.Gym.GymName}.", trainer.UserAccountModel);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowTrainers", new { pageNumber, currentFilter });
        }

        [Authorize(Roles = "gym")]
        /// <summary>
        /// Renders a view to display a Trainer's details, given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        public async Task<IActionResult> TrainerDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.TrainerProfilePhoto).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync());
        }

        /// <summary>
        /// Renders a view to edit a Trainer's settings.
        /// Only accessible to Administrator and Trainer roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettings(string? id)
        {
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            {
                return BadRequest();
            }

            Trainer? trainer = await GetTrainer(id);
            if (trainer is not null && trainer.TrainerProfilePhoto is not null)
            {
                trainer.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            }

            if (trainer is null)
            {
                return NotFound();
            }
            return View(trainer);
        }

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
        {
            if (string.IsNullOrEmpty(id) || User.Identity is null)
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            if (trainerToUpdate is null)
            {
                return NotFound();
            }

            Photo? oldPhoto = null;
            if (trainerToUpdate.TrainerProfilePhoto is not null)
            {
                oldPhoto = trainerToUpdate.TrainerProfilePhoto;
            }
            trainerToUpdate.TrainerProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName!, t => t.TrainerLastName!, t => t.TrainerProfilePhoto!))
            {
                if (oldPhoto is not null && trainerToUpdate.TrainerProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (trainerToUpdate.TrainerProfilePhoto is null)
                {
                    trainerToUpdate.TrainerProfilePhoto = oldPhoto;
                }

                if (trainerToUpdate.TrainerProfilePhoto is not null)
                {
                    trainerToUpdate.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", trainerToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
                }

                await _context.SaveChangesAsync();
            }
            return View(trainerToUpdate);
        }

        private async Task<Trainer?> GetTrainer(string? id)
        /// <summary>
        /// Returns a query result with the found Trainer given de id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query result</returns>
        private async Task<Trainer> GetTrainer(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Trainer
                    .Include(a => a.UserAccountModel)
                    .Include(a => a.TrainerProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer
                .Include(a => a.UserAccountModel)
                .Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
