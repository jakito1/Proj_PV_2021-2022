﻿using Microsoft.AspNetCore.Http;
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
***REMOVED***
    public class TrainingPlansControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IInteractNotification mockInteractNotification;

        public TrainingPlansControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
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

            IList<Client> clientsList = new List<Client>
            ***REMOVED***
                new Client()
                ***REMOVED***
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
            ***REMOVED***
        ***REMOVED***;

            IList<TrainingPlan> plansList = new List<TrainingPlan>
            ***REMOVED***
                new TrainingPlan()
                ***REMOVED***
                    Client = clientsList[0],
                    ClientEmail = usersList[0].Email,
                    Exercise = new Exercise(),
                    Exercises = new List<Exercise>(),
                    ToBeEdited = true,
                    Trainer = new Trainer(),
                    TrainingPlanDescription = "Test",
                    TrainingPlanEditRequest = new TrainingPlanEditRequest(),
                    TrainingPlanId = 1,
                    TrainingPlanName = "Test",
                    TrainingPlanNewRequest = new TrainingPlanNewRequest(),
                    TrainingPlanNewRequestId = 1
            ***REMOVED***
        ***REMOVED***;

            IList<Trainer> trainersList = new List<Trainer>
            ***REMOVED***
                new Trainer()
                ***REMOVED***
                    Clients = clientsList.ToList(),
                    Gym = new Gym(),
                    TrainerFirstName = "Trainer",
                    TrainerId = 1,
                    TrainerLastName = "Last",
                    TrainerProfilePhoto = null,
                    TrainingPlans = plansList.ToList(),
                    UserAccountModel = new UserAccountModel()
            ***REMOVED***
        ***REMOVED***;

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();
            var plans = plansList.AsQueryable().BuildMockDbSet();
            var clients = clientsList.AsQueryable().BuildMockDbSet();
            var trainers = trainersList.AsQueryable().BuildMockDbSet();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _context.Client = clients.Object;
            _context.Trainer = trainers.Object;
            _context.TrainingPlan = plans.Object;
            _manager = mockUserManager.Object;
    ***REMOVED***

        [Fact]
        public void TrainingPlansController_Should_Create_Controller()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            Assert.NotNull(controller);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_TrainingPlanDetails_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            var result = await controller.TrainingPlanDetails(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_TrainingPlanDetails_Should_Return_ViewResult()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.TrainingPlanDetails(1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_TrainingPlanDetails_Should_Return_NotFoundResult_IfNotInDB()
        ***REMOVED***
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Test User 1");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = fakeHttpContext.Object
        ***REMOVED***;

            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);
            controller.ControllerContext = controllerContext;

            var result = await controller.TrainingPlanDetails(10);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_EditTrainingPlan_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            var result = await controller.EditTrainingPlan(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_EditTrainingPlanPost_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            var result = await controller.EditTrainingPlanPost(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_DeleteTrainingPlan_Should_Return_NotFoundResult()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            var result = await controller.DeleteTrainingPlan(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task TrainingPlansController_DeleteTrainingPlan_Should_Return_ViewResult()
        ***REMOVED***
            TrainingPlansController controller = new TrainingPlansController(_context, _manager, mockInteractNotification);

            var result = await controller.DeleteTrainingPlan(1);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

***REMOVED***
***REMOVED***
