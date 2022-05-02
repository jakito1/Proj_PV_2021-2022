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
    public class TrainingPlanNewRequestsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IInteractNotification mockInteractNotification;

        public TrainingPlanNewRequestsControllerTest(NutrifitContextFixture contextFixture)
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
                    TrainingPlanRequests = null,
                    TrainingPlans = null,
                    OtherClientData = "",
                    Trainer = new Trainer(),
                    NutritionPlanRequests = null,
                    NutritionPlans = null,
                    UserAccountModel = usersList[0],
                    WantsNutritionist = true,
                    WantsTrainer = true,
                    Weight = 70
                }
            };

            IList<TrainingPlanNewRequest> plansList = new List<TrainingPlanNewRequest>
            {
                new TrainingPlanNewRequest()
                {
                    Client = clientsList[0],
                    TrainingPlanNewRequestDate = DateTime.Now,
                    TrainingPlanNewRequestDescription = "",
                    TrainingPlanNewRequestDone = false,
                    TrainingPlanNewRequestId = 1,
                    TrainingPlanNewRequestName = "Test"
                },
                new TrainingPlanNewRequest()
                {
                    Client = new Client(),
                    TrainingPlanNewRequestDate = DateTime.Now,
                    TrainingPlanNewRequestDescription = "",
                    TrainingPlanNewRequestDone = false,
                    TrainingPlanNewRequestId = 2,
                    TrainingPlanNewRequestName = "Test2"
                },
                new TrainingPlanNewRequest()
                {
                    Client = new Client(),
                    TrainingPlanNewRequestDate = DateTime.Now,
                    TrainingPlanNewRequestDescription = "",
                    TrainingPlanNewRequestDone = false,
                    TrainingPlanNewRequestId = 3,
                    TrainingPlanNewRequestName = "Test3"
                }
            };

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();
            var plans = plansList.AsQueryable().BuildMockDbSet();
            var clients = clientsList.AsQueryable().BuildMockDbSet();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _context.Client = clients.Object;
            _context.TrainingPlanNewRequests = plans.Object;
            _manager = mockUserManager.Object;
        }

        [Fact]
        public void TrainingPlanNewRequestsController_Should_Create()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            Assert.NotNull(controller);
        }


        [Fact]
        public async Task TrainingPlanNewRequestsController_TrainingPlanNewRequestDetails_Should_Return_NotFoundResult()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.TrainingPlanNewRequestDetails(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task TrainingPlanNewRequestsController_TrainingPlanNewRequestDetails_Should_Return_ViewResult()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.TrainingPlanNewRequestDetails(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task TrainingPlanNewRequestsController_TrainingPlanNewRequestDetails_Should_Return_NotFoundResult_WhenNotInDB()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.TrainingPlanNewRequestDetails(10);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TrainingPlanNewRequestsController_CreateTrainingPlan_Should_Return_ViewResult()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = controller.CreateTrainingPlanNewRequest();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task TrainingPlanNewRequestsController_DeleteTrainingPlanNewRequest_Should_Return_NotFoundResult()
        {
            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);

            var result = await controller.DeleteTrainingPlanNewRequest(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task TrainingPlanNewRequestsController_DeleteTrainingPlanNewRequest_Should_Return_ViewResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteTrainingPlanNewRequest(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task TrainingPlanNewRequestsController_DeleteTrainingPlanNewRequestConfirmed_Should_Return_ViewResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            TrainingPlanNewRequestsController controller = new TrainingPlanNewRequestsController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteTrainingPlanNewRequestConfirmed(1);

            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
