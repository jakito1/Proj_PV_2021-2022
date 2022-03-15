using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowClients(string? email)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (email == null)***REMOVED***
                return View(_context.Client.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                OrderByDescending(a => a.Gym));
        ***REMOVED***
            return View(_context.Client.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(email)).OrderByDescending(a => a.Gym));

    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ClientDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefaultAsync());
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveClientFromGym(int? id, string? url)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();

            if (client.Gym == gym)
            ***REMOVED***
                client.Gym = null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            
            return LocalRedirect(Url.Content(url));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddClientToGym(int? id, string? url)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();

            if (client.Gym == null)
            ***REMOVED***
                client.Gym = gym;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***      
            
            return LocalRedirect(Url.Content(url));
    ***REMOVED***
***REMOVED***
***REMOVED***
