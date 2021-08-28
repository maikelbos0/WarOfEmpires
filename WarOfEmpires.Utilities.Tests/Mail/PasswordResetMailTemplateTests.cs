using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.Utilities.Tests.Mail {
    [TestClass]
    public sealed class PasswordResetMailTemplateTests {
        private readonly AppSettings _appSettings = new AppSettings() {
            ApplicationBaseUrl = "http://localhost"
        };

        [TestMethod]
        public void PasswordResetMailTemplate_Generates_Correct_Subject() {
            var template = new PasswordResetMailTemplate(_appSettings);
            var parameters = new PasswordResetMailTemplateParameters("test@test.com", "asdfasdf");

            template.GetMessage(parameters, "test@test.com").Subject.Should().Be("Your password reset request");
        }

        [TestMethod]
        public void PasswordResetMailTemplate_Generates_Correct_Body() {
            var template = new PasswordResetMailTemplate(_appSettings);
            var parameters = new PasswordResetMailTemplateParameters("test@test.com", "asdfasdf");

            template.GetMessage(parameters, "test@test.com").Body.Should().Be("<p>Please <a href=\"http://localhost/Home/ResetPassword/?email=test%40test.com&token=asdfasdf\">click here to reset your password</a>.</p>");
        }

        [TestMethod]
        public void PasswordResetMailTemplate_Uses_Correct_Address() {
            var template = new PasswordResetMailTemplate(_appSettings);
            var parameters = new PasswordResetMailTemplateParameters("test@test.com", "asdfasdf");

            template.GetMessage(parameters, "test@test.com").To.Should().Be("test@test.com");
        }
    }
}
