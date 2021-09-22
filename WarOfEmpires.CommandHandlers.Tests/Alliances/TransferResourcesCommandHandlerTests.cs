using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class TransferResourcesCommandHandlerTests {
        [TestMethod]
        public void TransferResourcesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2, out var recipient)
                .BuildMember(1);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TransferResources(recipient, new Resources(3, 4, 5, 6, 7));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2, out var recipient)
                .BuildMember(1);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new TransferResourcesCommand("test1@test.com", 2, null, null, null, null, null);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TransferResources(recipient, new Resources());
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2, out var recipient)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources available to transfer that much");
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var recipient)
                .BuildAlliance(1)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Throws_Exception_For_Self() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context));
            var command = new TransferResourcesCommand("test1@test.com", 1, 3, 4, 5, 6, 7);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
