using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class BuyResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _buyer;
        private readonly Player _seller;
        private readonly List<Caravan> _caravans;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public BuyResourcesCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var buyerUser = Substitute.For<User>();
            buyerUser.Email.Returns("test@test.com");
            buyerUser.Status.Returns(UserStatus.Active);

            _buyer = Substitute.For<Player>();
            _buyer.User.Returns(buyerUser);
            _buyer.CanAfford(Arg.Any<Resources>()).Returns(true);
            _buyer.Caravans.Returns(new List<Caravan>());

            var sellerUser = Substitute.For<User>();
            sellerUser.Email.Returns("seller@test.com");
            sellerUser.Status.Returns(UserStatus.Active);

            _seller = Substitute.For<Player>();
            _caravans = new List<Caravan>();
            _caravans.Add(CreateCaravan(1, MerchandiseType.Wood, 10000, 5));
            _caravans.Add(CreateCaravan(2, MerchandiseType.Wood, 10000, 4));
            _caravans.Add(CreateCaravan(3, MerchandiseType.Wood, 10000, 5));
            _caravans.Add(CreateCaravan(4, MerchandiseType.Food, 10000, 5));
            _caravans.Add(CreateCaravan(5, MerchandiseType.Stone, 10000, 5));
            _caravans.Add(CreateCaravan(6, MerchandiseType.Ore, 10000, 5));
            _caravans.Add(CreateCaravan(7, MerchandiseType.Food, 10000, 4));
            _caravans.Add(CreateCaravan(8, MerchandiseType.Stone, 10000, 4));
            _caravans.Add(CreateCaravan(9, MerchandiseType.Ore, 10000, 4));
            _seller.Caravans.Returns(new List<Caravan>(_caravans));
            _seller.User.Returns(sellerUser);
            _seller.CanAfford(Arg.Any<Resources>()).Returns(true);

            _context.Users.Add(buyerUser);
            _context.Players.Add(_buyer);
            _context.Players.Add(_seller);
        }

        private Caravan CreateCaravan(int id, MerchandiseType type, int quantity, int price) {
            var caravan = Substitute.For<Caravan>();

            caravan.Id.Returns(id);
            caravan.Player.Returns(_seller);
            caravan.Merchandise.Returns(new List<Merchandise>() { new Merchandise(type, quantity, price) });
            caravan.Buy(Arg.Any<Player>(), type, Arg.Any<int>()).Returns(c => caravan.Merchandise.Single().Buy(_seller, c.ArgAt<Player>(0), c.ArgAt<int>(2)));

            return caravan;
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Succeeds() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            foreach (var caravan in _caravans.Where(c => c != _caravans[0] && c != _caravans[1])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[0].Received().Buy(_buyer, MerchandiseType.Wood, 6000);
            _caravans[1].Received().Buy(_buyer, MerchandiseType.Wood, 16000);
            _context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Removes_Empty_Caravans() {
            var previousCaravanCount = _context.Players.Sum(c => c.Caravans.Count());
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "5")
            });

            var result = handler.Execute(command);

            _context.Players.Sum(c => c.Caravans.Count()).Should().Be(previousCaravanCount - 1);
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Does_Not_Buy_From_Self() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("seller@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "1", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Should().HaveWarning("There was not enough wood available at that price; all available wood has been purchased");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Allows_Empty_Values() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "", ""),
                new MerchandiseInfo("Wood", "", ""),
                new MerchandiseInfo("Stone", "", ""),
                new MerchandiseInfo("Ore", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Too_Little_Gold() {
            _buyer.CanAfford(Arg.Any<Resources>()).Returns(c => c.ArgAt<Resources>(0).Gold == 0);

            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "5")
            });

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
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Low_Available_Quantity() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "10001", "4")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Should().HaveWarning("There was not enough food available at that price; all available food has been purchased");

            foreach (var caravan in _caravans.Where(c => c != _caravans[6])) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _caravans[6].Received().Buy(_buyer, MerchandiseType.Food, 10001);
            _context.CallsToSaveChanges.Should().Be(2);
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Quantity() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "A", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Quantity() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "-1", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Price() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Price() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Quantity_Without_Price() {
            var handler = new BuyResourcesCommandHandler(_repository, _formatter);
            var command = new BuyResourcesCommand("test@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Wood price is required when buying wood");

            foreach (var caravan in _caravans) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}