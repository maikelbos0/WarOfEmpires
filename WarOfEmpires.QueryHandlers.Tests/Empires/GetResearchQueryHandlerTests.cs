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
                .WithQueuedResearch(4, ResearchType.CombatMedicine, 3, 0)
                .WithQueuedResearch(2, ResearchType.CombatMedicine, 1, completedResearchTime: 1500)
                .WithQueuedResearch(3, ResearchType.Commerce, 2, completedResearchTime: 1000);

            throw new NotImplementedException();
        }
    }
}
