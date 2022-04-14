using Microsoft.AspNetCore.Identity;
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
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public class TrainersControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IIsUserInRoleByUserId _userInRole;
        private IPhotoManagement _photoManager;
        private IInteractNotification mockInteractNotification;

        public TrainersControllerTest(NutrifitContextFixture contextFixture)
        {
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
        public void TrainersController_Should_Create()
        {
            TrainersController controller = new TrainersController(_context, _manager, _userInRole, _photoManager, mockInteractNotification);

            Assert.NotNull(controller);
        }
    }
}

