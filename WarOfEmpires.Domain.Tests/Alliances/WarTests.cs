using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class WarTests {
        [TestMethod]
        public void War_DeclarePeace_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First", Race.Elves), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second", Race.Elves), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);

            war.DeclarePeace(firstAlliance);

            war.PeaceDeclarations.Should().BeEquivalentTo(firstAlliance);
            firstAlliance.ChatMessages.Should().BeEmpty();
            secondAlliance.ChatMessages.Should().BeEmpty();
        }

        [TestMethod]
        public void War_DeclarePeace_Succeeds_Last_Alliance() {
            var firstAlliance = new Alliance(new Player(1, "First", Race.Elves), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second", Race.Elves), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);
            war.PeaceDeclarations.Add(secondAlliance);

            war.DeclarePeace(firstAlliance);

            war.PeaceDeclarations.Should().BeEquivalentTo(firstAlliance, secondAlliance);
            firstAlliance.Wars.Should().BeEmpty();
            secondAlliance.Wars.Should().BeEmpty();
            firstAlliance.ChatMessages.Should().HaveCount(1);
            firstAlliance.ChatMessages.Single().Player.Should().BeNull();
            firstAlliance.ChatMessages.Single().Message.Should().Be("The war between The First and The Others has ended.");
            secondAlliance.ChatMessages.Should().HaveCount(1);
            secondAlliance.ChatMessages.Single().Player.Should().BeNull();
            secondAlliance.ChatMessages.Single().Message.Should().Be("The war between The First and The Others has ended.");
        }

        [TestMethod]
        public void War_CancelPeaceDeclaration_Succeeds() {
            var firstAlliance = new Alliance(new Player(1, "First", Race.Elves), "1ST", "The First");
            var secondAlliance = new Alliance(new Player(2, "Second", Race.Elves), "OTHR", "The Others");
            var war = new War();

            war.Alliances.Add(firstAlliance);
            war.Alliances.Add(secondAlliance);
            war.PeaceDeclarations.Add(firstAlliance);

            war.CancelPeaceDeclaration(firstAlliance);

            war.PeaceDeclarations.Should().BeEmpty();
        }
    }
}
