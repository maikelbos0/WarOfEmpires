﻿using FluentAssertions;
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
    public sealed class DepositCommandHandlerTests {
        [TestMethod]
        public void DepositCommandHandler_Succeeds() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(3, rank: 5)
                .WithMember(2, out var highestRankedPlayer, rank: 3)
                .BuildMember(1, rank: 7);

            rankService.GetRatio(builder.Player, highestRankedPlayer).Returns(0.6);

            var handler = new DepositCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new DepositCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Deposit(builder.Player, 0.6, new Resources(2, 3, 4, 5, 6));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DepositCommandHandler_Allows_Empty_Values() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(3, rank: 5)
                .WithMember(2, out var highestRankedPlayer, rank: 3)
                .BuildMember(1, rank: 7);

            rankService.GetRatio(builder.Player, highestRankedPlayer).Returns(0.6);

            var handler = new DepositCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new DepositCommand("test1@test.com", null, null, null, null, null);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Deposit(builder.Player, 0.6, new Resources());
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DepositCommandHandler_Fails_For_Too_Little_Resources() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, canAffordAnything: false);

            var handler = new DepositCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new DepositCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources available to deposit that much");
            builder.Alliance.DidNotReceiveWithAnyArgs().Deposit(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DepositCommandHandler_Fails_For_Too_Few_Bank_Turns() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .BuildAlliance(1, bankTurns: 0)
                .WithMember(1);

            var handler = new DepositCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new DepositCommand("test1@test.com", 2, 3, 4, 5, 6);

            var result = handler.Execute(command);

            result.Should().HaveError("Your alliance doesn't have any bank turns available");
            builder.Alliance.DidNotReceiveWithAnyArgs().Deposit(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DepositCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1);

            var handler = new DepositCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new DepositCommand("test1@test.com", 2, 3, 4, 5, 6);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Deposit(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
