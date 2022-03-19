using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class AdminsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> ShowAllUsers(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (searchString != null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            ViewData["CurrentFilter"] = searchString;

            IdentityRole? adminRole = await _context.Roles.FirstOrDefaultAsync(a => a.Name == "administrator");
            IQueryable<IdentityUserRole<string>>? admins = _context.UserRoles.Where(a => a.RoleId == adminRole.Id);

            IQueryable<UserAccountModel>? users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id));
            if (!string.IsNullOrEmpty(searchString))
            ***REMOVED***
                 users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id)).Where(a => a.Email.Contains(searchString));
        ***REMOVED***

            int pageSize = 3;
            return View(await PaginatedList<UserAccountModel>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccount(string? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
             
            if (user == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(user);
    ***REMOVED***

        [HttpPost, ActionName("DeleteUserAccount")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccountPost(string? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);

            if (trainer != null)
            ***REMOVED***
                var clients = _context.Client.Where(a => a.Trainer == trainer);

                foreach (var c in clients)
                ***REMOVED***
                    c.Trainer = null;
                    _context.Client.Update(c);
            ***REMOVED***

                _context.Trainer.Remove(trainer);
        ***REMOVED***
            #region
            if (nutritionist != null)
            ***REMOVED***
                var clients = _context.Client.Where(a => a.Nutritionist == nutritionist);

                foreach (var c in clients)
                ***REMOVED***
                    c.Nutritionist = null;
                    _context.Client.Update(c);
            ***REMOVED***

                _context.Nutritionist.Remove(nutritionist);
        ***REMOVED***
            #endregion

            if (client != null)
            ***REMOVED***
                _context.Client.Remove(client);
        ***REMOVED***

            if (gym != null)
            ***REMOVED***            
                _context.Gym.Remove(gym);
        ***REMOVED***          
                        
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user != null)
            ***REMOVED***
                _context.Users.Remove(user);
                
        ***REMOVED***

            await _context.SaveChangesAsync();

            return RedirectToAction("ShowAllUsers");
    ***REMOVED***

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettings(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? userAccountModel = await _context.Users.FindAsync(id);
            if (userAccountModel == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(userAccountModel);
    ***REMOVED***

        [HttpPost, ActionName("EditUserSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettingsPost(string? id)
        ***REMOVED***
            if (string.IsNullOrEmpty(id))
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            var userToUpdate = await _context.Users.FindAsync(id);
            if (await TryUpdateModelAsync<UserAccountModel>(userToUpdate, "", 
                u => u.PhoneNumber, u => u.UserName))
            ***REMOVED***
                _context.SaveChanges();
                return RedirectToAction("ShowAllUsers");
        ***REMOVED***
            return View(userToUpdate);
    ***REMOVED***

***REMOVED***
***REMOVED***
