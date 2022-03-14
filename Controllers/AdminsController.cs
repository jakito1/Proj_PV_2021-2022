using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

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
        public async Task<IActionResult> ShowAllUsers(string? email)
        ***REMOVED***
            IQueryable<string>? adminRole = from a in _context.Roles where a.Name == "administrator" select a.Id;
            var admin = from a in _context.UserRoles where a.RoleId == adminRole.FirstOrDefault() select a.UserId;

            if (email == null)
            ***REMOVED***
                return View(_context.Users.Where(p => admin.All(p2 => p2 != p.Id)));
        ***REMOVED***

            return View(_context.Users.Where(p => admin.All(p2 => p2 != p.Id)).Where(a => a.Email.Contains(email)));

    ***REMOVED***

        
        public async Task<IActionResult> DeleteUserAccount(string? id, string? url)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Trainer? trainer = await _context.Trainer.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Nutritionist? nutritionist = await _context.Nutritionist.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Gym? gym = await _context.Gym.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();
            Client? client = await _context.Client.Where(a => a.UserAccountModel.Id == id).FirstOrDefaultAsync();



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
            if (client != null)
            ***REMOVED***
                _context.Client.Remove(client);
        ***REMOVED***
            if (gym != null)
            ***REMOVED***
                var clients = _context.Client.
                    Where(a => a.Gym == gym);
                var trainers = _context.Trainer.
                    Where(a => a.Gym == gym);
                var nutritionists = _context.Nutritionist.
                    Where(a => a.Gym == gym);

                foreach (var c in clients)
                ***REMOVED***
                    c.Gym = null;
                    _context.Client.Update(c);
            ***REMOVED***
                foreach (var n in nutritionists)
                ***REMOVED***
                    n.Gym = null;
                    _context.Nutritionist.Update(n);
            ***REMOVED***
                foreach (var t in trainers)
                ***REMOVED***
                    t.Gym = null;
                    _context.Trainer.Update(t);
            ***REMOVED***

                await _context.SaveChangesAsync();
                _context.Gym.Remove(gym);
        ***REMOVED***          
                 
            _context.Users.Remove(await _context.Users.Where(a => a.Id == id).FirstOrDefaultAsync());
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content(url));
    ***REMOVED***

***REMOVED***
***REMOVED***
