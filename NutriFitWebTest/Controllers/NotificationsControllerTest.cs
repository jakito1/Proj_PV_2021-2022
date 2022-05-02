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
{
    public class NotificationsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;

        public NotificationsControllerTest(NutrifitContextFixture contextFixture)
        {
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

            IList<Notification> notificationsList = new List<Notification>
            {
                new Notification()
                {
                    NotificationId = 1,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[0]
                },
                new Notification()
                {
                    NotificationId = 2,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[1]
                },
                new Notification()
                {
                    NotificationId = 3,
                    NotificationMessage = "Test Message",
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = usersList[2]
                }
            };

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();
            var notifications = notificationsList.AsQueryable().BuildMockDbSet();


            mockUserManager.Setup(u => u.Users).Returns(users);

            _context.Notifications = notifications.Object;
            _manager = mockUserManager.Object;
        }

        [Fact]
        public void NotificationsController_Should_Create()
        {
            NotificationsController controller = new NotificationsController(_context, _manager);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task NotificationsController_DeleteNotification_Should_Return_NotFoundResult()
        {
            NotificationsController controller = new NotificationsController(_context, _manager);

            var result = await controller.DeleteNotification(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task NotificationsController_DeleteNotification_Should_Return_RedirectToActionResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            NotificationsController controller = new NotificationsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteNotification(1);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task NotificationsController_RemoveAll_Should_Return_RedirectToActionResult()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            {
                HttpContext = fakeHttpContext.Object
            };

            NotificationsController controller = new NotificationsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.RemoveAll();

            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
