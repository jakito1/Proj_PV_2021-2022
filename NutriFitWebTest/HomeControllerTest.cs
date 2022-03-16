using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Controllers;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public class HomeControllerTest
    ***REMOVED***

        [Fact]
        public void Index_ReturnsViewResult()
        ***REMOVED***


            var controller = new HomeController(null, null, null, null);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public void Privacy_ReturnsViewResult()
        ***REMOVED***
            var controller = new HomeController(null, null, null, null);

            var result = controller.Privacy();

            var viewResult = Assert.IsType<ViewResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***