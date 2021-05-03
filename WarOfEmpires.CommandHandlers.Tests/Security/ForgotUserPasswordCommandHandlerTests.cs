using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ForgotUserPasswordCommandHandlerTests {
        [TestMethod]
        public void ForgotUserPasswordCommandHandler_Sends_Email() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ForgotUserPasswordCommandHandler(new UserRepository(builder.Context), mailClient, new PasswordResetMailTemplate(new AppSettings()));
            var command = new ForgotUserPasswordCommand("test1@test.com");
            
            handler.Execute(command);

            mailClient.SentMessages.Should().HaveCount(1);
            mailClient.SentMessages[0].Subject.Should().Be("Your password reset request");
            mailClient.SentMessages[0].To.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void ForgotUserPasswordCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ForgotUserPasswordCommandHandler(new UserRepository(builder.Context), new FakeMailClient(), new PasswordResetMailTemplate(new AppSettings()));
            var command = new ForgotUserPasswordCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().GeneratePasswordResetToken();
            builder.User.DidNotReceiveWithAnyArgs().PasswordResetRequestFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ForgotUserPasswordCommandHandler_Does_Nothing_For_Nonexistent_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ForgotUserPasswordCommandHandler(new UserRepository(builder.Context), mailClient, new PasswordResetMailTemplate(new AppSettings()));
            var command = new ForgotUserPasswordCommand("wrong@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            mailClient.SentMessages.Should().BeEmpty();
            builder.User.DidNotReceiveWithAnyArgs().GeneratePasswordResetToken();
            builder.User.DidNotReceiveWithAnyArgs().PasswordResetRequestFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ForgotUserPasswordCommandHandler_Does_Nothing_For_Inactive_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new ForgotUserPasswordCommandHandler(new UserRepository(builder.Context), mailClient, new PasswordResetMailTemplate(new AppSettings()));
            var command = new ForgotUserPasswordCommand("test1@test.com");
            
            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            mailClient.SentMessages.Should().BeEmpty();
            builder.User.DidNotReceiveWithAnyArgs().GeneratePasswordResetToken();
            builder.User.Received().PasswordResetRequestFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ForgotUserPasswordCommandHandler_Does_Nothing_For_New_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            var handler = new ForgotUserPasswordCommandHandler(new UserRepository(builder.Context), mailClient, new PasswordResetMailTemplate(new AppSettings()));
            var command = new ForgotUserPasswordCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            mailClient.SentMessages.Should().BeEmpty();
            builder.User.DidNotReceiveWithAnyArgs().GeneratePasswordResetToken();
            builder.User.Received().PasswordResetRequestFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}