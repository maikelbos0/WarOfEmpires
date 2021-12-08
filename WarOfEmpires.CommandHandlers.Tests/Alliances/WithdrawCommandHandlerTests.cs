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
    public sealed class WithdrawCommandHandlerTests {
        [TestMethod]
        public void WithdrawCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, bankedResources: new Resources(2, 3, 4, 5, 6))
                .BuildMember(1);

            var handler = new WithdrawCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Withdraw(builder.Player, new Resources(2, 3, 4, 5, 6));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1);

            var handler = new WithdrawCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCommand("test1@test.com", null, null, null, null, null);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Withdraw(builder.Player, new Resources());
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new WithdrawCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("Your alliance doesn't have enough resources available to withdraw that much");
            builder.Alliance.DidNotReceiveWithAnyArgs().Withdraw(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawCommandHandler_Fails_For_Too_Few_Bank_Turns() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, bankedResources: new Resources(2, 3, 4, 5, 6), bankTurns: 0)
                .WithMember(1);

            var handler = new WithdrawCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("Your alliance doesn't have any bank turns available");
            builder.Alliance.DidNotReceiveWithAnyArgs().Withdraw(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1);

            var handler = new WithdrawCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCommand("test1@test.com", 2, 3, 4, 5, 6);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Withdraw(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
