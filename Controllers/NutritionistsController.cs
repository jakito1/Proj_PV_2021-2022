using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace NutriFitWeb.Controllers
{
    public class NutritionistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public NutritionistsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            var returnQuery = _context.Nutritionist.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.Gym.UserAccountModel.Id == user.Id || a.Gym.UserAccountModel.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(await returnQuery.ToListAsync());
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveNutritionistFromGym(int? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Where(a => a.NutritionistId == id).
                FirstOrDefaultAsync();

            if(nutritionist.Gym == gym)
            {
                nutritionist.Gym = null;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
            }
          
            return LocalRedirect(Url.Content("~/Nutritionists/ShowNutritionists"));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddNutritionistToGym(int? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Where(a => a.NutritionistId == id).
                FirstOrDefaultAsync();

            if(nutritionist.Gym == null)
            {
                nutritionist.Gym = gym;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
            }
           
            return LocalRedirect(Url.Content("~/Nutritionists/ShowNutritionists"));
        }

    }
}
