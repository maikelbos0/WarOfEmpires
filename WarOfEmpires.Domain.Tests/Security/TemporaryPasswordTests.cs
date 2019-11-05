using WarOfEmpires.Domain.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WarOfEmpires.Domain.Tests.Security {
    [TestClass]
    public sealed class TemporaryPasswordTests {
        [TestMethod]
        public void TemporaryPassword_Verify_Valid_Password_Returns_True() {
            var pass = new TemporaryPassword("test");

            pass.Verify("test").Should().BeTrue();
        }

        [TestMethod]
        public void TemporaryPassword_Verify_Expired_Password_Returns_False() {
            var pass = new TemporaryPassword("test");
            typeof(TemporaryPassword).GetProperty(nameof(TemporaryPassword.ExpiryDate)).SetValue(pass, DateTime.UtcNow.AddSeconds(-60));

            pass.Verify("test").Should().BeFalse();
        }

        [TestMethod]
        public void TemporaryPassword_Verify_Wrong_Password_Returns_False() {
            var pass = new TemporaryPassword("test");

            pass.Verify("wrong").Should().BeFalse();
        }

        [TestMethod]
        public void TemporaryPassword_Verify_Expired_Wrong_Password_Returns_False() {
            var pass = new TemporaryPassword("test");
            typeof(TemporaryPassword).GetProperty(nameof(TemporaryPassword.ExpiryDate)).SetValue(pass, DateTime.UtcNow.AddSeconds(-60));

            pass.Verify("wrong").Should().BeFalse();
        }
    }
}