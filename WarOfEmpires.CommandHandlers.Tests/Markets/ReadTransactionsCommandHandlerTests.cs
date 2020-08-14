using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class ReadTransactionsCommandHandlerTests {
        [TestMethod]
        public void ReadTransactionsCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new ReadTransactionsCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadTransactionsCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().HasNewMarketSales = false;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}