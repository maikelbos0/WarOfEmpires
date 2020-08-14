using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class BuyResourcesCommandHandlerTests {
        [TestMethod]
        public void BuyResourcesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var buyer)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var partialSaleCaravan, new Merchandise(MerchandiseType.Wood, 10000, 5))
                .WithCaravan(2, out var fullSaleCaravan, new Merchandise(MerchandiseType.Wood, 10000, 4))
                .WithCaravan(3, new Merchandise(MerchandiseType.Wood, 10000, 5))
                .WithCaravan(4, new Merchandise(MerchandiseType.Food, 10000, 5))
                .WithCaravan(5, new Merchandise(MerchandiseType.Food, 10000, 4));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            foreach (var caravan in builder.Player.Caravans.Where(c => c != partialSaleCaravan && c != fullSaleCaravan)) {
                caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            }

            fullSaleCaravan.Received().Buy(buyer, MerchandiseType.Wood, 16000);
            partialSaleCaravan.Received().Buy(buyer, MerchandiseType.Wood, 6000);
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Removes_Empty_Caravans() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var partialSaleCaravan, new Merchandise(MerchandiseType.Wood, 10000, 5))
                .WithCaravan(2, out var fullSaleCaravan, new Merchandise(MerchandiseType.Wood, 10000, 4));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "5")
            });

            handler.Execute(command);

            builder.Player.Caravans.Should().NotContain(fullSaleCaravan);
            builder.Player.Caravans.Should().Contain(partialSaleCaravan);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Does_Not_Buy_From_Self() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("seller@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "1", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Should().HaveWarning("There was not enough wood available at that price; all available wood has been purchased");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Too_Little_Gold() {
            var builder = new FakeBuilder()
                .WithPlayer(1, canAffordAnything: false)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "6000", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough gold available to buy this many resources");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Gives_Warning_For_Too_Low_Available_Quantity() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var buyer)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "10001", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            result.Should().HaveWarning("There was not enough wood available at that price; all available wood has been purchased");
            caravan.Received().Buy(buyer, MerchandiseType.Wood, 10001);
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Quantity() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", "A", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Quantity() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "-1", "5")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "Invalid number");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Alphanumeric_Price() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "10000", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Negative_Price() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Invalid number");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Quantity_Without_Price() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildPlayer(2, email: "seller@test.com")
                .WithCaravan(1, out var caravan, new Merchandise(MerchandiseType.Wood, 10000, 5));

            var handler = new BuyResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new BuyResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Wood", "16000", "")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Wood price is required when buying wood");
            caravan.DidNotReceiveWithAnyArgs().Buy(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}