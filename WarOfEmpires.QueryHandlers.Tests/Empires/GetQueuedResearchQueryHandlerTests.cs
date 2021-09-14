using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetQueuedResearchQueryHandlerTests {
        [TestMethod]
        public void GetResearchQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithResearch(1, ResearchType.CombatMedicine, 2)
                .WithQueuedResearch(2, ResearchType.CombatMedicine, 3, 0)
                .WithQueuedResearch(4, ResearchType.CombatMedicine, 1, completedResearchTime: 1500)
                .WithQueuedResearch(3, ResearchType.Commerce, 2, completedResearchTime: 1000);

            var handler = new GetQueuedResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetQueuedResearchQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);

            var combatMedicine1 = result.Should().ContainSingle(r => r.Id == 4).Subject;

            combatMedicine1.Type.Should().Be("CombatMedicine");
            combatMedicine1.Name.Should().Be("Combat medicine");
            combatMedicine1.Priority.Should().Be(1);
            combatMedicine1.CompletedResearchTime.Should().Be(1500);
            combatMedicine1.NeededResearchTime.Should().Be(55000);

            var commerce = result.Should().ContainSingle(r => r.Id == 3).Subject;

            commerce.Type.Should().Be("Commerce");
            commerce.Name.Should().Be("Commerce");
            commerce.Priority.Should().Be(2);
            commerce.CompletedResearchTime.Should().Be(1000);
            commerce.NeededResearchTime.Should().Be(20000);

            var combatMedicine2 = result.Should().ContainSingle(r => r.Id == 2).Subject;

            combatMedicine2.Type.Should().Be("CombatMedicine");
            combatMedicine2.Name.Should().Be("Combat medicine");
            combatMedicine2.Priority.Should().Be(3);
            combatMedicine2.CompletedResearchTime.Should().Be(0);
            combatMedicine2.NeededResearchTime.Should().Be(155000);
        }
    }
}
