using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainersController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public TrainersController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowTrainers(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (searchString != null)
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
        public async Task<IActionResult> RemoveTrainerFromGym(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            if(trainer.Gym == gym)
            ***REMOVED***
                trainer.Gym = null;
                _context.Trainer.Update(trainer);
                await _context.SaveChangesAsync();
        ***REMOVED***

            return RedirectToAction("ShowTrainers", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddTrainerToGym(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            if(trainer.Gym == null)
            ***REMOVED***
                trainer.Gym = gym;
                _context.Trainer.Update(trainer);
                await _context.SaveChangesAsync();
        ***REMOVED***

            return RedirectToAction("ShowTrainers", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        public async Task<IActionResult> TrainerDetails(int? id)
        ***REMOVED***
            if (id == null)
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

            if (trainer == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(trainer);
    ***REMOVED***

        [HttpPost, ActionName("EditTrainerSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettingsPost(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName, t => t.TrainerLastName))
            ***REMOVED***
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                return LocalRedirect(Url.Content("~/"));
        ***REMOVED***
            return View(trainerToUpdate);
    ***REMOVED***

        private async Task<Trainer> GetTrainer(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Trainer.FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***
***REMOVED***
***REMOVED***
