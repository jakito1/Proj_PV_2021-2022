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
        }

        [Authorize(Roles = "gym, nutritionist, trainer")]
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

            IOrderedQueryable<Client>? clients = null;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (User.IsInRole("gym"))
            {
                clients = GetClientsForGym(searchString, user.Id);
            }
            else if (User.IsInRole("trainer"))
            {
                clients = await GetClientsForTrainer(searchString, user.Id);
            }
            else if (User.IsInRole("nutritionist"))
            {
                clients = await GetClientsForNutritionist(searchString, user.Id);
            }

            int pageSize = 3;
            return View(await PaginatedList<Client>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "gym, trainer, nutritionist")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Trainer).
                Include(a => a.Nutritionist).
                FirstOrDefaultAsync(a => a.ClientId == id));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Gym).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && gym is not null && client.Gym is null ||
                (client is not null && client.Gym is not null && _userManager.GetUserId(User) == client.Gym.UserAccountModel.Id))
            {
                client.Gym = (client.Gym is null) ? gym : null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowClients", new { pageNumber, currentFilter });
        }

        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> ChangeClientTrainerStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && trainer is not null && client.Gym == trainer.Gym && client.Trainer is null ||
                (client is not null && client.Trainer is not null && _userManager.GetUserId(User) == client.Trainer.UserAccountModel.Id))
            {
                client.Trainer = (client.Trainer is null) ? trainer : null;
                client.WantsTrainer = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowClients", new { pageNumber, currentFilter });
        }

        [Authorize(Roles = "nutritionist")]
        public async Task<IActionResult> ChangeClientNutritionistStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && nutritionist is not null && client.Gym == nutritionist.Gym && client.Nutritionist is null ||
                (client is not null && client.Nutritionist is not null && _userManager.GetUserId(User) == client.Nutritionist.UserAccountModel.Id))
            {
                client.Nutritionist = (client.Nutritionist is null) ? nutritionist : null;
                client.WantsNutritionist = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowClients", new { pageNumber, currentFilter });
        }
        

        [Authorize(Roles = "trainer, nutritionist")]
        public async Task<IActionResult> EditClientForTrainerAndNutritionist(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Client? client = await _context.Client.FindAsync(id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (client is null)
            {
                return NotFound();
            }
            if (nutritionist is not null  && nutritionist.Clients.Contains(client) || 
                trainer is not null && trainer.Clients.Contains(client))
            {
                return View(client);
            }
            return Forbid();
        }

        [HttpPost, ActionName("EditClientForTrainerAndNutritionist")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "trainer, nutritionist")]
        public async Task<IActionResult> EditClientForTrainerAndNutritionistPost(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Client? client = await _context.Client.FindAsync(id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (client is null)
            {
                return NotFound();
            }
            if (nutritionist is not null  && nutritionist.Clients.Contains(client) || 
                trainer is not null && trainer.Clients.Contains(client))
            {
                if (await TryUpdateModelAsync<Client>(client, "",
                c => c.Weight, c => c.Height))
                {
                    await _context.SaveChangesAsync();
                    return LocalRedirect(Url.Content("~/"));
                }
            }            
            return View(client);
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
                c => c.Weight, c => c.Height, c => c.Photo))
            {
                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                return LocalRedirect(Url.Content("~/"));
            }
            return View(clientToUpdate);
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestTrainer(int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                    client.WantsTrainer = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans", new { pageNumber, currentFilter });
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestNutritionist(int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is not null)
                {
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                    client.WantsNutritionist = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowNutritionPlans", "NutritionPlans", new { pageNumber, currentFilter });
                }
            }
            return NotFound();
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

        private IOrderedQueryable<Client> GetClientsForGym(string? searchString, string? userID)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.Gym == null || a.Gym.UserAccountModel.Id == userID).
                    OrderByDescending(a => a.Gym);
            }
            return _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(searchString) && (a.Gym == null || a.Gym.UserAccountModel.Id == userID)).
                OrderByDescending(a => a.Gym);
        }

        private async Task<IOrderedQueryable<Client>> GetClientsForTrainer(string? searchString, string? userID)
        {
            Trainer? trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == userID);
            if (trainer is not null)
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    return _context.Client.
                        Include(a => a.UserAccountModel).
                        Include(a => a.Trainer).
                        Include(a => a.Trainer.UserAccountModel).
                        Where(a => a.Trainer.UserAccountModel.Id == userID || (a.Gym == trainer.Gym && a.WantsTrainer)).
                        OrderByDescending(a => a.Trainer);
                }
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Trainer).
                    Include(a => a.Trainer.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString) &&
                        (a.Trainer.UserAccountModel.Id == userID || (a.Gym == trainer.Gym && a.WantsTrainer))).
                    OrderByDescending(a => a.Trainer);
            }
            return null;
        }

        private async Task<IOrderedQueryable<Client>> GetClientsForNutritionist(string? searchString, string? userID)
        {
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == userID);
            if (nutritionist is not null)
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    return _context.Client.
                        Include(a => a.UserAccountModel).
                        Include(a => a.Nutritionist).
                        Include(a => a.Nutritionist.UserAccountModel).
                        Where(a => a.Nutritionist.UserAccountModel.Id == userID || (a.Gym == nutritionist.Gym && a.WantsNutritionist)).
                        OrderByDescending(a => a.Nutritionist);
                }
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Nutritionist).
                    Include(a => a.Nutritionist.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString) &&
                        (a.Nutritionist.UserAccountModel.Id == userID || (a.Gym == nutritionist.Gym && a.WantsNutritionist))).
                    OrderByDescending(a => a.Nutritionist);
            }
            return null;
        }
    }
}
