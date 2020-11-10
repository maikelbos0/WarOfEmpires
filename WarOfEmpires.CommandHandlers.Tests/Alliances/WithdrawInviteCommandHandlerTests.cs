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
    public sealed class WithdrawInviteCommandHandlerTests {
        [TestMethod]
        public void WithdrawInviteCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithMember(1)
                .WithInvite(3, out var invite, player);

            var handler = new WithdrawInviteCommandHandler(new AllianceRepository(builder.Context));
            var command = new WithdrawInviteCommand("test1@test.com", 3);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().RemoveInvite(invite);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Player_Not_Empire_Member() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithInvite(3, player);

            var handler = new WithdrawInviteCommandHandler(new AllianceRepository(builder.Context));
            var command = new WithdrawInviteCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Nonexistent_Invite_Id() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithMember(1)
                .WithInvite(3, player);

            var handler = new WithdrawInviteCommandHandler(new AllianceRepository(builder.Context));
            var command = new WithdrawInviteCommand("test1@test.com", 4);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Invite_Id_Other_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildAlliance(1)
                .WithMember(1)
                .BuildAlliance(2)
                .WithInvite(3, player);

            var handler = new WithdrawInviteCommandHandler(new AllianceRepository(builder.Context));
            var command = new WithdrawInviteCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.Invites.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}