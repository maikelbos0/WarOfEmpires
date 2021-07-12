using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class NonAggressionPactRequestTests {
        [TestMethod]
        public void NonAggressionPactRequest_Accept_Succeeds() {
            var senderAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient"), "RECV", "The Recipients");
            var request = new NonAggressionPactRequest(senderAlliance, recipientAlliance);

            senderAlliance.SentNonAggressionPactRequests.Add(request);
            recipientAlliance.ReceivedNonAggressionPactRequests.Add(request);

            request.Accept();

            senderAlliance.SentNonAggressionPactRequests.Should().BeEmpty();
            recipientAlliance.ReceivedNonAggressionPactRequests.Should().BeEmpty();
            senderAlliance.NonAggressionPacts.Should().HaveCount(1);
            recipientAlliance.NonAggressionPacts.Should().HaveCount(1);
            recipientAlliance.NonAggressionPacts.Single().Alliances.Should().BeEquivalentTo(senderAlliance, recipientAlliance);
            senderAlliance.ChatMessages.Should().HaveCount(1);
            senderAlliance.ChatMessages.Single().Player.Should().BeNull();
            senderAlliance.ChatMessages.Single().Message.Should().Be("The Senders and The Recipients have entered a non-aggression pact.");
            recipientAlliance.ChatMessages.Should().HaveCount(1);
            recipientAlliance.ChatMessages.Single().Player.Should().BeNull();
            recipientAlliance.ChatMessages.Single().Message.Should().Be("The Senders and The Recipients have entered a non-aggression pact.");            
        }

        [TestMethod]
        public void NonAggressionPactRequest_Reject_Succeeds() {
            var senderAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient"), "RECV", "The Recipients");
            var request = new NonAggressionPactRequest(senderAlliance, recipientAlliance);

            senderAlliance.SentNonAggressionPactRequests.Add(request);
            recipientAlliance.ReceivedNonAggressionPactRequests.Add(request);

            request.Reject();

            senderAlliance.SentNonAggressionPactRequests.Should().BeEmpty();
            recipientAlliance.ReceivedNonAggressionPactRequests.Should().BeEmpty();
            senderAlliance.NonAggressionPacts.Should().BeEmpty();
            recipientAlliance.NonAggressionPacts.Should().BeEmpty();
        }

        [TestMethod]
        public void NonAggressionPactRequest_Withdraw_Succeeds() {
            var senderAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient"), "RECV", "The Recipients");
            var request = new NonAggressionPactRequest(senderAlliance, recipientAlliance);

            senderAlliance.SentNonAggressionPactRequests.Add(request);
            recipientAlliance.ReceivedNonAggressionPactRequests.Add(request);

            request.Withdraw();

            recipientAlliance.ReceivedNonAggressionPactRequests.Should().BeEmpty();
            senderAlliance.NonAggressionPacts.Should().BeEmpty();
            recipientAlliance.NonAggressionPacts.Should().BeEmpty();
        }
    }
}
