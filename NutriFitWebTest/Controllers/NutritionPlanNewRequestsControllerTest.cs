using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public class NutritionPlanNewRequestsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IInteractNotification mockInteractNotification;

        public NutritionPlanNewRequestsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
            mockInteractNotification = Mock.Of<IInteractNotification>();

            Mock<UserManager<UserAccountModel>>? mockUserManager = new Mock<UserManager<UserAccountModel>>(new Mock<IUserStore<UserAccountModel>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<UserAccountModel>>().Object,
                new IUserValidator<UserAccountModel>[0],
                new IPasswordValidator<UserAccountModel>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<UserAccountModel>>>().Object);

            IList<UserAccountModel> usersList = new List<UserAccountModel>
            {
                new UserAccountModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 1",
                    Email = "testuser1@email.com"
                },
                new UserAccountModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 2",
                    Email = "testuser2@email.com"
                },
                new UserAccountModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 3",
                    Email = "testuser3@email.com"
                }
            };

            IList<Client> clientsList = new List<Client>
            {
                new Client()
                {
                    ClientBirthday = DateTime.Now,
                    ClientFirstName = "Test Client 1",
                    ClientId = 1,
                    ClientLastName = "Last Name",
                    ClientProfilePhoto = null,
                    ClientSex = ClientSex.MALE,
                    DateAddedToGym = DateTime.Now,
                    DateAddedToNutritionist = DateTime.Now,
                    DateAddedToTrainer = DateTime.Now,
                    FatMass = 60,
                    Gym = new Gym(),
                    Height = 175,
                    LeanMass = 15,
                    Nutritionist = new Nutritionist(),
                    NutritionPlanRequests = null,
                    NutritionPlans = null,
                    OtherClientData = "",
                    Trainer = new Trainer(),
                    TrainingPlanRequests = null,
                    TrainingPlans = null,
                    UserAccountModel = usersList[0],
                    WantsNutritionist = true,
                    WantsTrainer = true,
                    Weight = 70
                }
            };

            IList<NutritionPlanNewRequest> plansList = new List<NutritionPlanNewRequest>
            {
                new NutritionPlanNewRequest()
                {
                    Client = clientsList[0],
                    NutritionPlanNewRequestDate = DateTime.Now,
                    NutritionPlanNewRequestDescription = "",
                    NutritionPlanNewRequestDone = false,
                    NutritionPlanNewRequestId = 1,
                    NutritionPlanNewRequestName = "Test"
                },
                new NutritionPlanNewRequest()
                {
                    Client = new Client(),
                    NutritionPlanNewRequestDate = DateTime.Now,
                    NutritionPlanNewRequestDescription = "",
                    NutritionPlanNewRequestDone = false,
                    NutritionPlanNewRequestId = 2,
                    NutritionPlanNewRequestName = "Test2"
                },
                new NutritionPlanNewRequest()
                {
                    Client = new Client(),
                    NutritionPlanNewRequestDate = DateTime.Now,
                    NutritionPlanNewRequestDescription = "",
                    NutritionPlanNewRequestDone = false,
                    NutritionPlanNewRequestId = 3,
                    NutritionPlanNewRequestName = "Test3"
                }
            };

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();
            var plans = plansList.AsQueryable().BuildMockDbSet();
            var clients = clientsList.AsQueryable().BuildMockDbSet();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _context.Client = clients.Object;
            _context.NutritionPlanNewRequests = plans.Object;
            _manager = mockUserManager.Object;
        }

        [Fact]
        public void NutritionPlanNewRequestsController_Should_Create()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_NutritionPlanNewRequestDetails_Should_Return_NotFoundResult()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.NutritionPlanNewRequestDetails(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_NutritionPlanNewRequestDetails_Should_Return_ViewResult()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.NutritionPlanNewRequestDetails(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_NutritionPlanNewRequestDetails_Should_Return_NotFoundResult_WhenNotInDB()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.NutritionPlanNewRequestDetails(10);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void NutritionPlanNewRequestsController_CreateNutritionPlan_Should_Return_ViewResult()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = controller.CreateNutritionPlanNewRequest();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_DeleteNutritionPlanNewRequest_Should_Return_NotFoundResult()
        {
            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.DeleteNutritionPlanNewRequest(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_DeleteNutritionPlanNewRequest_Should_Return_ViewResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteNutritionPlanNewRequest(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NutritionPlanNewRequestsController_DeleteNutritionPlanNewRequestConfirmed_Should_Return_ViewResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            NutritionPlanNewRequestsController controller = new NutritionPlanNewRequestsController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteNutritionPlanNewRequestConfirmed(1);

            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
