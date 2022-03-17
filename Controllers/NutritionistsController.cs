using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowNutritionists(string? email)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (email == null)
            ***REMOVED***
                return View(_context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym));
        ***REMOVED***

            return View(_context.Nutritionist.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(email)).
                    OrderByDescending(a => a.Gym));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveNutritionistFromGym(int? id, string? url)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            if(nutritionist.Gym == gym)
            ***REMOVED***
                nutritionist.Gym = null;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
        ***REMOVED***
          
            return LocalRedirect(Url.Content(url));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddNutritionistToGym(int? id, string? url)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id);

            if(nutritionist.Gym == null)
            ***REMOVED***
                nutritionist.Gym = gym;
                _context.Nutritionist.Update(nutritionist);
                await _context.SaveChangesAsync();
        ***REMOVED***
           
            return LocalRedirect(Url.Content(url));
    ***REMOVED***

        public async Task<IActionResult> NutritionistDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Nutritionist.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.NutritionistId == id));
    ***REMOVED***

***REMOVED***
***REMOVED***
