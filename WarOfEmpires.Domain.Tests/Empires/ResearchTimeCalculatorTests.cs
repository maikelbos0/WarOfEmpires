using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class ResearchTimeCalculatorTests {
        [DataTestMethod]
        [DataRow(0, 0, 5000)]
        [DataRow(6, 0, 35000)]
        [DataRow(12, 0, 65000)]
        [DataRow(6, 2, 75000)]
        [DataRow(12, 2, 105000)]
        [DataRow(18, 2, 135000)]
        [DataRow(12, 4, 465000)]
        [DataRow(18, 4, 495000)]
        [DataRow(24, 4, 525000)]
        [DataRow(18, 6, 3735000)]
        [DataRow(24, 6, 3765000)]
        [DataRow(30, 6, 3795000)]
        public void ResearchTimeCalculator_GetResearchTime_Succeeds(int completedResearchCount, int completedResearchLevelForType, long expectedResearchTime) {
            ResearchTimeCalculator.GetResearchTime(completedResearchCount, completedResearchLevelForType).Should().Be(expectedResearchTime);
        }
    }
}
