using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class SendInviteCommandHandlerTests {
        [TestMethod]
        public void SendInviteCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new SendInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendInviteCommand("test1@test.com", "2", "Test message", "Message body");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Alliance.Invites.Single().Subject.Should().Be("Test message");
            builder.Alliance.Invites.Single().Body.Should().Be("Message body");
            builder.Alliance.Invites.Single().Player.Should().Be(player);
            builder.Alliance.Invites.Single().Alliance.Should().Be(builder.Alliance);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Fails_For_Player_Already_Invited() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithMember(1)
                .WithInvite(1, player);

            var handler = new SendInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendInviteCommand("test1@test.com", "2", "Test message", "Message body");

            var result = handler.Execute(command);

            result.Should().HaveError("This player already has an open invite and can not be invited again");
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Sender_Not_Empire_Member() {
            var builder = new FakeBuilder()
                .WithPlayer(2)
                .WithPlayer(1);

            var handler = new SendInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendInviteCommand("test1@test.com", "2", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Alphanumeric_Player_Id() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new SendInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendInviteCommand("test1@test.com", "A", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            builder.Alliance.Invites.Should().HaveCount(0);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Nonexistent_Player_Id() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new SendInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new SendInviteCommand("test1@test.com", "3", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.Invites.Should().HaveCount(0);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}