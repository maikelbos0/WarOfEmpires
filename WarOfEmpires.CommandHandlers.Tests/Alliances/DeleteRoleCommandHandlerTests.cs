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
    public sealed class DeleteRoleCommandHandlerTests {
        [TestMethod]
        public void DeleteRoleCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1)
                .WithRole(3, out var role, "Test");

            var handler = new DeleteRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeleteRoleCommand("test1@test.com", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().DeleteRole(role);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeleteRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .BuildAlliance(1)
                .WithMember(1)
                .WithRole(3, out var role, "Test");

            var handler = new DeleteRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeleteRoleCommand("wrong@test.com", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().DeleteRole(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DeleteRoleCommandHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .BuildAlliance(1)
                .WithMember(1)
                .BuildAlliance(2)
                .WithRole(3, out var role, "Test");

            var handler = new DeleteRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeleteRoleCommand("test1@test.com", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().DeleteRole(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}