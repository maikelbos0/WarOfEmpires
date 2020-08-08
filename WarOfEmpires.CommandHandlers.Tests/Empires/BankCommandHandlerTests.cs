using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class BankCommandHandlerTests {
        [TestMethod]
        public void BankCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            player.Received().Bank();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BankCommandHandler_Fails_For_No_BankTurns() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player, bankTurns: 0);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have any bank turns available");
            player.DidNotReceiveWithAnyArgs().Bank();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}