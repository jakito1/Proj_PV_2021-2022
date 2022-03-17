using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public TrainersController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowTrainers(string? email)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (email == null)
            {
                return View(_context.Trainer.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                OrderByDescending(a => a.Gym));
            }
            return View(_context.Trainer.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(email)).OrderByDescending(a => a.Gym));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveTrainerFromGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            if(trainer.Gym == gym)
            {
                trainer.Gym = null;
                _context.Trainer.Update(trainer);
                await _context.SaveChangesAsync();
            }
           
            return LocalRedirect(Url.Content(url));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddTrainerToGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            if(trainer.Gym == null)
            {
                trainer.Gym = gym;
                _context.Trainer.Update(trainer);
                await _context.SaveChangesAsync();
            }
           
            return LocalRedirect(Url.Content(url));
        }

        public async Task<IActionResult> TrainerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync());
        }

        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Trainer? trainer = await GetTrainer(id);

            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        [HttpPost, ActionName("EditTrainerSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettingsPost(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName, t => t.TrainerLastName))
            {
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                return LocalRedirect(Url.Content("~/"));
            }
            return View(trainerToUpdate);
        }

        private async Task<Trainer> GetTrainer(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Trainer.FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
