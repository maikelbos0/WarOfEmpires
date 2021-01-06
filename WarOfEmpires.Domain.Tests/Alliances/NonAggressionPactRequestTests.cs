using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public class NonAggressionPactRequestTests {
        [TestMethod]
        public void NonAggressionPactRequest_Accept_Succeeds() {
            var senderAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient"), "RECV", "The Recipients");
            var request = new NonAggressionPactRequest(senderAlliance, recipientAlliance);

            request.Accept();

            senderAlliance.SentNonAggressionPactRequests.Should().BeEmpty();
            recipientAlliance.ReceivedNonAggressionPactRequests.Should().BeEmpty();
            senderAlliance.NonAggressionPacts.Should().HaveCount(1);
            recipientAlliance.NonAggressionPacts.Should().HaveCount(1);
            recipientAlliance.NonAggressionPacts.Single().Alliances.Should().BeEquivalentTo(senderAlliance, recipientAlliance);
        }
    }
}
