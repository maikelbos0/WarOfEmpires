﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {

    [TestClass]
    public sealed class HealTroopsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public HealTroopsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.Stamina.Returns(90);
            player.User.Returns(user);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);
            player.Archers.Returns(new Troops(10,2));
            player.Cavalry.Returns(new Troops(0,0));
            player.Footmen.Returns(new Troops(0,0));

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void HealTroopsCommandHandler_Succeeds() {
            var handler = new HealTroopsCommandHandler(_repository);
            var command = new HealTroopsCommand("test@test.com", "5");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().HealTroops(5);
            _context.CallsToSaveChanges.Should().Be(1);
        }


        [TestMethod]
        public void HealTroopsCommandHandler_Fails_For_Alphanumeric_Value() {
            var handler = new HealTroopsCommandHandler(_repository);
            var command = new HealTroopsCommand("test@test.com", "K");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StaminaToHeal");
            result.Errors[0].Message.Should().Be("Stamina to heal must be a valid number");
            _player.DidNotReceiveWithAnyArgs().HealTroops(default);
        }

        [TestMethod]
        public void HealTroopsCommandHandler_Fails_For_Negative_Value() {
            var handler = new HealTroopsCommandHandler(_repository);
            var command = new HealTroopsCommand("test@test.com", "-5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StaminaToHeal");
            result.Errors[0].Message.Should().Be("Stamina to heal must be a valid number");
            _player.DidNotReceiveWithAnyArgs().HealTroops(default);
        }

        [TestMethod]
        public void HealTroopsCommandHandler_Fails_For_Healing_Above_Full() {
            var handler = new HealTroopsCommandHandler(_repository);
            var command = new HealTroopsCommand("test@test.com", "11");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StaminaToHeal");
            result.Errors[0].Message.Should().Be("You cannot heal above 100%");
            _player.DidNotReceiveWithAnyArgs().HealTroops(default);
        }

        [TestMethod]

        public void HealTroopsCommandHandler_Fails_When_Not_Enough_Food() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);
            var handler = new HealTroopsCommandHandler(_repository);
            var command = new HealTroopsCommand("test@test.com", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().Be(null);
            result.Errors[0].Message.Should().Be("You don't have enough food to heal these troops");
            _player.DidNotReceiveWithAnyArgs().HealTroops(default);
        }
    }
}
