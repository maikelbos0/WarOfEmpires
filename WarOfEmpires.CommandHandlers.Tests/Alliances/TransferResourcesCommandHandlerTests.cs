using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class TransferResourcesCommandHandlerTests {
        [TestMethod]
        public void TransferResourcesCommandHandler_Succeeds() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2, out var recipient)
                .BuildMember(1);

            rankService.GetRatio(builder.Player, recipient).Returns(1.2);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TransferResources(1.2, recipient, new Resources(3, 4, 5, 6, 7));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Allows_Empty_Values() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2, out var recipient)
                .BuildMember(1);

            rankService.GetRatio(builder.Player, recipient).Returns(1.2);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new TransferResourcesCommand("test1@test.com", 2, null, null, null, null, null);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TransferResources(1.2, recipient, new Resources());
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Fails_For_Too_Little_Resources() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(2)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources available to transfer that much");
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .WithPlayer(2, out var recipient)
                .BuildAlliance(1)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new TransferResourcesCommand("test1@test.com", 2, 3, 4, 5, 6, 7);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Throws_Exception_For_Self() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1, canAffordAnything: false);

            var handler = new TransferResourcesCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new TransferResourcesCommand("test1@test.com", 1, 3, 4, 5, 6, 7);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().TransferResources(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
