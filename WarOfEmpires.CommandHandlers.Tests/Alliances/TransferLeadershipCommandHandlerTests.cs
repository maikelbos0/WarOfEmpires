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
    public sealed class TransferLeadershipCommandHandlerTests {
        [TestMethod]
        public void TransferLeadershipCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2, out var member);

            var handler = new TransferLeadershipCommandHandler(new AllianceRepository(builder.Context));
            var command = new TransferLeadershipCommand("test1@test.com", 2);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            //TODO enable
            //builder.Alliance.Received().TransferLeadership(member);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TransferLeadershipCommandHandler_Throws_Exception_When_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1)
                .WithLeader(2)
                .WithMember(3);

            var handler = new TransferLeadershipCommandHandler(new AllianceRepository(builder.Context));
            var command = new TransferLeadershipCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            //TODO enable
            //builder.Alliance.DidNotReceiveWithAnyArgs().TransferLeadership(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferLeadershipCommandHandler_Throws_Exception_When_Not_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(2)
                .WithMember(1)
                .WithMember(3);

            var handler = new TransferLeadershipCommandHandler(new AllianceRepository(builder.Context));
            var command = new TransferLeadershipCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            //TODO enable
            //builder.Alliance.DidNotReceiveWithAnyArgs().TransferLeadership(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferLeadershipCommandHandler_Throws_Exception_For_Member_Current_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new TransferLeadershipCommandHandler(new AllianceRepository(builder.Context));
            var command = new TransferLeadershipCommand("test1@test.com", 1);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            //TODO enable
            //builder.Alliance.DidNotReceiveWithAnyArgs().TransferLeadership(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferLeadershipCommandHandler_Throws_Exception_For_Member_Not_In_Alliance () {
            var builder = new FakeBuilder()
                .WithPlayer(2)
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new TransferLeadershipCommandHandler(new AllianceRepository(builder.Context));
            var command = new TransferLeadershipCommand("test1@test.com", 2);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            //TODO enable
            //builder.Alliance.DidNotReceiveWithAnyArgs().TransferLeadership(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
