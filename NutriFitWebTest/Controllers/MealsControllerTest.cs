using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Models;
using NutriFitWeb.Services;
using NutriFitWebTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public class MealsControllerTest
    {
        HttpContext _httpContext;
        IPhotoManagement photoManagement;
        List<Meal> exercises = null;

        public MealsControllerTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["_Meals"] = exercises;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _httpContext = mockHttpContext.Object;
        }

        [Fact]
        public void MealsController_Should_Be_Initialized()
        {
            var controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            Assert.NotNull(controller);

        }

        [Fact]
        public void MealsController_EditMeal_Should_Return_NotFound()
        {
            var controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            var result = controller.EditMeal(1);

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public void MealsController_GetCleanCreateMealPartial_Should_Return_PartialViewResult()
        {
            var controller = new MealsController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            var result = controller.GetCleanCreateMealPartial();

            Assert.IsType<PartialViewResult>(result);
        }
    }
}
