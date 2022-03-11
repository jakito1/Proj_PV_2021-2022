using Microsoft.AspNetCore.Mvc;

namespace NutriFitWeb.Controllers
{
    public class GymsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
