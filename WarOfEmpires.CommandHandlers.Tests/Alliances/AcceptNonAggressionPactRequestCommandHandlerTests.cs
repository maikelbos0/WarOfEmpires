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
    public sealed class AcceptNonAggressionPactRequestCommandHandlerTests {
        [TestMethod]
        public void AcceptNonAggressionPactRequestCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var sender)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPactRequestFrom(1, out var request, sender);

            var handler = new AcceptNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new AcceptNonAggressionPactRequestCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            request.Received().Accept();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AcceptNonAggressionPactRequestCommandHandler_Throws_Exception_For_Wrong_Alliance() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPactRequestTo(1, out var request, recipient);

            var handler = new AcceptNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new AcceptNonAggressionPactRequestCommand("test1@test.com", 1);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            request.DidNotReceiveWithAnyArgs().Accept();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
