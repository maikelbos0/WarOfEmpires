using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using DomainPlayers = WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;
using System;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainTroopsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly DomainPlayers.Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public UntrainTroopsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<DomainPlayers.Player>();
            player.User.Returns(user);
            player.GetTroops(TroopType.Archers).Returns(new Troops(TroopType.Archers, 10, 10));
            player.GetTroops(TroopType.Cavalry).Returns(new Troops(TroopType.Cavalry, 10, 10));
            player.GetTroops(TroopType.Footmen).Returns(new Troops(TroopType.Footmen, 10, 10));

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }
        
        [TestMethod]
        public void UntrainTroopsCommandHandler_Succeeds() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "0", "1"),
                new TroopInfo("Cavalry", "2", "3"),
                new TroopInfo("Footmen", "4", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UntrainTroops(TroopType.Archers, 0, 1);
            _player.Received().UntrainTroops(TroopType.Cavalry, 2, 3);
            _player.Received().UntrainTroops(TroopType.Footmen, 4, 5);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Allows_Empty_Troops() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "", ""),
                new TroopInfo("Cavalry", "", ""),
                new TroopInfo("Footmen", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Soldiers() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "A", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Mercenaries() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "1", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Soldiers() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "-1", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Mercenaries() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "1", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Soldiers() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "12", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "You don't have that many archers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_MercenaryArchers() {
            var handler = new UntrainTroopsCommandHandler(_repository, _formatter);
            var command = new UntrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "2", "12")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "You don't have that many mercenary archers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}