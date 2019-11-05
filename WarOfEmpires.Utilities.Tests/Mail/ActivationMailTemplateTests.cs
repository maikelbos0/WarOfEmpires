using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Mail;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WarOfEmpires.Utilities.Tests.Mail {
    [TestClass]
    public sealed class ActivationMailTemplateTests {
        private readonly FakeAppSettings _appSettings = new FakeAppSettings() {
            Settings = new Dictionary<string, string>() {
                { "Application.BaseUrl", "http://localhost/" }
            }
        };

        [TestMethod]
        public void ActivationMailTemplate_Generates_Correct_Subject() {
            var template = new ActivationMailTemplate(_appSettings);
            var parameters = new ActivationMailTemplateParameters("test@test.com", 12345);

            template.GetMessage(parameters, "test@test.com").Subject.Should().Be("Please activate your account");
        }

        [TestMethod]
        public void ActivationMailTemplate_Generates_Correct_Body() {
            var template = new ActivationMailTemplate(_appSettings);
            var parameters = new ActivationMailTemplateParameters("test@test.com", 12345);

            template.GetMessage(parameters, "test@test.com").Body.Should().Be("<p>Please <a href=\"http://localhost/Home/Activate/?activationCode=12345&email=test%40test.com\">click here to activate your account</a>.</p>");
        }

        [TestMethod]
        public void ActivationMailTemplate_Uses_Correct_Address() {
            var template = new ActivationMailTemplate(_appSettings);
            var parameters = new ActivationMailTemplateParameters("test@test.com", 12345);

            template.GetMessage(parameters, "test@test.com").To.Should().Be("test@test.com");
        }
    }
}