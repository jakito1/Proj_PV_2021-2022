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
using System.Threading.Tasks;
using Xunit;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace NutriFitWebTest.Controllers
***REMOVED***

    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private IInteractNotification mockInteractNotification;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;
            mockInteractNotification = Mock.Of<IInteractNotification>();
    ***REMOVED***

        [Fact(Skip = "Doesn't work")]
        public async Task ShowAllUsers_ReturnsViewResult()
        ***REMOVED***
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);
            DefaultHttpContext? httpContext = new DefaultHttpContext();
            ModelStateDictionary? modelState = new ModelStateDictionary();
            ActionContext? actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            EmptyModelMetadataProvider? modelMetadataProvider = new EmptyModelMetadataProvider();
            ViewDataDictionary? viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            TempDataDictionary? tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            PageContext? pageContext = new PageContext(actionContext)
            ***REMOVED***
                ViewData = viewData
        ***REMOVED***;

            IActionResult? result = await controller.ShowAllUsers("", "", 1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        ***REMOVED***
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);

            IActionResult? result = await controller.DeleteUserAccount(null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact(Skip = "Not a unit test, should be integration")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        ***REMOVED***
            AdminsController? controller = new AdminsController(_context, mockInteractNotification);
            Trainer mockTrainer = new()
            ***REMOVED***
                TrainerId = 1,
                TrainerFirstName = "Luis",
                TrainerLastName = "Carvalho",
                Gym = null,
                UserAccountModel = new UserAccountModel ***REMOVED*** UserName = "trainer", Email = "trainer@trainer.pt", EmailConfirmed = true ***REMOVED***,
                Clients = null,
        ***REMOVED***;

            string? trainerId = mockTrainer.UserAccountModel.Id;

            _context.Trainer?.Add(mockTrainer);
            await _context.SaveChangesAsync();

            IActionResult? result = await controller.DeleteUserAccount(trainerId);

            Assert.IsType<LocalRedirectResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***
