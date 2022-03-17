using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class ClientsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private IIsUserInRoleByUserId _isUserInRoleByUserId;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowClients(string? email)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (email == null)***REMOVED***
                return View(_context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym));
        ***REMOVED***
            return View(_context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(email)).
                OrderByDescending(a => a.Gym));

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
                FirstOrDefaultAsync(a => a.ClientId == id));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveClientFromGym(int? id, string? url)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = _context.Client.
                Include(a => a.Gym).
                FirstOrDefault(a => a.ClientId == id);

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
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.ClientId == id);

            if (client.Gym == null)
            ***REMOVED***
                client.Gym = gym;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***      
            
            return LocalRedirect(Url.Content(url));
    ***REMOVED***

        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await GetClient(id);

            if (client == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(client);
    ***REMOVED***

        [HttpPost, ActionName("EditClientSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettingsPost(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? clientToUpdate = await GetClient(id);
            


            if (await TryUpdateModelAsync<Client>(clientToUpdate, "",
                c => c.ClientFirstName, c => c.ClientLastName, c => c.ClientBirthday,
                c => c.Weight, c => c.Height))
            ***REMOVED***
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                return LocalRedirect(Url.Content("~/"));
        ***REMOVED***
            return View(clientToUpdate);
    ***REMOVED***

        private async Task<Client> GetClient(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return _context.Client.FirstOrDefault(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***

***REMOVED***
***REMOVED***
