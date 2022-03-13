using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Models;
using Microsoft.AspNetCore.Identity;

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

        public async Task<IActionResult> ShowClients()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            var returnQuery = _context.Client.Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccount).
                Where(a => a.Gym.UserAccount.Id == user.Id || a.Gym.UserAccount.Id != user.Id).
                OrderByDescending(a => a.Gym);
            return View(await returnQuery.ToListAsync());
        }

        

        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            return View(await _context.Client.Include(a => a.Gym).Where(a => a.ClientId == id).FirstOrDefaultAsync());
        }

        public async Task<IActionResult> RemoveClientFromGym(int? id)
        {
            Client? client = _context.Client.Where(a => a.ClientId == id).FirstOrDefault();
            client.Gym = null;
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
            return LocalRedirect(Url.Content("~/Clients/ShowClients"));
        }

    }
}
