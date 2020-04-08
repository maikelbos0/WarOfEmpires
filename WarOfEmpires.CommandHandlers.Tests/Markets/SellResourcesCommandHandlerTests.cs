using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class SellResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public SellResourcesCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.Merchants, 10) });
            player.GetBuildingBonus(BuildingType.Market).Returns(25000);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Succeeds() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "15000", "10"),
                new MerchandiseInfo("Wood", "16000", "9"),
                new MerchandiseInfo("Stone", "17000", "8"),
                new MerchandiseInfo("Ore", "18000", "7")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().SellResources(Arg.Is<IEnumerable<MerchandiseTotals>>(m => m.Count() == 4));
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Allows_Empty_Values() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "", ""),
                new MerchandiseInfo("Wood", "", ""),
                new MerchandiseInfo("Stone", "", ""),
                new MerchandiseInfo("Ore", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Few_Available_Merchants() {
            _player.Caravans.Returns(new List<Caravan>() {
                new Caravan(_player),
                new Caravan(_player)
            });

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "50000", "10"),
                new MerchandiseInfo("Wood", "50000", "9"),
                new MerchandiseInfo("Stone", "50000", "8"),
                new MerchandiseInfo("Ore", "51000", "7")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough merchants available to send this many to the market");
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Quantity() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "A", "10")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Quantity() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "-1", "10")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Quantity() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "5", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "You don't have enough food available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Price() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "5", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Price() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "5", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Food_Without_FoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "5", "")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Food price is required when selling food");
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}