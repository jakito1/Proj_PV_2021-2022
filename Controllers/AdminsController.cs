using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

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
        public async Task<IActionResult> ShowAllUsers()
        {
            return View(_context.Users);
        }

        public async Task<IActionResult> DeleteUserAccount(int? id)
        {

        }
    }
}
