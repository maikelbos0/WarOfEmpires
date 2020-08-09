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
        [DataRow("0", DisplayName = "Minimum")]
        [DataRow("50", DisplayName = "Normal")]
        [DataRow("100", DisplayName = "Maximum")]
        public void SetTaxCommandHandler_Succeeds(string tax) {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SetTaxCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetTaxCommand("test1@test.com", tax);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().Tax = int.Parse(tax);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [DataTestMethod]
        [DataRow("A", DisplayName = "Alphanumeric")]
        [DataRow("-1", DisplayName = "Negative")]
        [DataRow("101", DisplayName = "Too High")]
        public void SetTaxCommandHandler_Fails_For_Invalid_Tax(string tax) {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new SetTaxCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetTaxCommand("test1@test.com", tax);

            var result = handler.Execute(command);
            
            result.Should().HaveError("Tax", "Tax must be a valid number");
            builder.Player.DidNotReceive().Tax = Arg.Any<int>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}