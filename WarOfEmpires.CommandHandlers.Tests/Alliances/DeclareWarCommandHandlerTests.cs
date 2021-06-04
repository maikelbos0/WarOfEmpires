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
    public sealed class DeclareWarCommandHandlerTests {
        [TestMethod]
        public void SendWarCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1);

            var handler = new DeclareWarCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclareWarCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().DeclareWar(recipient);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendWarCommandHandler_Fails_For_Already_At_War() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithWar(1, recipient);

            var handler = new DeclareWarCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclareWarCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Should().HaveError("You are already at war with this alliance");
            builder.Alliance.DidNotReceiveWithAnyArgs().DeclareWar(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendWarCommandHandler_Throws_Exception_For_Same_Alliance() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var recipient)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithWar(1, recipient);

            var handler = new DeclareWarCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclareWarCommand("test1@test.com", 2);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().DeclareWar(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
