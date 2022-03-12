using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class GymsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        public GymsController(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        public IActionResult Index()
        ***REMOVED***
            return View();
    ***REMOVED***
***REMOVED***
***REMOVED***
