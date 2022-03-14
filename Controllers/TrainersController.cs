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
    }
}
