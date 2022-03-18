using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> ShowAllUsers(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IdentityRole? adminRole = await _context.Roles.FirstOrDefaultAsync(a => a.Name == "administrator");
            IQueryable<IdentityUserRole<string>>? admins = _context.UserRoles.Where(a => a.RoleId == adminRole.Id);

            IQueryable<UserAccountModel>? users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id));
            if (!string.IsNullOrEmpty(searchString))
            {
                 users = _context.Users.Where(p => admins.All(p2 => p2.UserId != p.Id)).Where(a => a.Email.Contains(searchString));
            }

            int pageSize = 3;
            return View(await PaginatedList<UserAccountModel>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccount(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
             
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUserAccount")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccountPost(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Trainer? trainer = await _context.Trainer.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Nutritionist? nutritionist = await _context.Nutritionist.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == id);

            if (trainer != null)
            {
                var clients = _context.Client.Where(a => a.Trainer == trainer);

                foreach (var c in clients)
                {
                    c.Trainer = null;
                    _context.Client.Update(c);
                }

                _context.Trainer.Remove(trainer);
            }

            if (nutritionist != null)
            {
                var clients = _context.Client.Where(a => a.Nutritionist == nutritionist);

                foreach (var c in clients)
                {
                    c.Nutritionist = null;
                    _context.Client.Update(c);
                }

                _context.Nutritionist.Remove(nutritionist);
            }

            if (client != null)
            {
                _context.Client.Remove(client);
            }

            if (gym != null)
            {

                var clients = _context.Client.
                    Where(a => a.Gym == gym);
                var trainers = _context.Trainer.
                    Where(a => a.Gym == gym);
                var nutritionists = _context.Nutritionist.
                    Where(a => a.Gym == gym);

                foreach (var c in clients)
                {
                    c.Gym = null;
                    _context.Client.Update(c);
                }

                foreach (var n in nutritionists)
                {
                    n.Gym = null;
                    _context.Nutritionist.Update(n);
                }

                foreach (var t in trainers)
                {
                    t.Gym = null;
                    _context.Trainer.Update(t);
                }

                _context.Gym.Remove(gym);
            }          
                        
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ShowAllUsers");
        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? userAccountModel = await _context.Users.FindAsync(id);
            if (userAccountModel == null)
            {
                return NotFound();
            }
            return View(userAccountModel);
        }

        [HttpPost, ActionName("EditUserSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> EditUserSettingsPost(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var userToUpdate = await _context.Users.FindAsync(id);
            if (await TryUpdateModelAsync<UserAccountModel>(userToUpdate, "", 
                u => u.PhoneNumber, u => u.UserName))
            {
                _context.SaveChanges();
                return RedirectToAction("ShowAllUsers");
            }
            return View(userToUpdate);
        }

    }
}
