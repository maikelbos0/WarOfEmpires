using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Extensions;

namespace WarOfEmpires.Tests.Extensions {
    [TestClass]
    public class StringExtensionsTests {
        [DataTestMethod]
        [DataRow(null, null, DisplayName = "Null")]
        [DataRow("", "", DisplayName = "Empty string")]
        [DataRow("A", "a", DisplayName = "A")]
        [DataRow("TestCase", "testCase", DisplayName = "Property")]
        [DataRow("Test Case", "test Case", DisplayName = "With space")]
        [DataRow("testCase", "testCase", DisplayName = "Already correct")]
        public void ToCamelCase_Succeeds(string input, string expectedOutput) {
            var output = input.ToCamelCase();

            output.Should().Be(expectedOutput);
        }
    }
}
