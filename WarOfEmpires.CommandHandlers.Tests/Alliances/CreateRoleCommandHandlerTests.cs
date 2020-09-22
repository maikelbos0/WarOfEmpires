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
    public sealed class CreateRoleCommandHandlerTests {
        [TestMethod]
        public void CreateRoleCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new CreateRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new CreateRoleCommand("test1@test.com", "Diva", true);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().CreateRole("Diva");
            builder.Context.CallsToSaveChanges.Should().Be(1);

            throw new NotImplementedException("Add CanInvite to test and command handler");
        }

        [TestMethod]
        public void CreateRoleCommandHandler_Throws_Exception_For_Nonexistent_Player() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new CreateRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new CreateRoleCommand("wrong@test.com", "Diva", true);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().CreateRole(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CreateRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new CreateRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new CreateRoleCommand("test1@test.com", "Diva", true);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}