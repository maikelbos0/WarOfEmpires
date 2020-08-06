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
    public sealed class SetRoleCommandHandlerTests {
        [TestMethod]
        public void SetRoleCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var player)
                .WithRole(3, out var role, "Test");

            var handler = new SetRoleCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetRoleCommand("test1@test.com", "1", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().SetRole(player, role);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .BuildAlliance(1)
                .WithMember(1)
                .WithRole(3, "Test");

            var handler = new SetRoleCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetRoleCommand("wrong@test.com", "1", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithRole(3, "Test")
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new SetRoleCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetRoleCommand("test1@test.com", "1", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Member_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2)
                .BuildAlliance(2)
                .WithMember(1)
                .WithRole(3, "Test");

            var handler = new SetRoleCommandHandler(new PlayerRepository(builder.Context));
            var command = new SetRoleCommand("test1@test.com", "2", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}