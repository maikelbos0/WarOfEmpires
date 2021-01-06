using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class NonAggressionPactTests {
        [TestMethod]
        public void NonAggressionPact_Dissolve_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First"), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second"), "OTHR", "The Others");
            var pact = new NonAggressionPact();

            pact.Alliances.Add(firstAlliance);
            pact.Alliances.Add(secondAlliance);
            firstAlliance.NonAggressionPacts.Add(pact);
            secondAlliance.NonAggressionPacts.Add(pact);

            pact.Dissolve();

            pact.Alliances.Should().BeEmpty();
            firstAlliance.NonAggressionPacts.Should().BeEmpty();
            secondAlliance.NonAggressionPacts.Should().BeEmpty();
        }
    }
}
