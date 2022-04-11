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
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId,
            IPhotoManagement photoManagement,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
            _photoManagement = photoManagement;
            _interactNotification = interactNotification;
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
                            Include(a => a.ClientProfilePhoto).
                            FirstOrDefaultAsync(a => a.ClientId == id));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Gym).
            Include(a => a.Trainer).
            Include(a => a.Trainer.TrainingPlans).
            Include(a => a.Nutritionist).
            Include(a => a.Nutritionist.NutritionPlans).
            Include(a => a.TrainingPlanRequests).
            Include(a => a.NutritionPlanRequests).
            Include(a => a.UserAccountModel).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && gym is not null && client.Gym is null ||
                (client is not null && client.Gym is not null && _userManager.GetUserId(User) == client.Gym.UserAccountModel.Id))
            {
                client.Gym = (client.Gym is null) ? gym : null;
                if (client.Gym is null)
                {
                    if (client.Trainer is not null && client.Trainer.TrainingPlans is not null)
                    {
                        client.Trainer.TrainingPlans = client.Trainer.TrainingPlans.Where(a => a.Client != client).ToList();
                    }
                    if (client.Nutritionist is not null && client.Nutritionist.NutritionPlans is not null)
                    {
                        client.Nutritionist.NutritionPlans = client.Nutritionist.NutritionPlans.Where(a => a.Client != client).ToList();
                    }
                    client.Trainer = null;
                    client.WantsTrainer = false;
                    client.Nutritionist = null;
                    client.WantsNutritionist = false;
                    client.TrainingPlanRequests = null;
                    client.NutritionPlanRequests = null;
                    client.DateAddedToGym = null;
                    await _interactNotification.Create("Foi removido do seu ginásio.", client.UserAccountModel);
                }
                else
                {
                    client.DateAddedToGym = DateTime.Now;
                    await _interactNotification.Create($"Foi adicionado ao ginásio {client.Gym.GymName}.", client.UserAccountModel);
                }
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowClients", new { pageNumber, currentFilter });
        }

        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> ChangeClientTrainerStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            Include(a => a.Trainer.TrainingPlans).
            Include(a => a.UserAccountModel).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && trainer is not null && client.Gym == trainer.Gym && client.Trainer is null ||
                (client is not null && client.Trainer is not null && _userManager.GetUserId(User) == client.Trainer.UserAccountModel.Id))
            {
                if (client.Trainer is not null && client.Trainer.TrainingPlans is not null)
                {
                    client.Trainer.TrainingPlans = client.Trainer.TrainingPlans.Where(a => a.Client != client).ToList();
                }
                client.Trainer = (client.Trainer is null) ? trainer : null;
                if (client.Trainer is null)
                {
                    client.DateAddedToTrainer = null;
                    await _interactNotification.Create($"O {trainer.UserAccountModel.UserName} já não é seu treinador.", client.UserAccountModel);                   
                }
                else
                {
                    client.DateAddedToTrainer = DateTime.Now;
                    await _interactNotification.Create($"O {trainer.UserAccountModel.UserName} é agora o seu treinador.", client.UserAccountModel);
                }
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
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Nutritionist).
            Include(a => a.Nutritionist.NutritionPlans).
            Include(a => a.UserAccountModel).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && nutritionist is not null && client.Gym == nutritionist.Gym && client.Nutritionist is null ||
                (client is not null && client.Nutritionist is not null && _userManager.GetUserId(User) == client.Nutritionist.UserAccountModel.Id))
            {
                if (client.Nutritionist is not null && client.Nutritionist.NutritionPlans is not null)
                {
                    client.Nutritionist.NutritionPlans = client.Nutritionist.NutritionPlans.Where(a => a.Client != client).ToList();
                }
                client.Nutritionist = (client.Nutritionist is null) ? nutritionist : null;
                if (client.Nutritionist is null)
                {
                    client.DateAddedToNutritionist = null;
                    await _interactNotification.Create($"O {nutritionist.UserAccountModel.UserName} já não é seu nutricionista.", client.UserAccountModel);
                }
                else
                {
                    client.DateAddedToNutritionist = DateTime.Now;
                    await _interactNotification.Create($"O {nutritionist.UserAccountModel.UserName} é agora o seu nutricionista.", client.UserAccountModel);
                }
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
            if (nutritionist is not null && nutritionist.Clients.Contains(client) ||
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

            Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.ClientId == id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (client is null)
            {
                return NotFound();
            }
            if (nutritionist is not null && nutritionist.Clients.Contains(client) ||
                trainer is not null && trainer.Clients.Contains(client))
            {
                if (await TryUpdateModelAsync<Client>(client, "",
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData))
                {
                    if (trainer is not null)
                    {
                        await _interactNotification.Create($"O treinador {trainer.UserAccountModel.UserName} alterou algumas informações do seu perfil.", client.UserAccountModel);
                    }
                    else if (nutritionist is not null)
                    {
                        await _interactNotification.Create($"O nutricionista {nutritionist.UserAccountModel.UserName} alterou algumas informações do seu perfil.", client.UserAccountModel);
                    }
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
            if (client is not null && client.ClientProfilePhoto is not null)
            {
                client.ClientProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            }

            if (client is null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("EditClientSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettingsPost(string? id, IFormFile? formFile)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? clientToUpdate = await GetClient(id);

            Photo? oldPhoto = null;
            if (clientToUpdate is not null && clientToUpdate.ClientProfilePhoto is not null)
            {
                oldPhoto = clientToUpdate.ClientProfilePhoto;
            }
            if (clientToUpdate is not null)
            {
                clientToUpdate.ClientProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
            }

            if (await TryUpdateModelAsync<Client>(clientToUpdate, "",
                c => c.ClientFirstName, c => c.ClientLastName, c => c.ClientBirthday,
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData,
                c => c.ClientProfilePhoto, c => c.ClientSex))
            {
                if (oldPhoto is not null && clientToUpdate.ClientProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (clientToUpdate.ClientProfilePhoto is null)
                {
                    clientToUpdate.ClientProfilePhoto = oldPhoto;
                }

                if (clientToUpdate.ClientProfilePhoto is not null)
                {
                    clientToUpdate.ClientProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", clientToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                await _context.SaveChangesAsync();
                return View(clientToUpdate);
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
                return await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
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
