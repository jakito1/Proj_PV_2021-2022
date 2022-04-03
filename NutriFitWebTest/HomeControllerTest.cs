using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest
{
    public class HomeControllerTest
    {

        [Fact]
        public void Index_ReturnsViewResult()
        {


            var controller = new HomeController(null, null, null, null);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var controller = new HomeController(null, null, null, null);

            var result = controller.Privacy();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Users_ReturnsViewResult()
        {
            var controller = new HomeController(null, null, null, null);

            var result = controller.Users();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResult()
        {
            var controller = new HomeController(new NullLogger<HomeController>(), null, null, null);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(h => h.TraceIdentifier).Returns("Test");
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
        }
    }
}