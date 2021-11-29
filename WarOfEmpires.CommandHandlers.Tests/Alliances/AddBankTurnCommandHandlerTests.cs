using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class AddBankTurnCommandHandlerTests {
        [TestMethod]
        public void AddBankTurnCommandHandler_Calls_AddBankTurn_For_All_Alliances() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .WithAlliance(2, out var anotherAlliance);

            var handler = new AddBankTurnCommandHandler(new AllianceRepository(builder.Context));
            var command = new AddBankTurnCommand();

            handler.Execute(command);

            alliance.Received().AddBankTurn();
            anotherAlliance.Received().AddBankTurn();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}