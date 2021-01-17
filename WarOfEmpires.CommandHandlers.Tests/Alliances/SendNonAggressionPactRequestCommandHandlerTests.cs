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
    public sealed class SendNonAggressionPactRequestCommandHandlerTests {
        [TestMethod]
        public void SendNonAggressionPactRequestCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1);

            var handler = new SendNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new SendNonAggressionPactRequestCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().SendNonAggressionPactRequest(recipient);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendNonAggressionPactRequestCommandHandler_Fails_For_Already_Sent() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPactRequestTo(1, recipient);

            var handler = new SendNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new SendNonAggressionPactRequestCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Should().HaveError("This alliance already has an outstanding non aggression pact request from your alliance");
            builder.Alliance.DidNotReceiveWithAnyArgs().SendNonAggressionPactRequest(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendNonAggressionPactRequestCommandHandler_Fails_For_Already_In_Pact() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPact(1, recipient);

            var handler = new SendNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new SendNonAggressionPactRequestCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Should().HaveError("Your alliance is already in a non aggression pact with this alliance");
            builder.Alliance.DidNotReceiveWithAnyArgs().SendNonAggressionPactRequest(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendNonAggressionPactRequestCommandHandler_Throws_Exception_For_Same_Alliance() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPact(1, recipient);

            var handler = new SendNonAggressionPactRequestCommandHandler(new AllianceRepository(builder.Context));
            var command = new SendNonAggressionPactRequestCommand("test1@test.com", 2);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().SendNonAggressionPactRequest(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
