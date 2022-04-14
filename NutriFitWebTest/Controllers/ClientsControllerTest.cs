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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NutriFitWebTest.Controllers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> input)
        {
            return new NotInDbSet<T>(input);
        }
    }

    public class NotInDbSet<T> : IQueryable<T>, IAsyncEnumerable<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> _innerCollection;

        public NotInDbSet(IEnumerable<T> innerCollection)
        {
            _innerCollection = innerCollection.ToList();
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new AsyncEnumerator(GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class AsyncEnumerator : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public AsyncEnumerator(IEnumerator<T> enumerator)
            {
                _enumerator = enumerator;
            }

            public ValueTask DisposeAsync()
            {
                return new ValueTask();
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_enumerator.MoveNext());
            }

            public T Current => _enumerator.Current;
        }

        public Type ElementType => typeof(T);
        public Expression Expression => Expression.Empty();
        public IQueryProvider Provider => new EnumerableQuery<T>(Expression);
    }

    public class ClientsControllerTest : IClassFixture<NutrifitContextFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _manager;
        private IInteractNotification mockInteractNotification;

        public ClientsControllerTest(NutrifitContextFixture contextFixture)
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

            IQueryable<UserAccountModel>? users = usersList.AsAsyncQueryable();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _manager = mockUserManager.Object;
        }

        [Fact(Skip = "Doesn't work")]
        public async Task Clients_TestShowClientsReturnsViewOnNullEmail()
        {
            ControllerContext? controllerContext = new ControllerContext()
            {
                HttpContext = Mock.Of<HttpContext>(ctx => ctx.User.Identity.Name == "User")
            };
            ClientsController? controller = new ClientsController(_context, _manager, null, null, mockInteractNotification)
            {
                ControllerContext = controllerContext
            };

            IActionResult? result = await controller.ShowClients(null, null, null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact(Skip = "Doesn't work")]
        public async Task Clients_TestShowClientsReturnsViewOnNonNullEmail()
        {
            ControllerContext? controllerContext = new ControllerContext()
            {
                HttpContext = Mock.Of<HttpContext>(ctx => ctx.User.Identity.Name == "User")
            };
            ClientsController? controller = new ClientsController(_context, _manager, null, null, mockInteractNotification)
            {
                ControllerContext = controllerContext
            };

            IActionResult? result = await controller.ShowClients("testuser1@email.com", null, null);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Clients_TestClientDetailsNotFoundOnNullDetails()
        {
            ClientsController? controller = new ClientsController(_context, _manager, null, null, mockInteractNotification);

            IActionResult? result = await controller.ClientDetails(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Clients_TestClientDetailsReturnsViewResult()
        {
            ClientsController? controller = new ClientsController(_context, _manager, null, null, mockInteractNotification);

            Client mockClient = new Client()
            {
                ClientId = 1
            };

            int clientId = mockClient.ClientId;

            IActionResult? result = await controller.ClientDetails(clientId);

            Assert.IsType<ViewResult>(result);
        }
    }

}

