using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class WarTests {
        [TestMethod]
        public void War_DeclarePeace_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First"), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second"), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);

            war.DeclarePeace(firstAlliance);

            war.PeaceDeclarations.Should().BeEquivalentTo(firstAlliance);
        }

        [TestMethod]
        public void War_DeclarePeace_Succeeds_Last_Alliance() {
            var firstAlliance = new Alliance(new Player(1, "First"), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second"), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);
            war.PeaceDeclarations.Add(secondAlliance);

            war.DeclarePeace(firstAlliance);

            war.PeaceDeclarations.Should().BeEquivalentTo(firstAlliance, secondAlliance);
            firstAlliance.Wars.Should().BeEmpty();
            secondAlliance.Wars.Should().BeEmpty();
        }

        [TestMethod]
        public void War_CancelPeaceDeclaration_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First"), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second"), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);
            war.PeaceDeclarations.Add(firstAlliance);

            war.CancelPeaceDeclaration(firstAlliance);

            war.PeaceDeclarations.Should().BeEmpty();
        }
    }
}
