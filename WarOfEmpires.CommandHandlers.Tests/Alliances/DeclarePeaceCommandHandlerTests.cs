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
    public sealed class DeclarePeaceCommandHandlerTests {
        [TestMethod]
        public void DeclarePeaceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(2, out var alliance)
                .BuildAlliance(1)
                .WithMember(1)
                .WithWar(3, out var war, alliance);

            var handler = new DeclarePeaceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclarePeaceCommand("test1@test.com", 3);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            war.Received().DeclarePeace(builder.Alliance);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeclarePeaceCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .WithAlliance(2, out var alliance)
                .BuildAlliance(1)
                .WithMember(1)
                .WithWar(3, out var war, alliance);

            var handler = new DeclarePeaceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclarePeaceCommand("wrong@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            war.DidNotReceiveWithAnyArgs().DeclarePeace(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DeclarePeaceCommandHandler_Throws_Exception_For_War_Of_Different_Alliance() {
            var builder = new FakeBuilder()
                .WithAlliance(2, out var alliance)
                .BuildAlliance(1)
                .WithMember(1)
                .BuildAlliance(3)
                .WithWar(3, out var war, alliance);

            var handler = new DeclarePeaceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclarePeaceCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            war.DidNotReceiveWithAnyArgs().DeclarePeace(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DeclarePeaceCommandHandler_Throws_Exception_For_Already_Declared() {
            var builder = new FakeBuilder()
                .WithAlliance(2, out var alliance)
                .BuildAlliance(1)
                .WithMember(1)
                .WithWar(3, out var war, alliance, peaceDeclared: true);

            var handler = new DeclarePeaceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DeclarePeaceCommand("test1@test.com", 3);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            war.DidNotReceiveWithAnyArgs().DeclarePeace(default);
        }
    }
}
