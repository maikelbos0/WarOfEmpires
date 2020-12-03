using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class SellResourcesCommandHandlerTests {
        [TestMethod]
        public void SellResourcesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Market, 3)
                .WithWorkers(WorkerType.Merchants, 10);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", 15000, 10),
                new MerchandiseInfo("Wood", 16000, 9),
                new MerchandiseInfo("Stone", 17000, 8),
                new MerchandiseInfo("Ore", 18000, 7)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().SellResources(Arg.Is<IEnumerable<MerchandiseTotals>>(m => m.Count() == 4));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", null, null),
                new MerchandiseInfo("Wood", null, null),
                new MerchandiseInfo("Stone", null, null),
                new MerchandiseInfo("Ore", null, null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().SellResources(default);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Test", 1, 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().SellResources(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Few_Available_Merchants() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Market, 3)
                .WithWorkers(WorkerType.Merchants, 10)
                .WithCaravan(1)
                .WithCaravan(2);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", 50000, 10),
                new MerchandiseInfo("Wood", 50000, 9),
                new MerchandiseInfo("Stone", 50000, 8),
                new MerchandiseInfo("Ore", 51000, 7)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough merchants available to send this many to the market");
            builder.Player.DidNotReceiveWithAnyArgs().SellResources(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Quantity() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", 5, 5)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Quantity", "You don't have enough food available to sell that much");
            builder.Player.DidNotReceiveWithAnyArgs().SellResources(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Food_Without_FoodPrice() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SellResourcesCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new SellResourcesCommand("test1@test.com", new List<MerchandiseInfo>() {
                new MerchandiseInfo("Food", 5, null)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Merchandise[0].Price", "Food price is required when selling food");
            builder.Player.DidNotReceiveWithAnyArgs().SellResources(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}