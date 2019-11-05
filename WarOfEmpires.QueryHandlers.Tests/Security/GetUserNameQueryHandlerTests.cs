using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserNameQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void GetUserNameQueryHandler_Gives_DisplayName_If_Available() {
            var handler = new GetUserNameQueryHandler(_context);
            var query = new GetUserNameQuery("test@test.com");
            var user = Substitute.For<User>();

            user.Email.Returns("test@test.com");
            user.DisplayName.Returns("Test name");

            _context.Users.Add(user);

            var result = handler.Execute(query);

            result.Should().Be("Test name");
        }

        [TestMethod]
        public void GetUserNameQueryHandler_Gives_Email_If_DisplayName_Is_Not_Available() {
            var handler = new GetUserNameQueryHandler(_context);
            var query = new GetUserNameQuery("test@test.com");
            var user = Substitute.For<User>();

            user.Email.Returns("test@test.com");
            user.DisplayName.Returns((string)null);

            _context.Users.Add(user);

            var result = handler.Execute(query);

            result.Should().Be("test@test.com");
        }

        [TestMethod]
        public void GetUserNameQueryHandler_Throws_Exception_For_Nonexistent_User() {
            var handler = new GetUserNameQueryHandler(_context);
            var query = new GetUserNameQuery("test@test.com");

            Action queryAction = () => {
                var result = handler.Execute(query);
            };

            queryAction.Should().Throw<InvalidOperationException>();
        }
    }
}