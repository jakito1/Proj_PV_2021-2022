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
{

    public class GymsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IInteractNotification mockInteractNotification;
        private IIsUserInRoleByUserId _isUserInRoleByUserId;
        private IPhotoManagement _photoManager;

        public GymsControllerTest(NutrifitContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
            mockInteractNotification = Mock.Of<IInteractNotification>();
            _isUserInRoleByUserId = Mock.Of<IIsUserInRoleByUserId>();
            _photoManager = Mock.Of<IPhotoManagement>();

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

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _manager = mockUserManager.Object;
        }

        [Fact]
        public void GymsController_Should_Create()
        {
            GymsController? controller = new GymsController(_context, _manager, _isUserInRoleByUserId, _photoManager, mockInteractNotification);

            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GymsController_EditGymSettings_Should_Return_BadRequest()
        {
            GymsController? controller = new GymsController(_context, _manager, _isUserInRoleByUserId, _photoManager, mockInteractNotification);

            IActionResult? result = await controller.EditGymSettings(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GymsController_EditGymSettingsPost_Should_Return_BadRequest()
        {
            GymsController? controller = new GymsController(_context, _manager, _isUserInRoleByUserId, _photoManager, mockInteractNotification);

            IActionResult? result = await controller.EditGymSettingsPost(null, null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact (Skip = "Broken")]
        public async Task GymsController_Edit_Should_Return_ViewResult()
        {

            GymsController? controller = new GymsController(_context, _manager, _isUserInRoleByUserId, _photoManager, mockInteractNotification);

            IActionResult? result = await controller.EditGymSettingsPost("Test User 1", null);

            Assert.Null(result);
        }

    }
}
