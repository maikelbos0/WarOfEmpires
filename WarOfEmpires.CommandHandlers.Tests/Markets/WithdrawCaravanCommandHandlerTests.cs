using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
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
    public sealed class WithdrawCaravanCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Caravan _caravan;

        public WithdrawCaravanCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _player = Substitute.For<Player>();
            _player.User.Returns(user);

            _caravan = Substitute.For<Caravan>();
            _caravan.Id.Returns(1);
            _caravan.Player.Returns(_player);
            _player.Caravans.Returns(new List<Caravan>() { _caravan });

            var user2 = Substitute.For<User>();
            user2.Email.Returns("not@test.com");
            user2.Status.Returns(UserStatus.Active);

            var player2 = Substitute.For<Player>();
            player2.User.Returns(user2);

            _context.Players.Add(_player);
            _context.Players.Add(player2);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Succeeds() {
            var handler = new WithdrawCaravanCommandHandler(_repository);
            var command = new WithdrawCaravanCommand("test@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            _caravan.Received().Withdraw();
            _player.Caravans.Should().NotContain(_caravan);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Alphanumeric_CaravanId() {
            var handler = new WithdrawCaravanCommandHandler(_repository);
            var command = new WithdrawCaravanCommand("test@test.com", "A");

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<FormatException>();
            _caravan.DidNotReceiveWithAnyArgs().Withdraw();
            _player.Caravans.Should().Contain(_caravan);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Invalid_CaravanId() {
            var handler = new WithdrawCaravanCommandHandler(_repository);
            var command = new WithdrawCaravanCommand("test@test.com", "51");

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            _caravan.DidNotReceiveWithAnyArgs().Withdraw();
            _player.Caravans.Should().Contain(_caravan);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Caravan_Of_Different_Player() {
            var handler = new WithdrawCaravanCommandHandler(_repository);
            var command = new WithdrawCaravanCommand("not@test.com", "1");

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            _caravan.DidNotReceiveWithAnyArgs().Withdraw();
            _player.Caravans.Should().Contain(_caravan);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}