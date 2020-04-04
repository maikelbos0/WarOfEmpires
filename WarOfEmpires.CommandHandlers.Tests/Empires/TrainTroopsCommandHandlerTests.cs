using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using DomainPlayers = WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;
using System;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class TrainTroopsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly DomainPlayers.Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public TrainTroopsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<DomainPlayers.Player>();
            player.GetAvailableBarracksCapacity().Returns(20);
            player.User.Returns(user);
            player.Peasants.Returns(30);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Succeeds() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "5", "3"),
                new TroopInfo("Cavalry", "4", "2"),
                new TroopInfo("Footmen", "4", "1")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainTroops(TroopType.Archers, 5, 3);
            _player.Received().TrainTroops(TroopType.Cavalry, 4, 2);
            _player.Received().TrainTroops(TroopType.Footmen, 4, 1);
            _context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Allows_Empty_Troops() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "", ""),
                new TroopInfo("Cavalry", "", ""),
                new TroopInfo("Footmen", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>() {
                new TroopInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Barracks_Room() {
            _player.Peasants.Returns(30);

            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "8", "2"),
                new TroopInfo("Cavalry", "8", "2"),
                new TroopInfo("Footmen", "8", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough barracks available to train that many troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Soldiers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "A", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Mercenaries() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "2", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Soldiers() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "-1", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Mercenaries() {
            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "2", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_High_TroopCounts() {
            _player.GetAvailableBarracksCapacity().Returns(40);

            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "11", "0"),
                new TroopInfo("Cavalry", "11", "0"),
                new TroopInfo("Footmen", "11", "0")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have that many peasants available to train");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Resources() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new TrainTroopsCommandHandler(_repository);
            var command = new TrainTroopsCommand("test@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "1", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources to train these troops");
            _player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}