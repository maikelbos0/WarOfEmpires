using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class NonAggressionPactTests {
        [TestMethod]
        public void NonAggressionPact_Dissolve_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First", Race.Elves), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second", Race.Elves), "OTHR", "The Others");
            var pact = new NonAggressionPact();

            pact.Alliances.Add(firstAlliance);
            pact.Alliances.Add(secondAlliance);
            firstAlliance.NonAggressionPacts.Add(pact);
            secondAlliance.NonAggressionPacts.Add(pact);

            pact.Dissolve();

            firstAlliance.NonAggressionPacts.Should().BeEmpty();
            secondAlliance.NonAggressionPacts.Should().BeEmpty();
            firstAlliance.ChatMessages.Should().HaveCount(1);
            firstAlliance.ChatMessages.Single().Player.Should().BeNull();
            firstAlliance.ChatMessages.Single().Message.Should().Be("The non-aggression pact between The First and The Others has been dissolved.");
            secondAlliance.ChatMessages.Should().HaveCount(1);
            secondAlliance.ChatMessages.Single().Player.Should().BeNull();
            secondAlliance.ChatMessages.Single().Message.Should().Be("The non-aggression pact between The First and The Others has been dissolved.");
        }
    }
}
