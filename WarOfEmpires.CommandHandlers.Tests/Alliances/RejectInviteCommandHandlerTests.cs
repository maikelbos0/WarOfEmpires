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
    public sealed class RejectInviteCommandHandlerTests {
        [TestMethod]
        public void RejectInviteCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, out var invite, player);

            var handler = new RejectInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new RejectInviteCommand("test1@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().RemoveInvite(invite);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void RejectInviteCommandHandler_Throws_Exception_For_Alphanumeric_InviteId() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new RejectInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new RejectInviteCommand("test1@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void RejectInviteCommandHandler_Throws_Exception_For_Nonexistent_InviteId() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new RejectInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new RejectInviteCommand("test1@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void RejectInviteCommandHandler_Throws_Exception_For_InviteId_For_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player);

            var handler = new RejectInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new RejectInviteCommand("wrong@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            player.Alliance.Should().BeNull();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}