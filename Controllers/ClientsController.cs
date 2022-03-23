using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class ClientsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowClients(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (searchString is not null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            ViewData["CurrentFilter"] = searchString;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            IOrderedQueryable<Client>? clients = _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym);
            if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                clients = _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(searchString)).
                OrderByDescending(a => a.Gym);
        ***REMOVED***

            int pageSize = 3;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ClientDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.ClientId == id));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = _context.Client.
            Include(a => a.Gym).
            FirstOrDefault(a => a.ClientId == id);

            client.Gym = (client.Gym is null) ? gym : null;
            _context.Client.Update(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Client? client = await GetClient(id);

            if (client is null)
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
                return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            var userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***

***REMOVED***
***REMOVED***
