using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
***REMOVED***
    public class TrainersControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IIsUserInRoleByUserId _userInRole;
        private IPhotoManagement _photoManager;
        private IInteractNotification mockInteractNotification;

        public TrainersControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;
            mockInteractNotification = Mock.Of<IInteractNotification>();
            _photoManager = Mock.Of<IPhotoManagement>();
            _userInRole = Mock.Of<IIsUserInRoleByUserId>();

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
            ***REMOVED***
                new UserAccountModel()
                ***REMOVED***
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 1",
                    Email = "testuser1@email.com"
              ***REMOVED***
                new UserAccountModel()
                ***REMOVED***
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 2",
                    Email = "testuser2@email.com"
              ***REMOVED***
                new UserAccountModel()
                ***REMOVED***
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Test User 3",
                    Email = "testuser3@email.com"
            ***REMOVED***
        ***REMOVED***;

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _manager = mockUserManager.Object;
    ***REMOVED***

        [Fact]
        public void TrainersController_Should_Create()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact (Skip = "Broken")]
        public async Task TrainersController_ShowTrainers_Should_Return_ActionResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.ShowTrainers("", "testuser1@email.com", 1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainersController_TrainersDetails_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.TrainerDetails(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainersController_TrainersDetails_Should_Return_ViewResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.TrainerDetails(1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainersController_EditTrainerSettings_Should_Return_BadRequestResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.EditTrainerSettings(null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact (Skip ="Broken")]
        public async Task TrainersController_EditTrainerSettings_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);
            
            var result = await controller.EditTrainerSettings("1");

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainersController_EditTrainerSettingsPost_Should_Return_BadRequestResult()
        ***REMOVED***
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.EditTrainerSettingsPost(null, null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***

