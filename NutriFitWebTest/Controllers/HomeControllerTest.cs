using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest
{
    public class HomeControllerTest
    {
        private readonly HttpContext _httpContext;

        public HomeControllerTest()
        {
            Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(h => h.TraceIdentifier).Returns("Test");
            mockHttpContext.Setup(h => h.Session.Clear());
            _httpContext = mockHttpContext.Object;

        }


        [Fact]
        public void Index_ReturnsViewResult()
        {
            HomeController? controller = new HomeController(null, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };

            IActionResult? result = controller.Index();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Users_ReturnsViewResult()
        {
            HomeController? controller = new HomeController(null, null, null);

            IActionResult? result = controller.Users();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResult()
        {
            HomeController? controller = new HomeController(null, null, null)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };

            IActionResult? result = controller.Error();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
            ErrorViewModel? model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
        }
    }
}