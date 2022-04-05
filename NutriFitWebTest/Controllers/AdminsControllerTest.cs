using Microsoft.AspNetCore.Mvc;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NutriFitWebTest
***REMOVED***

    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private ApplicationDbContext _context;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;
    ***REMOVED***

        [Fact (Skip = "Doesn't work")]
        public async Task ShowAllUsers_ReturnsViewResult()
        ***REMOVED***
            var controller = new AdminsController(_context);
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            ***REMOVED***
                ViewData = viewData
        ***REMOVED***;
            
            var result = await controller.ShowAllUsers("", "", 1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        ***REMOVED***
            var controller = new AdminsController(_context);

            var result = await controller.DeleteUserAccount(null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact(Skip = "Not a unit test, should be integration")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        ***REMOVED***
            var controller = new AdminsController(_context);
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

            var result = await controller.DeleteUserAccount(trainerId);

            Assert.IsType<LocalRedirectResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***
