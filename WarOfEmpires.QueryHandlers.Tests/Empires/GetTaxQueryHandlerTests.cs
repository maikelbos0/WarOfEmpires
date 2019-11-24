using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetTaxQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetTaxQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Tax.Returns(250);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetTaxQueryHandler_Returns_Correct_Tax() {
            var command = new GetTaxQuery("test@test.com");
            var handler = new GetTaxQueryHandler(_context);

            var result = handler.Execute(command);

            result.Tax.Should().Be("250");
        }
    }
}