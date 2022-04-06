using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// ClientsController Class derives from Controller
    /// </summary>
    public class ClientsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User Manager API</param>
        /// <param name="isUserInRoleByUserId">Interface to determine if the user is of a given role</param>
        /// <param name="photoManagement">Photo management Interface</param>
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId isUserInRoleByUserId,
            IPhotoManagement photoManagement)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = isUserInRoleByUserId;
            _photoManagement = photoManagement;
    ***REMOVED***

        /// <summary>
        /// Shows a paginated View of all the clients.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>An Action result</returns>
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

        /// <summary>
        /// Shows a specific client details page, based on client id.
        /// </summary>
        /// <param name="id">Client id</param>
        /// <returns>An Action result</returns>
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

        /// <summary>
        /// Displays the page to change a client's gym status, based on client id.
        /// Only accessible for the Gym role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>An Action result</returns>
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
            ***REMOVED***
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        /// <summary>
        /// Displays the page to change a client's trainer status, based on client id.
        /// Only accessible for the Trainer role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> ChangeClientTrainerStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Trainer).
            Include(a => a.Trainer.TrainingPlans).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && trainer is not null && client.Gym == trainer.Gym && client.Trainer is null ||
                (client is not null && client.Trainer is not null && _userManager.GetUserId(User) == client.Trainer.UserAccountModel.Id))
            ***REMOVED***
                if (client.Trainer is not null && client.Trainer.TrainingPlans is not null)
                ***REMOVED***
                    client.Trainer.TrainingPlans = client.Trainer.TrainingPlans.Where(a => a.Client != client).ToList();
            ***REMOVED***
                client.Trainer = (client.Trainer is null) ? trainer : null;
                client.WantsTrainer = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        /// <summary>
        /// Displays the page to change a client's nutritionist status, based on client id.
        /// Only accessible for the Gym role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "nutritionist")]
        public async Task<IActionResult> ChangeClientNutritionistStatus(int? id, int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Nutritionist nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
            Include(a => a.Nutritionist).
            Include(a => a.Nutritionist.NutritionPlans).
            FirstOrDefaultAsync(a => a.ClientId == id);

            if (client is not null && nutritionist is not null && client.Gym == nutritionist.Gym && client.Nutritionist is null ||
                (client is not null && client.Nutritionist is not null && _userManager.GetUserId(User) == client.Nutritionist.UserAccountModel.Id))
            ***REMOVED***
                if (client.Nutritionist is not null && client.Nutritionist.NutritionPlans is not null)
                ***REMOVED***
                    client.Nutritionist.NutritionPlans = client.Nutritionist.NutritionPlans.Where(a => a.Client != client).ToList();
            ***REMOVED***
                client.Nutritionist = (client.Nutritionist is null) ? nutritionist : null;
                client.WantsNutritionist = false;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowClients", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
    ***REMOVED***

        /// <summary>
        /// Displays a page to edit the client details.
        /// Only accessible to the Trainer and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
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

        /// <summary>
        /// Http POST method to the API to edit the client for the trainers and nutritionists.
        /// Only accessible to the Trainer and Nutritionist roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
        [HttpPost, ActionName("EditClientForTrainerAndNutritionist")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "trainer, nutritionist")]
        public async Task<IActionResult> EditClientForTrainerAndNutritionistPost(int? id)
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
                if (await TryUpdateModelAsync<Client>(client, "",
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData))
                ***REMOVED***
                    await _context.SaveChangesAsync();
                    return LocalRedirect(Url.Content("~/"));
            ***REMOVED***
        ***REMOVED***
            return View(client);
    ***REMOVED***

        /// <summary>
        /// Displays a page to edit the client settings for admins and clients.
        /// Only accessible to the Administrator and Client roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
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

        /// <summary>
        /// Http POST method to the API to edit the client for the admins and clients.
        /// Only accessible to the Administrator and Client roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
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
                c => c.Weight, c => c.Height, c => c.LeanMass, c => c.FatMass, c => c.OtherClientData, c => c.ClientProfilePhoto))
            ***REMOVED***
                if (oldPhoto is not null && clientToUpdate.ClientProfilePhoto is not null)
                ***REMOVED***
                    _context.Photos.Remove(oldPhoto);
            ***REMOVED***
                else if (clientToUpdate.ClientProfilePhoto is null)
                ***REMOVED***
                    clientToUpdate.ClientProfilePhoto = oldPhoto;
            ***REMOVED***

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                ***REMOVED***
                    return RedirectToAction("ShowAllUsers", "Admins");
            ***REMOVED***
                if (clientToUpdate.ClientProfilePhoto is not null)
                ***REMOVED***
                    clientToUpdate.ClientProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
                return View(clientToUpdate);
        ***REMOVED***
            return View(clientToUpdate);
    ***REMOVED***

        /// <summary>
        /// Displays a page for the users to request a trainer.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestTrainer(int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null)
            ***REMOVED***
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                    client.WantsTrainer = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        /// <summary>
        /// Displays a page for the users to request a nutritionist.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="currentFilter"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> RequestNutritionist(int? pageNumber, string? currentFilter)
        ***REMOVED***
            UserAccountModel? user = null;
            Client? client = null;
            if (User.Identity is not null)
            ***REMOVED***
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is not null)
                ***REMOVED***
                    client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                    client.WantsNutritionist = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowNutritionPlans", "NutritionPlans", new ***REMOVED*** pageNumber, currentFilter ***REMOVED***);
            ***REMOVED***
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        /// <summary>
        /// Utility method to get a client by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A query result</returns>
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

        /// <summary>
        /// Utility method to get all clients for a gym by client id.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="userID"></param>
        /// <returns>A query result</returns>
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

        /// <summary>
        /// Utility method to get all clients for a trainer by client id.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="userID"></param>
        /// <returns>A query result</returns>
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

        /// <summary>
        /// Utility method to get all clients for a nutritionist by client id.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="userID"></param>
        /// <returns>A query result</returns>
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
