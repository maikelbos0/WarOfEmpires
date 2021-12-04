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
    public sealed class BankCommandHandlerTests {
        [TestMethod]
        public void BankCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Bank(builder.Player, 1, new Resources(2, 3, 4, 5, 6)); // TODO ratio
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BankCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com", null, null, null, null, null);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Bank(builder.Player, 1, new Resources()); // TODO ratio
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BankCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, canAffordAnything: false);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources available to bank that much");
            builder.Alliance.DidNotReceiveWithAnyArgs().Bank(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BankCommandHandler_Fails_For_Too_Few_Bank_Turns() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, bankTurns: 0)
                .WithMember(1);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have any bank turns available");
            builder.Alliance.DidNotReceiveWithAnyArgs().Bank(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TransferResourcesCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var recipient)
                .BuildAlliance(1);

            var handler = new BankCommandHandler(new PlayerRepository(builder.Context));
            var command = new BankCommand("test1@test.com", 2, 3, 4, 5, 6);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Bank(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
