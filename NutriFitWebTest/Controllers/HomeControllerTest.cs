using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest.Controllers
***REMOVED***
    public class HomeControllerTest
    ***REMOVED***
        private readonly HttpContext _httpContext;

        public HomeControllerTest()
        ***REMOVED***
            Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(h => h.TraceIdentifier).Returns("Test");
            mockHttpContext.Setup(h => h.Session.Clear());
            _httpContext = mockHttpContext.Object;

    ***REMOVED***


        [Fact]
        public void Index_ReturnsViewResult()
        ***REMOVED***
            HomeController? controller = new HomeController(null, null, null)
            ***REMOVED***
                ControllerContext = new ControllerContext
                ***REMOVED***
                    HttpContext = _httpContext
            ***REMOVED***
        ***REMOVED***;

            IActionResult? result = controller.Index();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***


        [Fact]
        public void Users_ReturnsViewResult()
        ***REMOVED***
            HomeController? controller = new HomeController(null, null, null);

            IActionResult? result = controller.Users();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public void Error_ReturnsViewResult()
        ***REMOVED***
            HomeController? controller = new HomeController(null, null, null)
            ***REMOVED***
                ControllerContext = new ControllerContext
                ***REMOVED***
                    HttpContext = _httpContext
            ***REMOVED***
        ***REMOVED***;

            IActionResult? result = controller.Error();

            ViewResult? viewResult = Assert.IsType<ViewResult>(result);
            ErrorViewModel? model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
    ***REMOVED***
***REMOVED***
***REMOVED***