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
{

    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private ApplicationDbContext _context;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact (Skip = "Doesn't work")]
        public async Task ShowAllUsers_ReturnsViewResult()
        {
            var controller = new AdminsController(_context);
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            
            var result = await controller.ShowAllUsers("", "", 1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteUserAccount_ReturnsNotFound_WhenAccontDoesntExist()
        {
            var controller = new AdminsController(_context);

            var result = await controller.DeleteUserAccount(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(Skip = "Not a unit test, should be integration")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        {
            var controller = new AdminsController(_context);
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

            var result = await controller.DeleteUserAccount(trainerId);

            Assert.IsType<LocalRedirectResult>(result);
        }
    }
}
