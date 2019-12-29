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
    public sealed class BankCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public BankCommandHandlerTests() {
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

        [TestMethod]
        public void BankCommandHandler_Succeeds() {
            _player.BankTurns.Returns(1);

            var command = new BankCommand("test@test.com");
            var handler = new BankCommandHandler(_repository);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().Bank();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BankCommandHandler_Fails_For_No_BankTurns() {
            _player.BankTurns.Returns(0);

            var command = new BankCommand("test@test.com");
            var handler = new BankCommandHandler(_repository);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have any bank turns available");
            _player.DidNotReceive().Bank();
        }
    }
}