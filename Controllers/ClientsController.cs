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
        public async Task<IActionResult> ShowClients()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(_context.Client.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                Where(a => a.Gym.UserAccountModel.Id == user.Id || a.Gym.UserAccountModel.Id != user.Id).
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
                Where(a => a.ClientId == id).
                FirstOrDefaultAsync());
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> RemoveClientFromGym(int? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();

            if (client.Gym == gym)
            {
                client.Gym = null;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }
            
            return LocalRedirect(Url.Content("~/Clients/ShowClients"));
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> AddClientToGym(int? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Client? client = _context.Client.
                Include(a => a.Gym).
                Where(a => a.ClientId == id).
                FirstOrDefault();

            if (client.Gym == null)
            {
                client.Gym = gym;
                _context.Client.Update(client);
                await _context.SaveChangesAsync();
            }      
            
            return LocalRedirect(Url.Content("~/Clients/ShowClients"));
        }
    }
}
