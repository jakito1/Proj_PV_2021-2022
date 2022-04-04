using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NutriFitWeb.Controllers;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using Xunit;

namespace NutriFitWebTest
***REMOVED***
    public static class QueryableExtensions
    ***REMOVED***
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> input)
        ***REMOVED***
            return new NotInDbSet<T>(input);
    ***REMOVED***
***REMOVED***

    public class NotInDbSet<T> : IQueryable<T>, IAsyncEnumerable<T>, IEnumerable<T>, IEnumerable
    ***REMOVED***
        private readonly List<T> _innerCollection;

        public NotInDbSet(IEnumerable<T> innerCollection)
        ***REMOVED***
            _innerCollection = innerCollection.ToList();
    ***REMOVED***

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        ***REMOVED***
            return new AsyncEnumerator(GetEnumerator());
    ***REMOVED***

        public IEnumerator<T> GetEnumerator()
        ***REMOVED***
            return _innerCollection.GetEnumerator();
    ***REMOVED***

        IEnumerator IEnumerable.GetEnumerator()
        ***REMOVED***
            return GetEnumerator();
    ***REMOVED***

        public class AsyncEnumerator : IAsyncEnumerator<T>
        ***REMOVED***
            private readonly IEnumerator<T> _enumerator;

            public AsyncEnumerator(IEnumerator<T> enumerator)
            ***REMOVED***
                _enumerator = enumerator;
        ***REMOVED***

            public ValueTask DisposeAsync()
            ***REMOVED***
                return new ValueTask();
        ***REMOVED***

            public ValueTask<bool> MoveNextAsync()
            ***REMOVED***
                return new ValueTask<bool>(_enumerator.MoveNext());
        ***REMOVED***

            public T Current => _enumerator.Current;
    ***REMOVED***

        public Type ElementType => typeof(T);
        public Expression Expression => Expression.Empty();
        public IQueryProvider Provider => new EnumerableQuery<T>(Expression);
***REMOVED***
    
    public class ClientsControllerTest : IClassFixture<NutrifitContextFixture>
    ***REMOVED***
        private ApplicationDbContext _context;
        private UserManager<UserAccountModel> _manager;

        public ClientsControllerTest(NutrifitContextFixture contextFixture)
        ***REMOVED***
            _context = contextFixture.DbContext;

            var mockUserManager = new Mock<UserManager<UserAccountModel>>(new Mock<IUserStore<UserAccountModel>>().Object,
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

            var users = usersList.AsAsyncQueryable();

            mockUserManager.Setup(u => u.Users).Returns(users);

            _manager = mockUserManager.Object;
    ***REMOVED***

        [Fact]
        public async Task Clients_TestShowClientsReturnsViewOnNullEmail()
        ***REMOVED***
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = Mock.Of<HttpContext>(ctx => ctx.User.Identity.Name == "User")
        ***REMOVED***;
            var controller = new ClientsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.ShowClients(null);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task Clients_TestShowClientsReturnsViewOnNonNullEmail()
        ***REMOVED***
            var controllerContext = new ControllerContext()
            ***REMOVED***
                HttpContext = Mock.Of<HttpContext>(ctx => ctx.User.Identity.Name == "User")
        ***REMOVED***;
            var controller = new ClientsController(_context, _manager);
            controller.ControllerContext = controllerContext;

            var result = await controller.ShowClients("testuser1@email.com");

            Assert.IsType<ViewResult>(result);
    ***REMOVED***

        [Fact]
        public async Task Clients_TestClientDetailsNotFoundOnNullDetails()
        ***REMOVED***
            var controller = new ClientsController(_context, _manager);

            var result = await controller.ClientDetails(null);

            Assert.IsType<NotFoundResult>(result);
    ***REMOVED***

        [Fact]
        public async Task Clients_TestClientDetailsReturnsViewResult()
        ***REMOVED***
            var controller = new ClientsController(_context, _manager);

            Client mockClient = new Client()
            ***REMOVED***
                ClientId = 1
        ***REMOVED***;

            var clientId = mockClient.ClientId;

            var result = await controller.ClientDetails(clientId);

            Assert.IsType<ViewResult>(result);
    ***REMOVED***
***REMOVED***
    
***REMOVED***

