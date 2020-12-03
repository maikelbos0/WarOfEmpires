using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class SetTaxCommandHandlerTests {
        [DataTestMethod]
        [DataRow(0, DisplayName = "Minimum")]
        [DataRow(50, DisplayName = "Normal")]
        [DataRow(100, DisplayName = "Maximum")]
        public void SetTaxCommandHandler_Succeeds(int tax) {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SetTaxCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetTaxCommand("test1@test.com", tax);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().Tax = tax;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}