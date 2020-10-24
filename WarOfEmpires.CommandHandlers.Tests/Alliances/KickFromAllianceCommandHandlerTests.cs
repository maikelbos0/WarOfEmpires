using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class KickFromAllianceCommandHandlerTests {
        [TestMethod]
        public void KickFromAllianceCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2, out var member);

            var handler = new KickFromAllianceCommandHandler(new AllianceRepository(builder.Context));
            var command = new KickFromAllianceCommand("test1@test.com", "2");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().RemoveMember(member);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Fails_For_Member_Alliance_Leader() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void KickFromAllianceCommandHandler_Throws_Exception_For_Member_Not_In_Alliance() {
            throw new System.NotImplementedException();
        }
    }
}