﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainWorkersCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public UntrainWorkersCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.Farmers, 10),
                new Workers(WorkerType.WoodWorkers, 10),
                new Workers(WorkerType.StoneMasons, 10),
                new Workers(WorkerType.OreMiners, 10),
                new Workers(WorkerType.SiegeEngineers, 10),
                new Workers(WorkerType.Merchants, 10),
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Succeeds() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "5", "4", "3", "2", "1", "6");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UntrainWorkers(WorkerType.Farmers, 5);
            _player.Received().UntrainWorkers(WorkerType.WoodWorkers, 4);
            _player.Received().UntrainWorkers(WorkerType.StoneMasons, 3);
            _player.Received().UntrainWorkers(WorkerType.OreMiners, 2);
            _player.Received().UntrainWorkers(WorkerType.SiegeEngineers, 1);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Allows_Empty_Workers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_Farmers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "A", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Farmers");
            result.Errors[0].Message.Should().Be("Farmers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_WoodWorkers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "A", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodWorkers");
            result.Errors[0].Message.Should().Be("Wood workers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_StoneMasons() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "A", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StoneMasons");
            result.Errors[0].Message.Should().Be("Stone masons must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_OreMiners() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OreMiners");
            result.Errors[0].Message.Should().Be("Ore miners must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_SiegeEngineers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.SiegeEngineers");
            result.Errors[0].Message.Should().Be("Siege engineers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_Merchants() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Merchants");
            result.Errors[0].Message.Should().Be("Merchants must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_Farmers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "12", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Farmers");
            result.Errors[0].Message.Should().Be("You don't have that many farmers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_WoodWorkers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "12", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodWorkers");
            result.Errors[0].Message.Should().Be("You don't have that many wood workers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_StoneMasons() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "12", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StoneMasons");
            result.Errors[0].Message.Should().Be("You don't have that many stone masons to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_OreMiners() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "12", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OreMiners");
            result.Errors[0].Message.Should().Be("You don't have that many ore miners to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_SiegeEngineers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "12", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.SiegeEngineers");
            result.Errors[0].Message.Should().Be("You don't have that many siege engineers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_Merchants() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "2", "12");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Merchants");
            result.Errors[0].Message.Should().Be("You don't have that many merchants to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_Farmers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "-2", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Farmers");
            result.Errors[0].Message.Should().Be("Farmers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_WoodWorkers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "-2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodWorkers");
            result.Errors[0].Message.Should().Be("Wood workers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_StoneMasons() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "-2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StoneMasons");
            result.Errors[0].Message.Should().Be("Stone masons must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_OreMiners() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "-2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OreMiners");
            result.Errors[0].Message.Should().Be("Ore miners must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_SiegeEngineers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "-2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.SiegeEngineers");
            result.Errors[0].Message.Should().Be("Siege engineers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_Merchants() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "2", "2", "2", "2", "2", "-2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Merchants");
            result.Errors[0].Message.Should().Be("Merchants must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Siege_Maintenance_In_Use() {
            _player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(6);
            _player.SiegeWeapons.Returns(new List<SiegeWeapon>() {
                new SiegeWeapon(SiegeWeaponType.FireArrows, 2),
                new SiegeWeapon(SiegeWeaponType.BatteringRams, 2),
                new SiegeWeapon(SiegeWeaponType.ScalingLadders, 2)
            });

            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "", "", "", "", "1", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.SiegeEngineers");
            result.Errors[0].Message.Should().Be("Your siege engineers are maintaining too many siege weapons for that many to be untrained");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Merchants_In_Use() {
            _player.Caravans.Returns(new List<Caravan>() {
                new Caravan(_player),
                new Caravan(_player)
            });

            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", "", "", "", "", "", "9");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Merchants");
            result.Errors[0].Message.Should().Be("You can not untrain merchants that have a caravan on the market");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }
    }
}