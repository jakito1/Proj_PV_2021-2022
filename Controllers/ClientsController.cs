using Microsoft.AspNetCore.Mvc;

namespace NutriFitWeb.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowClients()
        {
            return View();
        }
    }
}
