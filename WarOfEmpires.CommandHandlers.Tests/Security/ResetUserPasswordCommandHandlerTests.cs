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
        [TestMethod]
        public void ResetUserPasswordCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.PasswordResetToken.Returns(new TemporaryPassword("reset-token"));

            var handler = new ResetUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ResetUserPasswordCommand("test1@test.com", "reset-token", "test2");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().ResetPassword("test2");
            builder.User.DidNotReceiveWithAnyArgs().ResetPasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_For_Expired_Token() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.PasswordResetToken.Returns(new TemporaryPassword("reset-token"));
            typeof(TemporaryPassword).GetProperty(nameof(TemporaryPassword.ExpiryDate)).SetValue(builder.User.PasswordResetToken, DateTime.UtcNow.AddSeconds(-60));

            var handler = new ResetUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ResetUserPasswordCommand("test1@test.com", "reset-token", "test2");
            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            builder.User.DidNotReceiveWithAnyArgs().ResetPassword(default);
            builder.User.Received().ResetPasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_When_Already_Reset() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.PasswordResetToken.Returns(TemporaryPassword.None);

            var handler = new ResetUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ResetUserPasswordCommand("test1@test.com", "test", "test2");

            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            builder.User.DidNotReceiveWithAnyArgs().ResetPassword(default);
            builder.User.Received().ResetPasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Fails_For_Incorrect_Token() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.PasswordResetToken.Returns(new TemporaryPassword("reset-token"));

            var handler = new ResetUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ResetUserPasswordCommand("test1@test.com", "wrong", "test2");

            var result = handler.Execute(command);

            result.Should().HaveError("The password reset link has expired; please request a new one");
            builder.User.DidNotReceiveWithAnyArgs().ResetPassword(default);
            builder.User.Received().ResetPasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetUserPasswordCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            builder.User.PasswordResetToken.Returns(new TemporaryPassword("reset-token"));

            var handler = new ResetUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ResetUserPasswordCommand("test1@test.com", "test", "test2");

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().ResetPassword(default);
            builder.User.DidNotReceiveWithAnyArgs().ResetPasswordFailed();
        }
    }
}