using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class ReadTransactionsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Transaction _transaction;

        public ReadTransactionsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _transaction = Substitute.For<Transaction>();

            var player = Substitute.For<Player>();
            player.SellTransactions.Returns(new List<Transaction>() { _transaction });
            player.User.Returns(user);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void ReadMessageCommandHandler_Succeeds() {
            var handler = new ReadTransactionsCommandHandler(_repository);
            var command = new ReadTransactionsCommand("test@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _transaction.Received().IsRead = true;
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}