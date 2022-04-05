using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class HomeControllerTest
    ***REMOVED***
        private HttpContext _httpContext;

        public HomeControllerTest()
        ***REMOVED***
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(h => h.TraceIdentifier).Returns("Test");
            mockHttpContext.Setup(h => h.Session.Clear());
            _httpContext = mockHttpContext.Object;
            
    ***REMOVED***


        [Fact]
        public void Index_ReturnsViewResult()
        ***REMOVED***
            var controller = new HomeController(null, null, null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = _httpContext;

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***


        [Fact]
        public void Users_ReturnsViewResult()
        ***REMOVED***
            var controller = new HomeController(null, null, null);

            var result = controller.Users();

            var viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public void Error_ReturnsViewResult()
        ***REMOVED***
            var controller = new HomeController(null, null, null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = _httpContext;

            var result = controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
    ***REMOVED***
***REMOVED***
***REMOVED***