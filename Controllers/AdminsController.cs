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
    /// AdminsController class, derives from Controller.
    /// </summary>
    public class AdminsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AdminsController(ApplicationDbContext context,
            IInteractNotification interactNotification)
        ***REMOVED***
            _context = context;
            _interactNotification = interactNotification;
    ***REMOVED***

        /// <summary>
        /// Shows all users for in the application from a Admin perspective
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A Paginated ViewResult</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> ShowAllUsers(string? searchString, string? currentFilter, int? pageNumber)
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

            IdentityRole? adminRole = await _context.Roles.FirstOrDefaultAsync(a => a.Name == "administrator");
            IQueryable<IdentityUserRole<string>>? admins = null;
            IQueryable<UserAccountModel>? users = null;
            if (adminRole is not null)
            ***REMOVED***
                admins = _context.UserRoles.Where(a => a.RoleId == adminRole.Id);
                if (admins is not null)
                ***REMOVED***
                    if (!string.IsNullOrEmpty(searchString))
                    ***REMOVED***
                        users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id)).Where(a => a.Email.Contains(searchString));
                ***REMOVED***
                    else
                    ***REMOVED***
                        users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id));
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***
            if (users is not null)
            ***REMOVED***
                int pageSize = 3;
                return View(await PaginatedList<UserAccountModel>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NoContent();
    ***REMOVED***

        /// <summary>
        /// Deletes a user account.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The user account id</param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccount(string? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(user);
    ***REMOVED***

        /// <summary>
        /// Deletes a user account and calls the Http POST method to the API to update the data.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The User account id</param>
        /// <returns>An Action result</returns>
        [HttpPost, ActionName("DeleteUserAccount")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccountPost(string? id)
        ***REMOVED***
            if (id is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);

            if (trainer is not null)
            ***REMOVED***
                _context.Trainer.Remove(trainer);
        ***REMOVED***
            if (nutritionist is not null)
            ***REMOVED***
                _context.Nutritionist.Remove(nutritionist);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                _context.Client.Remove(client);
        ***REMOVED***

            if (gym is not null)
            ***REMOVED***
                _context.Gym.Remove(gym);
        ***REMOVED***

            UserAccountModel? user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user is not null)
            ***REMOVED***
                _context.Users.Remove(user);
        ***REMOVED***

            await _context.SaveChangesAsync();

            return RedirectToAction("ShowAllUsers");
    ***REMOVED***

        /// <summary>
        /// Edit the User settings action to redirect to the selected user account.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The User account id</param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? userAccountModel = await _context.Users.FindAsync(id);
            if (userAccountModel is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(userAccountModel);
    ***REMOVED***

        /// <summary>
        /// Edits a user account and calls the Http POST method to the API to update the data.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The User account id</param>
        /// <returns>An Action result</returns>
        [HttpPost, ActionName("EditUserSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettingsPost(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate is not null && await TryUpdateModelAsync<UserAccountModel>(userToUpdate, "",
                u => u.PhoneNumber, u => u.UserName))
            ***REMOVED***
                await _interactNotification.Create($"O administrador alterou parte da sua conta.", userToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAllUsers");
        ***REMOVED***
            return View(userToUpdate);
    ***REMOVED***

        public IActionResult EditUserSettings()
        ***REMOVED***
            throw new NotImplementedException();
    ***REMOVED***
***REMOVED***
***REMOVED***
