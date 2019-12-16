using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Attacks;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Attacks {
    [TestClass]
    public sealed class ReadAttackCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Attack _Attack;

        public ReadAttackCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
            
            var attackerUser = Substitute.For<User>();
            attackerUser.Email.Returns("attacker@test.com");
            attackerUser.Status.Returns(UserStatus.Active);

            _Attack = Substitute.For<Attack>();

            var attacker = Substitute.For<Player>();
            attacker.ReceivedAttacks.Returns(new List<Attack>());
            attacker.ExecutedAttacks.Returns(new List<Attack>() { _Attack });
            attacker.User.Returns(attackerUser);

            _context.Users.Add(attackerUser);
            _context.Players.Add(attacker);

            var defenderUser = Substitute.For<User>();
            defenderUser.Email.Returns("defender@test.com");
            defenderUser.Status.Returns(UserStatus.Active);

            var defender = Substitute.For<Player>();
            defender.ReceivedAttacks.Returns(new List<Attack>() { _Attack });
            defender.ExecutedAttacks.Returns(new List<Attack>());
            defender.User.Returns(defenderUser);

            _Attack.Id.Returns(1);
            _Attack.Attacker.Returns(attacker);
            _Attack.Defender.Returns(defender);

            _context.Users.Add(defenderUser);
            _context.Players.Add(defender);
        }

        [TestMethod]
        public void ReadAttackCommandHandler_Succeeds() {
            var handler = new ReadAttackCommandHandler(_repository);
            var command = new ReadAttackCommand("defender@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _Attack.Received().IsRead = true;
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ReadAttackCommandHandler_Throws_Exception_For_Attack_Received_By_Different_Player() {
            var handler = new ReadAttackCommandHandler(_repository);
            var command = new ReadAttackCommand("attacker@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _Attack.DidNotReceive().IsRead = true;
        }

        [TestMethod]
        public void ReadAttackCommandHandler_Throws_Exception_For_Alphanumeric_AttackId() {
            var handler = new ReadAttackCommandHandler(_repository);
            var command = new ReadAttackCommand("defender@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
        }
    }
}