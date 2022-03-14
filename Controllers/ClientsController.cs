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
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.Gym.UserAccountModel.Id == user.Id || a.Gym.UserAccountModel.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(returnQuery);
    ***REMOVED***

        public async Task<IActionResult> ClientDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***


            return View(await _context.Client.Include(a => a.Gym).Where(a => a.ClientId == id).FirstOrDefaultAsync());
    ***REMOVED***

        public async Task<IActionResult> RemoveClientFromGym(int? id)
        ***REMOVED***
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();
            client.Gym = null;
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content("~/Clients/ShowClients"));
    ***REMOVED***

        public async Task<IActionResult> AddClientToGym(int? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();
            client.Gym = gym;
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content("~/Clients/ShowClients"));
    ***REMOVED***
***REMOVED***
***REMOVED***
