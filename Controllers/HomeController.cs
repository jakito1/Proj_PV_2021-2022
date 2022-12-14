using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System.Diagnostics;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// HomeController class, derived from Controller.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Build the HomeController to be used on the main page.
        /// </summary>
        /// <param name="logger">A generic interface for logging where the category name is derived from this class.</param>
        /// <param name="userManager">Provides the APIs for managing the UserAccountModel in a persistence store.</param>
        /// <param name="roleManager">Provides the APIs for managing roles in a persistence store.</param>
        public HomeController(UserManager<UserAccountModel> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            try
            {
                SeedData.Seed(userManager, roleManager, context).Wait();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Redirects to the Index page.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        /// <summary>
        /// Redirects to the Users page.
        /// </summary>
        /// <returns>ViewResult</returns>
        public IActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// Redirects to Error Page when an error occurs.
        /// </summary>
        /// <returns>ViewResult</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}