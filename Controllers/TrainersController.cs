using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace NutriFitWeb.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public TrainersController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowTrainers()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            var returnQuery = _context.Trainer.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.Gym.UserAccountModel.Id == user.Id || a.Gym.UserAccountModel.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(await returnQuery.ToListAsync());
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveTrainerFromGym(int? id)
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
           
            return LocalRedirect(Url.Content("~/Trainers/ShowTrainers"));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddTrainerToGym(int? id)
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
           
            return LocalRedirect(Url.Content("~/Trainers/ShowTrainers"));
        }
    }
}
