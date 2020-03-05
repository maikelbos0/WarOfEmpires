using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Utilities.Tests.Formatting {
    [TestClass]
    public sealed class EnumFormatterTests {
        public enum Test {
            Test = 1,
            TestValue = 2,
            TValue = 3,
            T = 4,
            anotherTest = 5
        }

        [DataTestMethod]
        [DataRow(Test.Test, "Test")]
        [DataRow(Test.TestValue, "Test value")]
        [DataRow(Test.TValue, "T value")]
        [DataRow(Test.T, "T")]
        [DataRow(Test.anotherTest, "Another test")]
        public void EnumFormatter_Succeeds_Capitalized(Test value, string expectedResult) {
            var formatter = new EnumFormatter();

            formatter.ToString(value).Should().Be(expectedResult);
        }

        [DataTestMethod]
        [DataRow(Test.Test, "test")]
        [DataRow(Test.TestValue, "test value")]
        [DataRow(Test.TValue, "t value")]
        [DataRow(Test.T, "t")]
        [DataRow(Test.anotherTest, "another test")]
        public void EnumFormatter_Succeeds_Not_Capitalized(Test value, string expectedResult) {
            var formatter = new EnumFormatter();

            formatter.ToString(value, false).Should().Be(expectedResult);
        }
    }
}