using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class DissolveNonAggressionPactCommandHandlerTests {
        [TestMethod]
        public void DissolveNonAggressionPactCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPact(3, out var pact, alliance);

            var handler = new DissolveNonAggressionPactCommandHandler(new AllianceRepository(builder.Context));
            var command = new DissolveNonAggressionPactCommand("test1@test.com", 3);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            pact.Received().Dissolve();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DissolveNonAggressionPactCommandHandler_Throws_Exception_For_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .BuildAlliance(2)
                .WithNonAggressionPact(3, out var pact, alliance)
                .BuildAlliance(3)
                .WithLeader(1);

            var handler = new DissolveNonAggressionPactCommandHandler(new AllianceRepository(builder.Context));
            var command = new DissolveNonAggressionPactCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();            
            pact.DidNotReceiveWithAnyArgs().Dissolve();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
