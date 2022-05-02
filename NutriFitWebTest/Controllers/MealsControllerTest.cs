using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using NutriFitWeb.Services;
using NutriFitWebTest.Utils;
using System.Collections.Generic;
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public class MealsControllerTest
    {
        private readonly HttpContext _httpContext;
        private readonly IPhotoManagement photoManagement;
        private readonly List<Meal>? exercises = null;

        public MealsControllerTest()
        {
            Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["_Meals"] = exercises;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _httpContext = mockHttpContext.Object;
            photoManagement = Mock.Of<IPhotoManagement>();
        }

        [Fact]
        public void MealsController_Should_Be_Initialized()
        {
            MealsController? controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            Assert.NotNull(controller);

        }

        [Fact]
        public void MealsController_EditMeal_Should_Return_NotFound()
        {
            MealsController? controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            IActionResult? result = controller.EditMeal(1);

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public void MealsController_GetCleanCreateMealPartial_Should_Return_PartialViewResult()
        {
            MealsController? controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            IActionResult? result = controller.GetCleanCreateMealPartial();

            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public void MealsController_DeleteMeal_Should_Give_PartialView()
        {
            MealsController? controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            IActionResult? result = controller.DeleteMeal(1);

            Assert.IsType<PartialViewResult>(result);
        }
    }
}
