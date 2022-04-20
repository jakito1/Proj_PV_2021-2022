using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
***REMOVED***
    public class NotificationsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;

        public NotificationsControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;

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

            IList<Notification> notificationsList = new List<Notification>
            ***REMOVED***
                new Notification()
                ***REMOVED***
                    NotificationId = 1,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[0]
              ***REMOVED***
                new Notification()
                ***REMOVED***
                    NotificationId = 2,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[1]
              ***REMOVED***
                new Notification()
                ***REMOVED***
                    NotificationId = 3,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[2]
            ***REMOVED***
        ***REMOVED***;

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();
            var notifications = notificationsList.AsQueryable().BuildMockDbSet();


            mockUserManager.Setup(u => u.Users).Returns(users);

            _context.Notifications = notifications.Object;
            _manager = mockUserManager.Object;
    ***REMOVED***

        [Fact]
        public void NotificationsController_Should_Create()
        ***REMOVED***
            NotificationsController controller = new NotificationsController(_context, _manager);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact]
        public async Task NotificationsController_DeleteNotification_Should_Return_NotFoundResult()
        ***REMOVED***
            NotificationsController controller = new NotificationsController(_context, _manager);

            var result = await controller.DeleteNotification(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task NotificationsController_DeleteNotification_Should_Return_RedirectToActionResult()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            NotificationsController controller = new NotificationsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteNotification(1);

            Assert.IsType<RedirectToActionResult>(result);
    ***REMOVED***

        [Fact]
        public async Task NotificationsController_RemoveAll_Should_Return_RedirectToActionResult()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            NotificationsController controller = new NotificationsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.RemoveAll();

            Assert.IsType<RedirectToActionResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***
