using WarOfEmpires.Domain.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WarOfEmpires.Domain.Tests.Security {
    [TestClass]
    public sealed class PasswordTests {
        [TestMethod]
        public void Password_Generates_Unique_Salt() {
            var pass1 = new Password("test");
            var pass2 = new Password("test");

            pass1.Salt.SequenceEqual(pass2.Salt).Should().BeFalse();
        }

        [TestMethod]
        public void Passwords_With_Same_Properties_Are_Equal() {
            var pass1 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);
            var pass2 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);

            pass1.Should().Be(pass2);
        }

        [TestMethod]
        public void Passwords_With_Different_Salts_Are_Unequal() {
            var pass1 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);
            var pass2 = CreatePassword(new byte[] { 1, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);

            pass1.Should().NotBe(pass2);
        }

        [TestMethod]
        public void Passwords_With_Different_Hashes_Are_Unequal() {
            var pass1 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);
            var pass2 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 11, 11, 13, 14, 16 }, 1000);

            pass1.Should().NotBe(pass2);
        }

        [TestMethod]
        public void Passwords_With_Different_Iterations_Are_Unequal() {
            var pass1 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 1000);
            var pass2 = CreatePassword(new byte[] { 0, 1, 3, 4, 6 }, new byte[] { 10, 11, 13, 14, 16 }, 2000);

            pass1.Should().NotBe(pass2);
        }

        [TestMethod]
        public void Password_Verify_Correct_Password_Returns_True() {
            var pass = new Password("theRightPass1");

            pass.Verify("theRightPass1").Should().BeTrue();
        }

        [TestMethod]
        public void Password_Verify_Wrong_Password_Returns_False() {
            var pass = new Password("theRightPass1");

            pass.Verify("theWrongPass1").Should().BeFalse();
        }

        private Password CreatePassword(byte[] salt, byte[] hash, int iterations) {
            var password = (Password)Activator.CreateInstance(typeof(Password), true);

            typeof(Password).GetProperty(nameof(Password.Salt)).SetValue(password, salt);
            typeof(Password).GetProperty(nameof(Password.Hash)).SetValue(password, hash);
            typeof(Password).GetProperty(nameof(Password.HashIterations)).SetValue(password, iterations);

            return password;
        }
    }
}