using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class SetTaxCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public SetTaxCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [DataTestMethod]
        [DataRow("0", DisplayName = "Minimum")]
        [DataRow("200", DisplayName = "Normal")]
        [DataRow("500", DisplayName = "Maximum")]
        public void SetTaxCommandHandler_Succeeds(string tax) {
            var command = new SetTaxCommand("test@test.com", tax);
            var handler = new SetTaxCommandHandler(_repository);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().Tax = int.Parse(tax);
        }

        [DataTestMethod]
        [DataRow("A", DisplayName = "Alphanumeric")]
        [DataRow("-1", DisplayName = "Negative")]
        [DataRow("501", DisplayName = "Too High")]
        public void SetTaxCommandHandler_Fails_For_Invalid_Tax(string tax) {
            var command = new SetTaxCommand("test@test.com", tax);
            var handler = new SetTaxCommandHandler(_repository);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Tax");
            result.Errors[0].Message.Should().Be("Tax must be a valid number");
            _player.DidNotReceive().Tax = Arg.Any<int>();
        }
    }
}