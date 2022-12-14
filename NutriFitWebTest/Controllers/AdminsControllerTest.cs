using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace NutriFitWebTest.Controllers
{

    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private IInteractNotification mockInteractNotification;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
            mockInteractNotification = Mock.Of<IInteractNotification>();
        }

        [Fact(Skip = "Doesn't work")]
        public async Task ShowAllUsers_ReturnsViewResult()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);
            DefaultHttpContext? httpContext = new DefaultHttpContext();
            ModelStateDictionary? modelState = new ModelStateDictionary();
            ActionContext? actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            EmptyModelMetadataProvider? modelMetadataProvider = new EmptyModelMetadataProvider();
            ViewDataDictionary? viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            TempDataDictionary? tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            PageContext? pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };

            IActionResult? result = await controller.ShowAllUsers("", "", 1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);

            IActionResult? result = await controller.DeleteUserAccount(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(Skip = "Not a unit test, should be integration")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);
            Trainer mockTrainer = new()
            {
                TrainerId = 1,
                TrainerFirstName = "Luis",
                TrainerLastName = "Carvalho",
                Gym = null,
                UserAccountModel = new UserAccountModel { UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true },
                Clients = null,
            };

            string? trainerId = mockTrainer.UserAccountModel.Id;

            _context.Trainer?.Add(mockTrainer);
            await _context.SaveChangesAsync();

            IActionResult? result = await controller.DeleteUserAccount(trainerId);

            Assert.IsType<LocalRedirectResult>(result);
        }

        [Fact]
        public async Task AdminsController_DeleteUserAccountPost_ReturnsNotFound_WhenAccontDoesntExist()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);

            IActionResult? result = await controller.DeleteUserAccountPost(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AdminsController_EditUserSettings_ReturnsBadRequestResult_WhenIdIsNull()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);

            IActionResult? result = await controller.EditUserSettings(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AdminsController_EditUserSettingsPost_ReturnsBadRequestResult_WhenIdIsNull()
        {
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);

            IActionResult? result = await controller.EditUserSettingsPost(null);

            Assert.IsType<BadRequestResult>(result);

        }

    }
}
