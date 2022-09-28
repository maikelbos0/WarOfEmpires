using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.DataAnnotations;

namespace WarOfEmpires.Tests.DataAnnotations {
    [TestClass]
    public sealed class MaxFileSizeAttributeTests {
        [DataTestMethod]
        [DataRow(10, 9, true)]
        [DataRow(10, 10, true)]
        [DataRow(10, 11, false)]
        public void MaxFileSizeAttribute_IsValid_Succeeds(int maxSize, int fileSize, bool expectedIsValid) {
            var attribute = new MaxFileSizeAttribute(maxSize);
            var value = Substitute.For<IFormFile>();

            value.Length.Returns(fileSize);

            attribute.IsValid(value).Should().Be(expectedIsValid);
        }

        [TestMethod]
        public void MaxFileSizeAttribute_IsValid_Succeeds_For_Not_IFormFile() {
            var attribute = new MaxFileSizeAttribute(10);

            attribute.IsValid("A pretty long string").Should().BeTrue();
        }

        [TestMethod]
        public void MaxFileSizeAttribute_FormatErrorMessage_Succeeds() {
            var attribute = new MaxFileSizeAttribute(10) {
                ErrorMessage = "Test {0} with size {1}"
            };

            attribute.FormatErrorMessage("Property").Should().Be("Test Property with size 10");
        }
    }
}
