using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class ReadTransactionsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public ReadTransactionsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _player = Substitute.For<Player>();
            _player.User.Returns(user);

            _context.Users.Add(user);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void ReadTransactionsCommandHandler_Succeeds() {
            var handler = new ReadTransactionsCommandHandler(_repository);
            var command = new ReadTransactionsCommand("test@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            _player.Received().HasNewMarketSales = false;
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}