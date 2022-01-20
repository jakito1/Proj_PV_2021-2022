using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System.Diagnostics;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class HomeController : Controller
    ***REMOVED***
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager)
        ***REMOVED***
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            SeedData.Seed(userManager, roleManager).Wait();
    ***REMOVED***

        public IActionResult Index()
        ***REMOVED***
            return View();
    ***REMOVED***

        public IActionResult Privacy()
        ***REMOVED***
            return View();
    ***REMOVED***

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        ***REMOVED***
            return View(new ErrorViewModel ***REMOVED*** RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier ***REMOVED***);
    ***REMOVED***
***REMOVED***
***REMOVED***