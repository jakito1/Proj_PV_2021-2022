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
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public class ExercisesControllerTest
    {
        HttpContext _httpContext;
        IPhotoManagement photoManagement;
        List<Exercise> exercises = new List<Exercise>();

        public ExercisesControllerTest()
        {
            var mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["_Exercises"] = exercises;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _httpContext = mockHttpContext.Object;
        }

        [Fact]
        public void ExercisesController_Should_Be_Initialized()
        {
            var controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;
            
            
            Assert.NotNull(controller);
            
        }

        [Fact (Skip ="Doesn't work")]
        public void ExercisesController_ShowExerciseList_Should_Return_PartialView()
        {
            var controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            var result = controller.ShowExercisesList();

            Assert.IsType<PartialViewResult>(result);
        }

        [Fact]
        public void ExercisesController_GetCleanCreateExercisePartial_Should_Return_PartialView()
        {
            var controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            var result = controller.GetCleanCreateExercisePartial();

            Assert.IsType<PartialViewResult>(result);
        }
    }
}
