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
        public void GetResearchQueryHandler_Returns_Correct_Values_For_Existing_Research() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithResearch(1, ResearchType.CombatMedicine, 2);

            var handler = new GetResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetResearchQuery("test1@test.com", "CombatMedicine");

            var result = handler.Execute(query);

            result.ResearchType.Should().Be("CombatMedicine");
            result.Name.Should().Be("Combat medicine");
            result.Description.Should().Be("Combat medicine lowers the number of casualties your army suffers in battle");
            result.Level.Should().Be(2);
            result.LevelBonus.Should().Be(0.04M);
            result.CurrentBonus.Should().Be(0.08M);
        }

        [TestMethod]
        public void GetResearchQueryHandler_Returns_Correct_Values_For_New_Research() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetResearchQuery("test1@test.com", "CombatMedicine");

            var result = handler.Execute(query);

            result.ResearchType.Should().Be("CombatMedicine"); 
            result.Name.Should().Be("Combat medicine");
            result.Description.Should().Be("Combat medicine lowers the number of casualties your army suffers in battle");
            result.Level.Should().Be(0);
            result.LevelBonus.Should().Be(0.04M);
            result.CurrentBonus.Should().Be(0M);
        }

        [TestMethod]
        public void GetResearchQueryHandler_Throws_Exception_For_Invalid_BuildingType() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetResearchQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetResearchQuery("test1@test.com", "Wrong");

            Action action = () => handler.Execute(query);

            action.Should().Throw<ArgumentException>();
        }
    }
}
