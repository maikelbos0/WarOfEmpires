using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Mail;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ChangeUserEmailCommandHandlerTests {
        [TestMethod]
        public void ChangeUserEmailCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.NewEmailConfirmationCode.Returns(999999);

            var handler = new ChangeUserEmailCommandHandler(new UserRepository(builder.Context), new FakeMailClient(), new ConfirmEmailMailTemplate(new FakeAppSettings()));
            var command = new ChangeUserEmailCommand("test1@test.com", "test", "new@test.com");
            
            handler.Execute(command);

            builder.User.Received().RequestEmailChange("new@test.com");
            builder.User.DidNotReceiveWithAnyArgs().RequestEmailChangeFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Sends_Email() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.NewEmailConfirmationCode.Returns(999999);
            builder.User.NewEmail.Returns("new@test.com");

            var handler = new ChangeUserEmailCommandHandler(new UserRepository(builder.Context), mailClient, new ConfirmEmailMailTemplate(new FakeAppSettings()));
            var command = new ChangeUserEmailCommand("test1@test.com", "test", "new@test.com");

            handler.Execute(command);

            mailClient.SentMessages.Should().HaveCount(1);
            mailClient.SentMessages[0].Subject.Should().Be("Please confirm your email address");
            mailClient.SentMessages[0].To.Should().Be("new@test.com");
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Fails_For_Wrong_Password() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ChangeUserEmailCommandHandler(new UserRepository(builder.Context), mailClient, new ConfirmEmailMailTemplate(new FakeAppSettings()));
            var command = new ChangeUserEmailCommand("test1@test.com", "wrong", "new@test.com");

            var result = handler.Execute(command);

            result.Should().HaveError("Password", "Invalid password");
            builder.User.DidNotReceiveWithAnyArgs().RequestEmailChange(default);
            builder.User.Received().RequestEmailChangeFailed();
            mailClient.SentMessages.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Fails_For_Existing_New_Email() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .WithUser(2, email: "new@test.com")
                .BuildUser(1);

            var handler = new ChangeUserEmailCommandHandler(new UserRepository(builder.Context), mailClient, new ConfirmEmailMailTemplate(new FakeAppSettings()));
            var command = new ChangeUserEmailCommand("test1@test.com", "test", "new@test.com");

            var result = handler.Execute(command);

            result.Should().HaveError("NewEmail", "Email address already exists");
            builder.User.DidNotReceiveWithAnyArgs().RequestEmailChange(default);
            builder.User.Received().RequestEmailChangeFailed();
            mailClient.SentMessages.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void ChangeUserEmailCommandHandler_Throws_Exception_For_Invalid_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new ChangeUserEmailCommandHandler(new UserRepository(builder.Context), mailClient, new ConfirmEmailMailTemplate(new FakeAppSettings()));
            var command = new ChangeUserEmailCommand("test1@test.com", "test", "new@test.com");

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().RequestEmailChange(default);
            builder.User.DidNotReceiveWithAnyArgs().RequestEmailChangeFailed();
            mailClient.SentMessages.Should().BeEmpty();
        }
    }
}