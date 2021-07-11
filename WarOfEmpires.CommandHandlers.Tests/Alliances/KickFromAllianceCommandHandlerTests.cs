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
    public sealed class KickFromAllianceCommandHandlerTests {
        [TestMethod]
        public void KickFromAllianceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2, out var member);

            var handler = new KickFromAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new KickFromAllianceCommand("test1@test.com", "2");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Kick(member);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Throws_Exception_For_Member_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new KickFromAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new KickFromAllianceCommand("test1@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Kick(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1)
                .WithMember(2);

            var handler = new KickFromAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new KickFromAllianceCommand("test1@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Kick(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Throws_Exception_For_Member_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1)
                .WithMember(2);

            var handler = new KickFromAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new KickFromAllianceCommand("test2@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Kick(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}