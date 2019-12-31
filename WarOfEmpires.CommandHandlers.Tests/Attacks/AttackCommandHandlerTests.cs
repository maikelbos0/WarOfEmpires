using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Attacks;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Attacks {
    [TestClass]
    public sealed class AttackCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _attacker;
        private readonly Player _defender;

        public AttackCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var attackerUser = Substitute.For<User>();
            attackerUser.Email.Returns("test@test.com");
            attackerUser.Status.Returns(UserStatus.Active);

            _attacker = Substitute.For<Player>();
            _attacker.Id.Returns(1);
            _attacker.User.Returns(attackerUser);
            _attacker.ExecutedAttacks.Returns(new List<Attack>());
            _attacker.AttackTurns.Returns(20);

            _context.Users.Add(attackerUser);
            _context.Players.Add(_attacker);

            var defenderUser = Substitute.For<User>();
            defenderUser.Email.Returns("defender@test.com");
            defenderUser.Status.Returns(UserStatus.Active);

            _defender = Substitute.For<Player>();
            _defender.Id.Returns(2);
            _defender.User.Returns(defenderUser);
            _defender.ReceivedAttacks.Returns(new List<Attack>());

            _context.Users.Add(defenderUser);
            _context.Players.Add(_defender);
        }

        [TestMethod]
        public void AttackCommandHandler_Succeeds() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "2", "10");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.ResultId.Should().Be(0);
            _attacker.ExecutedAttacks.Should().HaveCount(1);
            _defender.ReceivedAttacks.Should().HaveCount(1);
            _attacker.ExecutedAttacks.Single().Result.Should().NotBe(AttackResult.Undefined);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Alphanumeric_Defender() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "A", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Defender() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "5", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Type() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("wrong", "test@test.com", "5", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("Raid", typeof(Raid), DisplayName = "Raid")]
        [DataRow("Assault", typeof(Assault), DisplayName = "Assault")]
        public void AttackCommandHandler_Resolves_Type_Parameter_To_Correct_Type(string typeParameter, Type attackType) {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand(typeParameter, "test@test.com", "2", "10");

            handler.Execute(command);

            _attacker.ExecutedAttacks.Single().Should().BeOfType(attackType);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_AlphaNumeric_Turns() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Turns");
            result.Errors[0].Message.Should().Be("Turns must be a valid number");
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_Too_Few_Turns_Available() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "2", "10");

            _attacker.AttackTurns.Returns(9);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Turns");
            result.Errors[0].Message.Should().Be("You don't have enough attack turns");
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_Less_Than_One_Turn() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "2", "0");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Turns");
            result.Errors[0].Message.Should().Be("Turns must be a valid number");
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_More_Than_Ten_Turns() {
            var handler = new AttackCommandHandler(_repository);
            var command = new AttackCommand("Raid", "test@test.com", "2", "11");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Turns");
            result.Errors[0].Message.Should().Be("Turns must be a valid number");
            _attacker.ExecutedAttacks.Should().BeEmpty();
            _defender.ReceivedAttacks.Should().BeEmpty();
        }
    }
}