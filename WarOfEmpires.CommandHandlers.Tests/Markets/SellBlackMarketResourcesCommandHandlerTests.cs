using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class SellBlackMarketResourcesCommandHandlerTests {
        [TestMethod]
        public void SellBlackMarketResourcesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Food", 2000),
                new BlackMarketMerchandiseInfo("Wood", 16000)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            builder.Player.Received().SellResourcesToBlackMarket(new BlackMarketMerchandiseTotals(MerchandiseType.Food, 2000));
            builder.Player.Received().SellResourcesToBlackMarket(new BlackMarketMerchandiseTotals(MerchandiseType.Wood, 16000));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SellBlackMarketResourcesCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Wood", null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().SellResourcesToBlackMarket(default);
        }

        [TestMethod]
        public void SellBlackMarketResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Test", 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().SellResourcesToBlackMarket(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellBlackMarketResourcesCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false);

            var handler = new SellBlackMarketResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellBlackMarketResourcesCommand("test1@test.com", new List<BlackMarketMerchandiseInfo>() {
                new BlackMarketMerchandiseInfo("Wood", 6000)
            });

            var result = handler.Execute(command);

            result.AddError(c => c.Merchandise[0].Quantity, $"You don't have enough wood available to sell that much");
            builder.Player.DidNotReceiveWithAnyArgs().SellResourcesToBlackMarket(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
