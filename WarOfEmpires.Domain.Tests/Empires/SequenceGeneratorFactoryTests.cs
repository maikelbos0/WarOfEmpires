using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class SequenceGeneratorFactoryTests {
        [DataTestMethod]
        [DataRow(0, 2, DisplayName = "First of first round")]
        [DataRow(1, 3, DisplayName = "Second of first round")]
        [DataRow(4, 13, DisplayName = "Last of first round")]
        [DataRow(5, 20, DisplayName = "First of second round")]
        [DataRow(9, 130, DisplayName = "Last of second round")]
        [DataRow(12, 500, DisplayName = "Third of third round")]
        public void SequenceGeneratorFactory_GetValue_Works(int level, int expectedResult) {
            SequenceGeneratorFactory.GetValue(level).Should().Be(expectedResult);
        }

        [TestMethod]
        public void SequenceGeneratorFactory_GetGeneratorFunction_Works() {
            var generator = SequenceGeneratorFactory.GetGeneratorFunction(1000);

            generator(0, 0).Should().Be(2000);
            generator(9, 0).Should().Be(130000);
        }
    }
}