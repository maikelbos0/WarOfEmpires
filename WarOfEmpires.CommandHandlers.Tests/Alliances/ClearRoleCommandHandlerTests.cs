﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class ClearRoleCommandHandlerTests {
        [TestMethod]
        public void ClearRoleCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var player)
                .WithRole(1, "Test", player);

            var handler = new ClearRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new ClearRoleCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().ClearRole(player);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ClearRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new ClearRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new ClearRoleCommand("test1@test.com", 1);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void ClearRoleCommandHandler_Throws_Exception_For_Member_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1)
                .BuildAlliance(2)
                .WithMember(2, out var player)
                .WithRole(1, "Test", player);

            var handler = new ClearRoleCommandHandler(new AllianceRepository(builder.Context));
            var command = new ClearRoleCommand("test1@test.com", 2);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().ClearRole(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}