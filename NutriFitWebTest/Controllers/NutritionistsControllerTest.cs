using Microsoft.AspNetCore.Http;
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
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
***REMOVED***
    public class NutritionistsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IIsUserInRoleByUserId _userInRole;
        private IPhotoManagement _photoManager;
        private IInteractNotification mockInteractNotification;

        public NutritionistsControllerTest(NutrifitContextFixture contextFixture)
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
        public void NutritionistsController_Should_Create()
        ***REMOVED***
            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact]
        public async Task NutritionistsController_Details_Should_Return_NotFoundResult()
        ***REMOVED***
            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.NutritionistDetails(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task NutritionistsController_Details_Should_Return_ViewResult()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.NutritionistDetails(1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task NutritionistsController_EditNutritionistSettings_Should_Return_BadRequestResult()
        ***REMOVED***
            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.EditNutritionistSettings(null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact (Skip = "Broken")]
        public async Task NutritionistsController_EditNutritionistSettings_Should_Return_ViewResult()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.EditNutritionistSettings("test id");

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task NutritionistsController_EditNutritionistSettingsPost_Should_Return_BadRequestResult()
        ***REMOVED***
            NutritionistsController controller = new NutritionistsController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            var result = await controller.EditNutritionistSettingsPost(null, null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***

