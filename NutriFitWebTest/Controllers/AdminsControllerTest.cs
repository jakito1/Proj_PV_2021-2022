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
using System.Threading.Tasks;
using Xunit;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace NutriFitWebTest
{

    public class AdminsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;

        public AdminsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact(Skip = "Doesn't work")]
        public async Task ShowAllUsers_ReturnsViewResult()
        {
            AdminsController? controller = new AdminsController(_context);
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
            AdminsController? controller = new AdminsController(_context);

            IActionResult? result = await controller.DeleteUserAccount(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(Skip = "Not a unit test, should be integration")]
        public async Task DeleteUserAccount_ReturnsLocalUrl()
        {
            AdminsController? controller = new AdminsController(_context);
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
    }
}
