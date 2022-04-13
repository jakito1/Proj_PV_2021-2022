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
        private readonly IPhotoManagement _photoManagement;
        private readonly IInteractNotification _interactNotification;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId,
            IPhotoManagement photoManagement,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
            _photoManagement = photoManagement;
            _interactNotification = interactNotification;
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
        ***REMOVED***
            else if (User.IsInRole("trainer"))
            ***REMOVED***
                clients = await GetClientsForTrainer(searchString, user.Id);
        ***REMOVED***
            else if (User.IsInRole("nutritionist"))
            ***REMOVED***
                clients = await GetClientsForNutritionist(searchString, user.Id);
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
                            Include(a => a.ClientProfilePhoto).
                            FirstOrDefaultAsync(a => a.ClientId == id));
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeClientGymStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
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
            ***REMOVED***
                client.Gym = (client.Gym is null) ? gym : null;
                if (client.Gym is null)
                ***REMOVED***
                    if (client.Trainer is not null && client.Trainer.TrainingPlans is not null)
                    ***REMOVED***
                        client.Trainer.TrainingPlans = client.Trainer.TrainingPlans.Where(a => a.Client != client).ToList();
                ***REMOVED***
                    if (client.Nutritionist is not null && client.Nutritionist.NutritionPlans is not null)
                    ***REMOVED***
                        client.Nutritionist.NutritionPlans = client.Nutritionist.NutritionPlans.Where(a => a.Client != client).ToList();
                ***REMOVED***
                    client.Trainer = null;
                    client.WantsTrainer = false;
                    client.Nutritionist = null;
                    client.WantsNutritionist = false;
                    client.TrainingPlanRequests = null;
                    client.NutritionPlanRequests = null;
                    client.DateAddedToGym = null;
                    await _interactNotification.Create("Foi removido do seu ginásio.", client.UserAccountModel);
            ***REMOVED***
                else
                ***REMOVED***
                    client.DateAddedToGym = DateTime.Now;
                    await _interactNotification.Create($"Foi adicionado ao ginásio ***REMOVED***client.Gym.GymName***REMOVED***.", client.UserAccountModel);
            ***REMOVED***
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> ChangeClientTrainerStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            Include(a => a.Trainer.TrainingPlans).
            Include(a => a.UserAccountModel).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && trainer is not null && client.Gym == trainer.Gym && client.Trainer is null ||
                (client is not null && client.Trainer is not null && _userManager.GetUserId(User) == client.Trainer.UserAccountModel.Id))
            ***REMOVED***
                if (client.Trainer is not null && client.Trainer.TrainingPlans is not null)
                ***REMOVED***
                    client.Trainer.TrainingPlans = client.Trainer.TrainingPlans.Where(a => a.Client != client).ToList();
            ***REMOVED***
                client.Trainer = (client.Trainer is null) ? trainer : null;
                if (client.Trainer is null)
                ***REMOVED***
                    client.DateAddedToTrainer = null;
                    await _interactNotification.Create($"O ***REMOVED***trainer.UserAccountModel.UserName***REMOVED*** já não é seu treinador.", client.UserAccountModel);
            ***REMOVED***
                else
                ***REMOVED***
                    client.DateAddedToTrainer = DateTime.Now;
                    await _interactNotification.Create($"O ***REMOVED***trainer.UserAccountModel.UserName***REMOVED*** é agora o seu treinador.", client.UserAccountModel);
            ***REMOVED***
                client.WantsTrainer = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        [Authorize(Roles = "nutritionist")]
        public async Task<IActionResult> ChangeClientNutritionistStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Nutritionist).
            Include(a => a.Nutritionist.NutritionPlans).
            Include(a => a.UserAccountModel).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && nutritionist is not null && client.Gym == nutritionist.Gym && client.Nutritionist is null ||
                (client is not null && client.Nutritionist is not null && _userManager.GetUserId(User) == client.Nutritionist.UserAccountModel.Id))
            ***REMOVED***
                if (client.Nutritionist is not null && client.Nutritionist.NutritionPlans is not null)
                ***REMOVED***
                    client.Nutritionist.NutritionPlans = client.Nutritionist.NutritionPlans.Where(a => a.Client != client).ToList();
            ***REMOVED***
                client.Nutritionist = (client.Nutritionist is null) ? nutritionist : null;
                if (client.Nutritionist is null)
                ***REMOVED***
                    client.DateAddedToNutritionist = null;
                    await _interactNotification.Create($"O ***REMOVED***nutritionist.UserAccountModel.UserName***REMOVED*** já não é seu nutricionista.", client.UserAccountModel);
            ***REMOVED***
                else
                ***REMOVED***
                    client.DateAddedToNutritionist = DateTime.Now;
                    await _interactNotification.Create($"O ***REMOVED***nutritionist.UserAccountModel.UserName***REMOVED*** é agora o seu nutricionista.", client.UserAccountModel);
            ***REMOVED***
                client.WantsNutritionist = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***


        [Authorize(Roles = "trainer, nutritionist")]
        public async Task<IActionResult> EditClientForTrainerAndNutritionist(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Client? client = await _context.Client.FindAsync(id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (client is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients.Contains(client) ||
                trainer is not null && trainer.Clients.Contains(client))
            ***REMOVED***
                return View(client);
        ***REMOVED***
            return Forbid();
    ***REMOVED***

        [HttpPost, ActionName("EditClientForTrainerAndNutritionist")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "trainer, nutritionist")]
        public async Task<IActionResult> EditClientForTrainerAndNutritionistPost(int? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Client? client = await _context.Client.Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.ClientId == id);
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Clients).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (client is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            if (nutritionist is not null && nutritionist.Clients.Contains(client) ||
                trainer is not null && trainer.Clients.Contains(client))
            ***REMOVED***
                if (await TryUpdateModelAsync<Client>(client, "",
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData))
                ***REMOVED***
                    if (trainer is not null)
                    ***REMOVED***
                        await _interactNotification.Create($"O treinador ***REMOVED***trainer.UserAccountModel.UserName***REMOVED*** alterou algumas informações do seu perfil.", client.UserAccountModel);
                ***REMOVED***
                    else if (nutritionist is not null)
                    ***REMOVED***
                        await _interactNotification.Create($"O nutricionista ***REMOVED***nutritionist.UserAccountModel.UserName***REMOVED*** alterou algumas informações do seu perfil.", client.UserAccountModel);
                ***REMOVED***
                    await _context.SaveChangesAsync();
                    return LocalRedirect(Url.Content("~/"));
            ***REMOVED***
        ***REMOVED***
            return View(client);
    ***REMOVED***

        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Client? client = await GetClient(id);
            if (client is not null && client.ClientProfilePhoto is not null)
            ***REMOVED***
                client.ClientProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
        ***REMOVED***

            if (client is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(client);
    ***REMOVED***

        [HttpPost, ActionName("EditClientSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, client")]
        public async Task<IActionResult> EditClientSettingsPost(string? id, IFormFile? formFile)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? clientToUpdate = await GetClient(id);

            Photo? oldPhoto = null;
            if (clientToUpdate is not null && clientToUpdate.ClientProfilePhoto is not null)
            ***REMOVED***
                oldPhoto = clientToUpdate.ClientProfilePhoto;
        ***REMOVED***
            if (clientToUpdate is not null)
            ***REMOVED***
                clientToUpdate.ClientProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
        ***REMOVED***

            if (await TryUpdateModelAsync<Client>(clientToUpdate, "",
                c => c.ClientFirstName, c => c.ClientLastName, c => c.ClientBirthday,
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData,
                c => c.ClientProfilePhoto, c => c.ClientSex))
            ***REMOVED***
                if (oldPhoto is not null && clientToUpdate.ClientProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (clientToUpdate.ClientProfilePhoto is null)
                ***REMOVED***
                    clientToUpdate.ClientProfilePhoto = oldPhoto;
            ***REMOVED***

                if (clientToUpdate.ClientProfilePhoto is not null)
                ***REMOVED***
                    clientToUpdate.ClientProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    await _interactNotification.Create($"O administrador alterou parte do seu perfil.", clientToUpdate.UserAccountModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                await _context.SaveChangesAsync();
                return View(clientToUpdate);
        ***REMOVED***
            return View(clientToUpdate);
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestTrainer(int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null && _context.Client is not null)
            ***REMOVED***
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                client = await _context.Client.Include(a => a.Gym).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (user is not null && client is not null && client.Gym is not null)
                ***REMOVED***
                    client.WantsTrainer = true;
                    foreach (Trainer trainer in _context.Trainer.Include(a => a.UserAccountModel).Where(a => a.Gym == client.Gym))
                    ***REMOVED***
                        await _interactNotification.Create($"O cliente '***REMOVED***client.UserAccountModel.UserName***REMOVED***' está à procura de um treinador.", trainer.UserAccountModel);
                ***REMOVED***
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestNutritionist(int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null && _context.Client is not null)
            ***REMOVED***
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                client = await _context.Client.Include(a => a.Gym).Include(a => a.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (user is not null && client is not null && client.Gym is not null)
                ***REMOVED***
                    client.WantsNutritionist = true;
                    foreach (Nutritionist nutritionist in _context.Nutritionist.Include(a => a.UserAccountModel).Where(a => a.Gym == client.Gym))
                    ***REMOVED***
                        await _interactNotification.Create($"O cliente '***REMOVED***client.UserAccountModel.UserName***REMOVED***' está à procura de um nutricionista.", nutritionist.UserAccountModel);
                ***REMOVED***
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowNutritionPlans", "NutritionPlans", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        private async Task<Client> GetClient(string? id)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            ***REMOVED***
                return await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
        ***REMOVED***

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
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
                        Where(a => a.Trainer.UserAccountModel.Id == userID || (a.Gym == trainer.Gym && a.WantsTrainer)).
                        OrderByDescending(a => a.Trainer);
            ***REMOVED***
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Trainer).
                    Include(a => a.Trainer.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString) &&
                        (a.Trainer.UserAccountModel.Id == userID || (a.Gym == trainer.Gym && a.WantsTrainer))).
                    OrderByDescending(a => a.Trainer);
        ***REMOVED***
            return null;
    ***REMOVED***

        private async Task<IOrderedQueryable<Client>> GetClientsForNutritionist(string? searchString, string? userID)
        ***REMOVED***
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == userID);
            if (nutritionist is not null)
            ***REMOVED***
                if (string.IsNullOrEmpty(searchString))
                ***REMOVED***
                    return _context.Client.
                        Include(a => a.UserAccountModel).
                        Include(a => a.Nutritionist).
                        Include(a => a.Nutritionist.UserAccountModel).
                        Where(a => a.Nutritionist.UserAccountModel.Id == userID || (a.Gym == nutritionist.Gym && a.WantsNutritionist)).
                        OrderByDescending(a => a.Nutritionist);
            ***REMOVED***
                return _context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Nutritionist).
                    Include(a => a.Nutritionist.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString) &&
                        (a.Nutritionist.UserAccountModel.Id == userID || (a.Gym == nutritionist.Gym && a.WantsNutritionist))).
                    OrderByDescending(a => a.Nutritionist);
        ***REMOVED***
            return null;
    ***REMOVED***
***REMOVED***
***REMOVED***
