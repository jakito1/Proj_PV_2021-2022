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
***REMOVED***
    public class ExercisesControllerTest
    ***REMOVED***
        private readonly HttpContext _httpContext;
        private readonly IPhotoManagement photoManagement;
        private readonly List<Exercise> exercises = null;

        public ExercisesControllerTest()
        ***REMOVED***
            Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
            MockHttpSession mockSession = new MockHttpSession();
            mockSession["_Exercises"] = exercises;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _httpContext = mockHttpContext.Object;
            photoManagement = Mock.Of<IPhotoManagement>();
    ***REMOVED***

        [Fact]
        public void ExercisesController_Should_Be_Initialized()
        ***REMOVED***
            ExercisesController? controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            Assert.NotNull(controller);

    ***REMOVED***

        [Fact]
        public void ExercisesController_ShowExerciseList_Should_Return_PartialView()
        ***REMOVED***
            ExercisesController? controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            IActionResult? result = controller.ShowExercisesList();

            Assert.IsType<PartialViewResult>(result);
    ***REMOVED***

        [Fact]
        public void ExercisesController_GetCleanCreateExercisePartial_Should_Return_PartialView()
        ***REMOVED***
            ExercisesController? controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            IActionResult? result = controller.GetCleanCreateExercisePartial();

            Assert.IsType<PartialViewResult>(result);
    ***REMOVED***

        [Fact]
        public void ExercisesController_EditExercise_Should_Return_NotFound()
        ***REMOVED***
            ExercisesController? controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;

            IActionResult? result = controller.EditExercise(1);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public void ExercisesController_DeleteExercise_Should_Return_PartialView()
        ***REMOVED***
            ExercisesController? controller = new ExercisesController(photoManagement);
            controller.ControllerContext.HttpContext = _httpContext;


            IActionResult? result = controller.DeleteExercise(1);

            Assert.IsType<PartialViewResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***
