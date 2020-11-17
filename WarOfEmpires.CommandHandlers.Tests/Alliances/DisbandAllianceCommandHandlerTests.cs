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
    public sealed class DisbandAllianceCommandHandlerTests {
        [TestMethod]
        public void DisbandAllianceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new DisbandAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DisbandAllianceCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Disband();
            builder.Context.Alliances.Should().NotContain(builder.Alliance);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DisbandAllianceCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1);

            var handler = new DisbandAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new DisbandAllianceCommand("test1@test.com");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Disband();
            builder.Context.Alliances.Should().Contain(builder.Alliance);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
