using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserNameQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void GetUserNameQueryHandler_Gives_DisplayName() {
            var handler = new GetUserNameQueryHandler(_context);
            var query = new GetUserNameQuery("test@test.com");
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Email.Returns("test@test.com");
            player.User.Returns(user);
            player.DisplayName.Returns("Test name");

            _context.Players.Add(player);
            _context.Users.Add(user);

            var result = handler.Execute(query);

            result.Should().Be("Test name");
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