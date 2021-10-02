using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Models.DataAnnotations;

namespace WarOfEmpires.Models.Tests.DataAnnotations {
    [TestClass]
    public sealed class FileExtensionAttributeTests {
        [DataTestMethod]
        [DataRow(new[] { ".jpg" }, "test.jpg", true)]
        [DataRow(new[] { ".jpeg", ".jpg", ".gif" }, "test.jpg", true)]
        [DataRow(new[] { ".jpeg", ".jpg", ".gif" }, "test.png", false)]
        public void FileExtensionAttribute_IsValid_Succeeds(string[] extensions, string fileName, bool expectedIsValid) {
            var attribute = new FileExtensionAttribute(extensions);
            var value = Substitute.For<IFormFile>();

            value.FileName.Returns(fileName);

            attribute.IsValid(value).Should().Be(expectedIsValid);
        }

        [TestMethod]
        public void FileExtensionAttributeTests_IsValid_Succeeds_For_Not_IFormFile() {
            var attribute = new FileExtensionAttribute(".jpg");

            attribute.IsValid("A pretty long string").Should().BeTrue();
        }

        [TestMethod]
        public void FileExtensionAttributeTests_FormatErrorMessage_Succeeds() {
            var attribute = new FileExtensionAttribute(".jpg", ".jpeg") {
                ErrorMessage = "Test {0} with extensions {1}"
            };

            attribute.FormatErrorMessage("Property").Should().Be("Test Property with extensions .jpg, .jpeg");
        }
    }
}
