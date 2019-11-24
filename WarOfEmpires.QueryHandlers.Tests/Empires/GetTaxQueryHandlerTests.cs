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
            player.Tax.Returns(50);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetTaxQueryHandler_Returns_Correct_Tax() {
            var command = new GetTaxQuery("test@test.com");
            var handler = new GetTaxQueryHandler(_context);

            var result = handler.Execute(command);

            result.Tax.Should().Be("50");
        }

        [TestMethod]
        public void GetTaxQueryHandler_Returns_Correct_Resources_Per_Turn() {
            var command = new GetTaxQuery("test@test.com");
            var handler = new GetTaxQueryHandler(_context);

            var result = handler.Execute(command);

            result.BaseGoldPerTurn.Should().Be(500);
            result.BaseFoodPerTurn.Should().Be(20);
            result.BaseWoodPerTurn.Should().Be(20);
            result.BaseStonePerTurn.Should().Be(20);
            result.BaseOrePerTurn.Should().Be(20);
            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentWoodPerWorkerPerTurn.Should().Be(10);
            result.CurrentFoodPerWorkerPerTurn.Should().Be(10);
            result.CurrentStonePerWorkerPerTurn.Should().Be(10);
            result.CurrentOrePerWorkerPerTurn.Should().Be(10);
        }
    }
}