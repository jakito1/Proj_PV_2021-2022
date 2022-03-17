using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace NutriFitWeb.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public ClientsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowClients(string? email)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (email == null){
                return View(_context.Client.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    OrderByDescending(a => a.Gym));
            }
            return View(_context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.UserAccountModel.Email.Contains(email)).
                OrderByDescending(a => a.Gym));

        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _context.Client.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.ClientId == id));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveClientFromGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = _context.Client.
                Include(a => a.Gym).
                FirstOrDefault(a => a.ClientId == id);

            if (client.Gym == gym)
            {
                client.Gym = null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            
            return LocalRedirect(Url.Content(url));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddClientToGym(int? id, string? url)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.
                Include(a => a.Gym).
                FirstOrDefaultAsync(a => a.ClientId == id);

            if (client.Gym == null)
            {
                client.Gym = gym;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }      
            
            return LocalRedirect(Url.Content(url));
        }
    }
}
