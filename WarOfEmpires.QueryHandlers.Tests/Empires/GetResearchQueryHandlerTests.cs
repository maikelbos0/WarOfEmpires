using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetResearchQueryHandlerTests {
        [TestMethod]
        public void GetResearchQueryHandler_Returns_Correct_Research() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithResearch(1, ResearchType.CombatMedicine, 2);

            var handler = new GetResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetResearchQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Research.Should().HaveCount(Enum.GetValues(typeof(ResearchType)).Length);

            var tactics = result.Research.Should().ContainSingle(r => r.Type == ResearchType.Tactics.ToString()).Subject;

            tactics.Name.Should().Be("Tactics");
            tactics.Description.Should().Be("Tactics increases the number of casualties your army inflicts on the battlefield");
            tactics.Level.Should().Be(0);
            tactics.LevelBonus.Should().Be(0.05M);
            tactics.CurrentBonus.Should().Be(0M);

            var combatMedicine = result.Research.Should().ContainSingle(r => r.Type == ResearchType.CombatMedicine.ToString()).Subject;

            combatMedicine.Name.Should().Be("Combat medicine");
            combatMedicine.Description.Should().Be("Combat medicine lowers the number of casualties your army suffers in battle");
            combatMedicine.Level.Should().Be(0);
            combatMedicine.LevelBonus.Should().Be(0.04M);
            combatMedicine.CurrentBonus.Should().Be(0.08M);
        }

        [TestMethod]
        public void GetResearchQueryHandler_Returns_Correct_QueuedResearch() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithResearch(1, ResearchType.CombatMedicine, 2)
                .WithQueuedResearch(2, ResearchType.CombatMedicine, 3, 0)
                .WithQueuedResearch(4, ResearchType.CombatMedicine, 1, completedResearchTime: 1500)
                .WithQueuedResearch(3, ResearchType.Commerce, 2, completedResearchTime: 1000);

            var handler = new GetResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetResearchQuery("test1@test.com");

            var result = handler.Execute(query);

            result.QueuedResearch.Should().HaveCount(3);

            var combatMedicine1 = result.QueuedResearch.Should().ContainSingle(r => r.Id == 4).Subject;

            combatMedicine1.Type.Should().Be("CombatMedicine");
            combatMedicine1.Name.Should().Be("Combat medicine");
            combatMedicine1.Priority.Should().Be(1);
            combatMedicine1.CompletedResearchTime.Should().Be(1500);
            combatMedicine1.NeededResearchTime.Should().Be(55000);

            var commerce = result.QueuedResearch.Should().ContainSingle(r => r.Id == 3).Subject;

            commerce.Type.Should().Be("Commerce");
            commerce.Name.Should().Be("Commerce");
            commerce.Priority.Should().Be(2);
            commerce.CompletedResearchTime.Should().Be(0);
            commerce.NeededResearchTime.Should().Be(20000);

            var combatMedicine2 = result.QueuedResearch.Should().ContainSingle(r => r.Id == 2).Subject;

            combatMedicine2.Type.Should().Be("CombatMedicine");
            combatMedicine2.Name.Should().Be("Combat medicine");
            combatMedicine2.Priority.Should().Be(3);
            combatMedicine2.CompletedResearchTime.Should().Be(0);
            combatMedicine2.NeededResearchTime.Should().Be(155000);
        }
    }
}
