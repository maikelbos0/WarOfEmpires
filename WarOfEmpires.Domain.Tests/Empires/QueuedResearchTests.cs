using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class QueuedResearchTests {
        [TestMethod]
        public void QueuedResearch_ProcessTurn_Succeeds_When_Not_Completed() {
            var player = new Player(0, "Test", Race.Elves);
            var research = new QueuedResearch(player, 1, ResearchType.CombatMedicine);

            player.Research.Add(new Research(ResearchType.CombatMedicine) { Level = 2 });
            player.Research.Add(new Research(ResearchType.Commerce) { Level = 2 });
            player.QueuedResearch.Add(research);

            research.ProcessTurn(64999);

            research.CompletedResearchTime.Should().Be(64999);
            player.Research.Should().ContainSingle(r => r.Type == ResearchType.CombatMedicine && r.Level == 2);
            player.QueuedResearch.Should().BeEquivalentTo(research);
        }

        [TestMethod]
        public void QueuedResearch_ProcessTurn_Succeeds_When_Completed_New() {
            var player = new Player(0, "Test", Race.Elves);
            var research = new QueuedResearch(player, 1, ResearchType.CombatMedicine);

            player.Research.Add(new Research(ResearchType.Tactics) { Level = 2 });
            player.Research.Add(new Research(ResearchType.Commerce) { Level = 2 });
            player.QueuedResearch.Add(research);

            research.ProcessTurn(25000);

            research.CompletedResearchTime.Should().Be(25000);
            player.Research.Should().ContainSingle(r => r.Type == ResearchType.CombatMedicine && r.Level == 1);
            player.QueuedResearch.Should().BeEmpty();
        }

        [TestMethod]
        public void QueuedResearch_ProcessTurn_Succeeds_When_Completed_Existing() {
            var player = new Player(0, "Test", Race.Elves);
            var research = new QueuedResearch(player, 1, ResearchType.CombatMedicine);

            player.Research.Add(new Research(ResearchType.CombatMedicine) { Level = 2 });
            player.Research.Add(new Research(ResearchType.Commerce) { Level = 2 });
            player.QueuedResearch.Add(research);

            research.ProcessTurn(65000);

            research.CompletedResearchTime.Should().Be(65000);
            player.Research.Should().ContainSingle(r => r.Type == ResearchType.CombatMedicine && r.Level == 3);
            player.QueuedResearch.Should().BeEmpty();
        }
    }
}
