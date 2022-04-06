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
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
***REMOVED***
    public class MachinesControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private readonly IPhotoManagement _photoManagement;
        private readonly HttpContext _httpContext;

        public MachinesControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;

            Mock<HttpContext>? mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(h => h.TraceIdentifier).Returns("Test");
            mockHttpContext.Setup(h => h.Session.Clear());
            _httpContext = mockHttpContext.Object;

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
        public void MachinesController_Should_Create()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_MachineDetails_Should_Throw_NotFoundResult()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.MachineDetails(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_MachineDetails_Should_Throw_NotFoundResult_On_Null_Machine()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.MachineDetails(1);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public void MachinesController_CreateMachine_Should_Return_ViewResult()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = controller.CreateMachine();

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_EditMachine_Should_Throw_NotFoundResult()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.EditMachine(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_EditMachine_Should_Throw_NotFoundResult_On_Null_Machine()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.EditMachine(1);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_EditMachinePost_Should_Throw_NotFoundResult()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.EditMachinePost(null, null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_DeleteMachine_Should_Throw_NotFoundResult()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.DeleteMachine(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task MachinesController_DeleteMachine_Should_Throw_NotFoundResult_On_Null_Machine()
        ***REMOVED***
            MachinesController? controller = new MachinesController(_context, _manager, _photoManagement);

            IActionResult? result = await controller.DeleteMachine(1);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***
***REMOVED***
***REMOVED***
