using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class BuyResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly CaravanRepository _repository;
        private readonly Player _buyer;
        private readonly Player _seller;
        private readonly List<Caravan> _caravans;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public BuyResourcesCommandHandlerTests() {
            _repository = new CaravanRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _buyer = Substitute.For<Player>();
            _buyer.User.Returns(user);
            _buyer.CanAfford(Arg.Any<Resources>()).Returns(true);
            _buyer.Caravans.Returns(new List<Caravan>());

            _seller = Substitute.For<Player>();
            _caravans = new List<Caravan>();
            _seller.Caravans.Returns(_caravans);
            _caravans.Add(CreateCaravan(1, MerchandiseType.Wood, 10000, 5));
            _caravans.Add(CreateCaravan(2, MerchandiseType.Wood, 10000, 4));
            _caravans.Add(CreateCaravan(3, MerchandiseType.Wood, 10000, 5));
            _caravans.Add(CreateCaravan(4, MerchandiseType.Food, 10000, 5));
            _caravans.Add(CreateCaravan(5, MerchandiseType.Stone, 10000, 5));
            _caravans.Add(CreateCaravan(6, MerchandiseType.Ore, 10000, 5));

            _context.Users.Add(user);
            _context.Players.Add(_buyer);
            _context.Players.Add(_seller);
        }

        private Caravan CreateCaravan(int id,  MerchandiseType type, int quantity, int price) {
            var caravan = new Caravan(_seller);
            typeof(Caravan).GetProperty(nameof(Caravan.Id)).SetValue(caravan, id);
            caravan.Merchandise.Add(new Merchandise(type, quantity, price));

            return caravan;
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Succeeds() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "16000", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _caravans[0].Received().Buy(_buyer, MerchandiseType.Wood, 6000);
            _caravans[1].Received().Buy(_buyer, MerchandiseType.Wood, 10000);
            _caravans[2].DidNotReceiveWithAnyArgs().Buy(default, default, default);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Allows_Empty_Values() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Too_Little_Gold() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "16000", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough gold available to buy this many resources");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Food() {
            throw new System.NotImplementedException();

            /*
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("You don't have enough food available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Wood() {
            throw new System.NotImplementedException();

            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "5", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("You don't have enough wood available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Stone() {
            throw new System.NotImplementedException();

            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "5", "5", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("You don't have enough stone available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Ore() {
            throw new System.NotImplementedException();

            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("You don't have enough ore available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Food() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "A", "5", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Food() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "-1", "5", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_FoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "5", "A", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_FoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "5", "-1", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Food_Without_FoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "5", "", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price is required when buying food");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Wood() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "A", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Wood() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "-1", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_WoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "5", "A", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_WoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "5", "-1", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Wood_Without_WoodPrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "5", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price is required when buying food");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Stone() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "A", "5", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Stone() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "-1", "5", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_StonePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "5", "A", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_StonePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "5", "-1", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Stone_Without_StonePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "5", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price is required when buying food");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Ore() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "A", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Ore() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "-1", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_OrePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_OrePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "-1");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Ore_Without_OrePrice() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price is required when buying food");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}