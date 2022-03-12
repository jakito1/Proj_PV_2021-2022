using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System.Diagnostics;

namespace NutriFitWeb.Controllers
***REMOVED***
    /// <summary>
    /// HomeController class, derived from Controller.
    /// </summary>
    public class HomeController : Controller
    ***REMOVED***
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Build the HomeController to be used on the main page.
        /// </summary>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="roleManager">Provides the APIs for managing roles in a persistence store.</param>
        public HomeController(ILogger<HomeController> logger, UserManager<UserAccountModel> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        ***REMOVED***
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            try
            ***REMOVED***
                SeedData.Seed(userManager, roleManager, context).Wait();
        ***REMOVED*** catch
            ***REMOVED***
              
        ***REMOVED***
    ***REMOVED***

        /// <summary>
        /// Redirects to the Index page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        ***REMOVED***
            return View();
    ***REMOVED***

        /// <summary>
        /// Redirects to the Privacy page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        ***REMOVED***
            return View();
    ***REMOVED***

        public IActionResult Users()
        ***REMOVED***
            return View();
    ***REMOVED***

        /// <summary>
        /// When an error occurs.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        ***REMOVED***
            return View(new ErrorViewModel ***REMOVED*** RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier ***REMOVED***);
    ***REMOVED***
***REMOVED***
***REMOVED***