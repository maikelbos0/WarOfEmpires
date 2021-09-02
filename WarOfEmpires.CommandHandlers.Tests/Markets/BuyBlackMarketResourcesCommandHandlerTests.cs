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

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class BuyBlackMarketResourcesCommandHandlerTests {
        [TestMethod]
        public void BuyBlackMarketResourcesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new BuyBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuyBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Food", 2000),
                new BlackMarketMerchandiseInfo("Wood", 16000)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            builder.Player.Received().BuyResourcesFromBlackMarket(new BlackMarketMerchandiseTotals(MerchandiseType.Food, 2000));
            builder.Player.Received().BuyResourcesFromBlackMarket(new BlackMarketMerchandiseTotals(MerchandiseType.Wood, 16000));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuyBlackMarketResourcesCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new BuyBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuyBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Wood", null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().BuyResourcesFromBlackMarket(default);
        }

        [TestMethod]
        public void BuyBlackMarketResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new BuyBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuyBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Test", 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().BuyResourcesFromBlackMarket(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuyBlackMarketResourcesCommandHandler_Fails_For_Too_Little_Gold() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false);

            var handler = new BuyBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuyBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Wood", 6000)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough gold available to buy this many resources");
            builder.Player.DidNotReceiveWithAnyArgs().BuyResourcesFromBlackMarket(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
