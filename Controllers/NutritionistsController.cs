using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class NutritionistsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public NutritionistsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***
        public IActionResult Index()
        ***REMOVED***
            return View();
    ***REMOVED***

        public async Task<IActionResult> ShowNutritionists()
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            var returnQuery = _context.Nutritionist.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.Gym.UserAccountModel.Id == user.Id || a.Gym.UserAccountModel.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(await returnQuery.ToListAsync());
    ***REMOVED***

        public async Task<IActionResult> RemoveNutritionistFromGym(int? id)
        ***REMOVED***
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Where(a => a.NutritionistId == id).
                FirstOrDefaultAsync();
            nutritionist.Gym = null;
            _context.Nutritionist.Update(nutritionist);
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content("~/Nutritionists/ShowNutritionists"));
    ***REMOVED***

        public async Task<IActionResult> AddNutritionistToGym(int? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                Where(a => a.NutritionistId == id).
                FirstOrDefaultAsync();
            nutritionist.Gym = gym;
            _context.Nutritionist.Update(nutritionist);
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content("~/Nutritionists/ShowNutritionists"));
    ***REMOVED***

***REMOVED***
***REMOVED***
