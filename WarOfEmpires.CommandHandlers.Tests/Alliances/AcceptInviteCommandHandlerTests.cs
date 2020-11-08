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
    public sealed class AcceptInviteCommandHandlerTests {
        [TestMethod]
        public void AcceptInviteCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, out var invite, player);
            
            var handler = new AcceptInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new AcceptInviteCommand("test1@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().AcceptInvite(invite);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Fails_For_Player_In_Empire() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithMember(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new AcceptInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new AcceptInviteCommand("test1@test.com", "1");

            var result = handler.Execute(command);

            result.Should().HaveError("You are already in an alliance; leave your current alliance before accepting an invite");
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Alliance.DidNotReceiveWithAnyArgs().AcceptInvite(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_For_Alphanumeric_InviteId() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new AcceptInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new AcceptInviteCommand("test1@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Alliance.DidNotReceiveWithAnyArgs().AcceptInvite(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_For_Nonexistent_InviteId() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new AcceptInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new AcceptInviteCommand("test1@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Alliance.DidNotReceiveWithAnyArgs().AcceptInvite(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_InviteId_For_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .WithPlayer(2, email: "wrong@test.com")
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new AcceptInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new AcceptInviteCommand("wrong@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Alliance.DidNotReceiveWithAnyArgs().AcceptInvite(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}