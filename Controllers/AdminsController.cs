using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

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
        public async Task<IActionResult> ShowAllUsers()
        ***REMOVED***
            return View(_context.Users);
    ***REMOVED***

        public async Task<IActionResult> DeleteUserAccount(int? id)
        ***REMOVED***

    ***REMOVED***
***REMOVED***
***REMOVED***
