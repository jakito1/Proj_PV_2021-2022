using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class ClientsController : Controller
    {
        private readonly string SessionKeyClientsUserAccounts;
        private readonly string SessionKeyCurrentTrainer;
        private readonly string SessionKeyCurrentNutritionist;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
            SessionKeyClientsUserAccounts = "_ClientsUserAccounts";
            SessionKeyCurrentTrainer = "_CurrentTrainer";
            SessionKeyCurrentNutritionist = "_CurrentNutritionist";
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowClients(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            IOrderedQueryable<Client>? clients = _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym);
            if (!string.IsNullOrEmpty(searchString))
            {
                clients = _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(searchString)).
                OrderByDescending(a => a.Gym);
            }

            int pageSize = 3;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.ClientId == id));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = _context.Client.
            Include(a => a.Gym).
            FirstOrDefault(a => a.ClientId == id);

            client.Gym = (client.Gym is null) ? gym : null;
            _context.Client.Update(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowClients", new { pageNumber, currentFilter });
        }

        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Client? client = await GetClient(id);

            if (client is null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("EditClientSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettingsPost(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? clientToUpdate = await GetClient(id);

            if (await TryUpdateModelAsync<Client>(clientToUpdate, "",
                c => c.ClientFirstName, c => c.ClientLastName, c => c.ClientBirthday,
                c => c.Weight, c => c.Height))
            {
                _context.SaveChanges();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                return LocalRedirect(Url.Content("~/"));
            }
            return View(clientToUpdate);
        }

        private async Task<Client> GetClient(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }

    }
}
