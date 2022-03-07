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
        public void String_ToCamelCase_Succeeds(string input, string expectedOutput) {
            var output = input.ToCamelCase();

            output.Should().Be(expectedOutput);
        }

        [DataTestMethod]
        [DataRow(null, null, DisplayName = "Null")]
        [DataRow("", "", DisplayName = "Empty string")]
        [DataRow("a", "A", DisplayName = "A")]
        [DataRow("testCase", "TestCase", DisplayName = "Property")]
        [DataRow("test Case", "Test Case", DisplayName = "With space")]
        [DataRow("TestCase", "TestCase", DisplayName = "Already correct")]
        public void String_ToPascalCase_Succeeds(string input, string expectedOutput) {
            var output = input.ToPascalCase();

            output.Should().Be(expectedOutput);
        }
    }
}
