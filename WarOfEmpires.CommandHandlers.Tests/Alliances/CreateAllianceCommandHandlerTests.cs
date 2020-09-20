using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class CreateAllianceCommandHandlerTests {
        [TestMethod]
        public void CreateAllianceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player);

            var handler = new CreateAllianceCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new CreateAllianceCommand("test1@test.com", "CODE", "The Alliance");

            var result = handler.Execute(command);
            var alliance = builder.Context.Alliances.SingleOrDefault();

            result.Success.Should().BeTrue();
            alliance.Should().NotBeNull();
            alliance.Code.Should().Be("CODE");
            alliance.Name.Should().Be("The Alliance");
            alliance.Leader.Should().Be(player);
            alliance.Members.Should().Contain(player);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void CreateAllianceCommandHandler_Throws_Exception_For_Nonexistent_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new CreateAllianceCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new CreateAllianceCommand("wrong@test.com", "CODE", "The Alliance");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Context.Alliances.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CreateAllianceCommandHandler_Fails_For_Too_Long_Code() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new CreateAllianceCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new CreateAllianceCommand("test1@test.com", "CODE1", "The Alliance");

            var result = handler.Execute(command);

            result.Should().HaveError("Code", "Code must be 4 characters or less");
            builder.Context.Alliances.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CreateAllianceCommandHandler_Fails_For_Player_In_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new CreateAllianceCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new CreateAllianceCommand("test1@test.com", "CODE", "The Alliance");

            var result = handler.Execute(command);

            result.Should().HaveError("You are already in an alliance; you have to leave before you can create an alliance");
            builder.Context.Alliances.Should().HaveCount(1);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}