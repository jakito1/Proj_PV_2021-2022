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

        [Authorize(Roles = "gym, nutritionist, trainer")]
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

            IOrderedQueryable<Client>? clients = null;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User.IsInRole("gym"))
            ***REMOVED***
                clients = GetClientsForGym(searchString, user.Id);
        ***REMOVED*** else if (User.IsInRole("trainer"))
            ***REMOVED***
                clients = await GetClientsForTrainer(searchString, user.Id);
        ***REMOVED*** else if (User.IsInRole("nutritionist"))
            ***REMOVED***
                //clients = 
        ***REMOVED***
            

            int pageSize = 3;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "gym, trainer, nutritionist")]
        public async Task<IActionResult> ClientDetails(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Trainer).
                Include(a => a.Nutritionist).
                FirstOrDefaultAsync(a => a.ClientId == id));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***           
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Gym).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && gym is not null && client.Gym is null ||
                (client is not null && client.Gym is not null && _userManager.GetUserId(User) == client.Gym.UserAccountModel.Id))
            ***REMOVED***
                client.Gym = (client.Gym is null) ? gym : null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***           
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> ChangeClientTrainerStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && trainer is not null && client.Gym == trainer.Gym && client.Trainer is null || 
                (client is not null && client.Trainer is not null && _userManager.GetUserId(User) == client.Trainer.UserAccountModel.Id))
            ***REMOVED***
                client.Trainer = (client.Trainer is null) ? trainer : null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***              
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
                c => c.Weight, c => c.Height, c => c.Photo))
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

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
    ***REMOVED***

        private IOrderedQueryable<Client> GetClientsForGym(string? searchString, string? userID)
        ***REMOVED***            
            if (string.IsNullOrEmpty(searchString))
            ***REMOVED***
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.Gym == null || a.Gym.UserAccountModel.Id == userID).
                    OrderByDescending(a => a.Gym);                
        ***REMOVED***
            return _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(searchString) && (a.Gym == null || a.Gym.UserAccountModel.Id == userID)).
                OrderByDescending(a => a.Gym);
    ***REMOVED***

        private async Task<IOrderedQueryable<Client>> GetClientsForTrainer(string? searchString, string? userID)
        ***REMOVED***
            Trainer? trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == userID);
            if (trainer is not null)
            ***REMOVED***
                if (string.IsNullOrEmpty(searchString))
                ***REMOVED***
                    return _context.Client.
                        Include(a => a.UserAccountModel).
                        Include(a => a.Trainer).
                        Include(a => a.Trainer.UserAccountModel).
                        Where(a => a.Gym == trainer.Gym && (a.Trainer == null || a.Trainer.UserAccountModel.Id == userID)).
                        OrderByDescending(a => a.Trainer);
            ***REMOVED***
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Trainer).
                    Include(a => a.Trainer.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString) && 
                        (a.Gym == trainer.Gym && (a.Trainer == null || a.Trainer.UserAccountModel.Id == userID))).
                    OrderByDescending(a => a.Trainer);
        ***REMOVED***
            return null;
            
    ***REMOVED***
***REMOVED***
***REMOVED***
