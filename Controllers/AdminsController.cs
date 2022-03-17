using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

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
        public async Task<IActionResult> ShowAllUsers(string? email)
        {
            IdentityRole? adminRole = await _context.Roles.Where(a => a.Name == "administrator").FirstOrDefaultAsync();
            var admin = _context.UserRoles.Where(a => a.RoleId == adminRole.Id);

            if (email == null)
            {
                return View(_context.Users.Where(p => admin.All(p2 => p2.UserId != p.Id)));
            }

            return View(_context.Users.Where(p => admin.All(p2 => p2.UserId != p.Id)).Where(a => a.Email.Contains(email)));

        }

        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteUserAccount(string? id)
        {
            if (id == null)
            {
                return NotFound();
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
        public async Task<IActionResult> DeleteUserAccountConfirmed(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer? trainer = await _context.Trainer.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Nutritionist? nutritionist = await _context.Nutritionist.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Gym? gym = await _context.Gym.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Client? client = await _context.Client.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();

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

    }
}
