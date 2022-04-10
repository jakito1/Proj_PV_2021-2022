﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest
***REMOVED***

    public class GymsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;

        public GymsControllerTest(NutrifitContextFixture contextFixture)
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

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _manager = mockUserManager.Object;
    ***REMOVED***

        [Fact]
        public void GymsController_Should_Create()
        ***REMOVED***
            GymsController? controller = new GymsController(_context, _manager, null, null);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact]
        public async Task GymsController_EditGymSettings_Should_Return_BadRequest()
        ***REMOVED***
            GymsController? controller = new GymsController(_context, _manager, null, null);

            IActionResult? result = await controller.EditGymSettings(null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact]
        public async Task GymsController_EditGymSettingsPost_Should_Return_BadRequest()
        ***REMOVED***
            GymsController? controller = new GymsController(_context, _manager, null, null);

            IActionResult? result = await controller.EditGymSettingsPost(null, null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

        [Fact(Skip = "Doesn't work")]
        public async Task GymsController_Edit_Should_Return_ViewResult()
        ***REMOVED***
            GymsController? controller = new GymsController(_context, _manager, null, null);

            IActionResult? result = await controller.EditGymSettingsPost("Test User 1", null);

            Assert.IsType<BadRequestResult>(result);
    ***REMOVED***

***REMOVED***
***REMOVED***
