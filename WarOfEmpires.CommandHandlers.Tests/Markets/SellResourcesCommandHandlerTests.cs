using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
            var command = new SellResourcesCommand("test@test.com", "15000", "10", "16000", "9", "17000", "8", "18000", "7");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().SellResources(Arg.Is<IEnumerable<MerchandiseTotals>>(m => m.Count() == 4));
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Allows_Empty_Values() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Few_Available_Merchants() {
            _player.Caravans.Returns(new List<Caravan>() {
                new Caravan(_player),
                new Caravan(_player)
            });

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "50000", "10", "50000", "9", "50000", "8", "51000", "7");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough merchants available to send this many to the market");
            _player.DidNotReceiveWithAnyArgs().SellResources(default);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Food() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "A", "5", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_Food() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "0", "5", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Food() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("You don't have enough food available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_FoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "A", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_FoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "0", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Food_Without_FoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price is required when selling food");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Wood() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "A", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_Wood() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "0", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Wood() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "5", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("You don't have enough wood available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_WoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "A", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_WoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "0", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Wood_Without_WoodPrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price is required when selling wood");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Stone() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "A", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_Stone() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "0", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Stone() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "5", "5", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("You don't have enough stone available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_StonePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "A", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_StonePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "0", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Stone_Without_StonePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price is required when selling stone");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Ore() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "A", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_Ore() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "0", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Ore() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("You don't have enough ore available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_OrePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Zero_OrePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "0");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Ore_Without_OrePrice() {
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price is required when selling ore");
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}