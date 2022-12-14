using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// AdminsController class, derives from Controller.
    /// </summary>
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInteractNotification _interactNotification;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AdminsController(ApplicationDbContext context,
            IInteractNotification interactNotification)
        {
            _context = context;
            _interactNotification = interactNotification;
        }

        /// <summary>
        /// Shows all users for in the application from a Admin perspective
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A Paginated ViewResult</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> ShowAllUsers(string? searchString, string? currentFilter, int? pageNumber)
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

            IdentityRole? adminRole = await _context.Roles.FirstOrDefaultAsync(a => a.Name == "administrator");
            IQueryable<IdentityUserRole<string>>? admins = null;
            IQueryable<UserAccountModel>? users = null;
            if (adminRole is not null)
            {
                admins = _context.UserRoles.Where(a => a.RoleId == adminRole.Id);
                if (admins is not null)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id)).Where(a => a.Email.Contains(searchString));
                    }
                    else
                    {
                        users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id));
                    }
                }
            }
            if (users is not null)
            {
                int pageSize = 3;
                return View(await PaginatedList<UserAccountModel>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a user account.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The user account id</param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccount(string? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            UserAccountModel? user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }

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
        {
            if (id is null)
            {
                return BadRequest();
            }

            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);

            if (trainer is not null)
            {
                _context.Trainer.Remove(trainer);
            }
            if (nutritionist is not null)
            {
                _context.Nutritionist.Remove(nutritionist);
            }

            if (client is not null)
            {
                _context.Client.Remove(client);
            }

            if (gym is not null)
            {
                _context.Gym.Remove(gym);
            }

            UserAccountModel? user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user is not null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ShowAllUsers");
        }

        /// <summary>
        /// Edit the User settings action to redirect to the selected user account.
        /// Only accessible for Administrator role.
        /// </summary>
        /// <param name="id">The User account id</param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? userAccountModel = await _context.Users.FindAsync(id);
            if (userAccountModel is null)
            {
                return NotFound();
            }
            return View(userAccountModel);
        }

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
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate is not null && await TryUpdateModelAsync<UserAccountModel>(userToUpdate, "",
                u => u.PhoneNumber, u => u.UserName))
            {
                await _interactNotification.Create($"O administrador alterou parte da sua conta.", userToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowAllUsers");
            }
            return View(userToUpdate);
        }
    }
}
