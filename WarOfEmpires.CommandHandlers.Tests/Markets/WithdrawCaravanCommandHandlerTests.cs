using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class WithdrawCaravanCommandHandlerTests {
        [TestMethod]
        public void WithdrawCaravanCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithCaravan(1, out var caravan);

            var handler = new WithdrawCaravanCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCaravanCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();

            caravan.Received().Withdraw();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Invalid_CaravanId() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithCaravan(1, out var caravan);

            var handler = new WithdrawCaravanCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCaravanCommand("test1@test.com", 51);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            caravan.DidNotReceiveWithAnyArgs().Withdraw();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Caravan_Of_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .BuildPlayer(1)
                .WithCaravan(1, out var caravan);

            var handler = new WithdrawCaravanCommandHandler(new PlayerRepository(builder.Context));
            var command = new WithdrawCaravanCommand("wrong@test.com", 1);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            caravan.DidNotReceiveWithAnyArgs().Withdraw();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}