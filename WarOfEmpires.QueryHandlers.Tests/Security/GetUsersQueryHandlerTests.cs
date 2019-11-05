using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUsersQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public User GetUser(int id, UserStatus userStatus, string email, string displayName, string description, bool showEmail, params DateTime[] eventDates) {
            var user = Substitute.For<User>();

            user.Id.Returns(id);
            user.Status.Returns(userStatus);
            user.Email.Returns(email);
            user.DisplayName.Returns(displayName);
            user.Description.Returns(description);
            user.ShowEmail.Returns(showEmail);

            foreach (var eventDate in eventDates) {
                var evnt = Substitute.For<UserEvent>();

                evnt.Date.Returns(eventDate);

                user.UserEvents.Add(evnt);
            }

            return user;
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_All_Active_Users() {
            for (var i = 0; i < 9; i++) {
                _context.Users.Add(GetUser(i + 1, (UserStatus)(i % 3 + 1), $"test{i}@test.com", null, null, false, new DateTime(2019, 1, 1)));
            }

            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            var results = handler.Execute(query);

            results.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Id() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, false));

            var results = handler.Execute(query);

            results.Single().Id.Should().Be(1);
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_DisplayName_If_Available() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", "Test display name", null, false));

            var results = handler.Execute(query);

            results.Single().DisplayName.Should().Be("Test display name");
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Anonymous_DisplayName_If_Not_Available() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, false));

            var results = handler.Execute(query);

            results.Single().DisplayName.Should().Be("Anonymous");
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Email_If_ShowEmail_Is_True() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, true));

            var results = handler.Execute(query);

            results.Single().Email.Should().Be("test@test.com");
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Private_If_ShowEmail_Is_False() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, false));

            var results = handler.Execute(query);

            results.Single().Email.Should().Be("Private");
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Description_If_Available() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, "A description", false));

            var results = handler.Execute(query);

            results.Single().Description.Should().Be("A description");
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_StartDate_If_Available() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, false, new DateTime(2019, 10, 1), new DateTime(2019, 1, 1)));

            var results = handler.Execute(query);

            results.Single().StartDate.Should().Be(new DateTime(2019, 1, 1));
        }

        [TestMethod]
        public void GetUsersQueryHandler_Returns_Null_StartDate_If_Not_Available() {
            var handler = new GetUsersQueryHandler(_context);
            var query = new GetUsersQuery();

            _context.Users.Add(GetUser(1, UserStatus.Active, $"test@test.com", null, null, false));

            var results = handler.Execute(query);

            results.Single().StartDate.Should().BeNull();
        }
    }
}