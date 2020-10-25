using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class LeaveAllianceCommandHandlerTests {
        [TestMethod]
        public void LeaveAllianceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2, out var member);

            var handler = new LeaveAllianceCommandHandler(new PlayerRepository(builder.Context));
            var command = new LeaveAllianceCommand("test2@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().RemoveMember(member);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void LeaveAllianceCommandHandler_Throws_Exception_For_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new LeaveAllianceCommandHandler(new PlayerRepository(builder.Context));
            var command = new LeaveAllianceCommand("test1@test.com");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().RemoveMember(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void LeaveAllianceCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new LeaveAllianceCommandHandler(new PlayerRepository(builder.Context));
            var command = new LeaveAllianceCommand("test1@test.com");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}