using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class ClientsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        public async Task<IActionResult> ShowClients()
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            var returnQuery = _context.Client.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.Gym.UserAccount.Id == user.Id || a.Gym.UserAccount.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(await returnQuery.ToListAsync());
    ***REMOVED***

        

        public async Task<IActionResult> ClientDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***


            return View(await _context.Client.Include(a => a.Gym).Where(a => a.ClientId == id).FirstOrDefaultAsync());
    ***REMOVED***

***REMOVED***
***REMOVED***
