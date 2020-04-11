using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ResetUserPasswordCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;

        public ResetUserPasswordCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Succeeds() {
            var handler = new ResetUserPasswordCommandHandler(_repository);
            var command = new ResetUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            user.Password.Returns(new Password("test"));
            user.PasswordResetToken.Returns(new TemporaryPassword("test"));

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.Received().ResetPassword("test2");
            user.DidNotReceive().ResetPasswordFailed();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_For_Expired_Token() {
            var handler = new ResetUserPasswordCommandHandler(_repository);
            var command = new ResetUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            user.Password.Returns(new Password("test"));
            var token = new TemporaryPassword("test");
            typeof(TemporaryPassword).GetProperty(nameof(TemporaryPassword.ExpiryDate)).SetValue(token, DateTime.UtcNow.AddSeconds(-60));
            user.PasswordResetToken.Returns(token);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            user.DidNotReceive().ResetPassword(Arg.Any<string>());
            user.Received().ResetPasswordFailed();
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_When_Already_Reset() {
            var handler = new ResetUserPasswordCommandHandler(_repository);
            var command = new ResetUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            user.Password.Returns(new Password("test"));
            user.PasswordResetToken.Returns(TemporaryPassword.None);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            user.DidNotReceive().ResetPassword(Arg.Any<string>());
            user.Received().ResetPasswordFailed();
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_For_Incorrect_Token() {
            var handler = new ResetUserPasswordCommandHandler(_repository);
            var command = new ResetUserPasswordCommand("test@test.com", "wrong", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            user.Password.Returns(new Password("test"));
            user.PasswordResetToken.Returns(new TemporaryPassword("test"));

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            user.DidNotReceive().ResetPassword(Arg.Any<string>());
            user.Received().ResetPasswordFailed();
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Throws_Exception_For_Invalid_User() {
            var handler = new ResetUserPasswordCommandHandler(_repository);
            var command = new ResetUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Inactive);
            user.Password.Returns(new Password("test"));
            user.PasswordResetToken.Returns(new TemporaryPassword("test"));

            _context.Users.Add(user);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            user.DidNotReceive().ResetPassword(Arg.Any<string>());
            user.DidNotReceive().ResetPasswordFailed();
        }
    }
}