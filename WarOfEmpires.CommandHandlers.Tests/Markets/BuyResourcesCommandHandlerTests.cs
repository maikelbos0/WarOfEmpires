﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
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
            _caravans.Add(CreateCaravan(7, MerchandiseType.Food, 10000, 4));
            _caravans.Add(CreateCaravan(8, MerchandiseType.Stone, 10000, 4));
            _caravans.Add(CreateCaravan(9, MerchandiseType.Ore, 10000, 4));

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

            foreach (var caravan in _caravans.Where(c => c != _caravans[0] && c != _caravans[1])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[0].Received().Buy(_buyer, MerchandiseType.Wood, 6000);
            _caravans[1].Received().Buy(_buyer, MerchandiseType.Wood, 16000);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Allows_Empty_Values() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }
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
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "10001", "", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Warnings.Should().HaveCount(1);
            result.Warnings[0].Should().Be("There was not enough food available at that price; all available wood has been purchased");

            foreach (var caravan in _caravans.Where(c => c != _caravans[6])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[6].Received().Buy(_buyer, MerchandiseType.Food, 10001);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Wood() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "10001", "4", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Warnings.Should().HaveCount(1);
            result.Warnings[0].Should().Be("There was not enough wood available at that price; all available wood has been purchased");

            foreach (var caravan in _caravans.Where(c => c != _caravans[1])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[1].Received().Buy(_buyer, MerchandiseType.Wood, 10001);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Stone() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "10001", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Warnings.Should().HaveCount(1);
            result.Warnings[0].Should().Be("There was not enough stone available at that price; all available wood has been purchased");

            foreach (var caravan in _caravans.Where(c => c != _caravans[7])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[7].Received().Buy(_buyer, MerchandiseType.Stone, 10001);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Ore() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", "", "", "", "", "", "", "10001", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Warnings.Should().HaveCount(1);
            result.Warnings[0].Should().Be("There was not enough ore available at that price; all available wood has been purchased");

            foreach (var caravan in _caravans.Where(c => c != _caravans[8])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[8].Received().Buy(_buyer, MerchandiseType.Ore, 10001);
            _context.CallsToSaveChanges.Should().Be(1);
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