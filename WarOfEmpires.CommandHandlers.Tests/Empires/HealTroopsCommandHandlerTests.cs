using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class HealTroopsCommandHandlerTests {
        [TestMethod]
        public void HealTroopsCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, stamina: 90)
                .WithTroops(TroopType.Archers, 10, 2);

            var handler = new HealTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new HealTroopsCommand("test1@test.com", 5);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().HealTroops(5);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void HealTroopsCommandHandler_Fails_For_Healing_Above_Full() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, stamina: 90)
                .WithTroops(TroopType.Archers, 10, 2);

            var handler = new HealTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new HealTroopsCommand("test1@test.com", 11);

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.StaminaToHeal, "You cannot heal above 100%");
            builder.Player.DidNotReceiveWithAnyArgs().HealTroops(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]

        public void HealTroopsCommandHandler_Fails_When_Not_Enough_Food() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, stamina: 90, canAffordAnything: false)
                .WithTroops(TroopType.Archers, 10, 2);

            var handler = new HealTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new HealTroopsCommand("test1@test.com", 5);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough food to heal these troops");
            builder.Player.DidNotReceiveWithAnyArgs().HealTroops(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
