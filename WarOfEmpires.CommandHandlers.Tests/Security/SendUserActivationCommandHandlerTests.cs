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
    public sealed class SendUserActivationCommandHandlerTests {
        [TestMethod]
        public void SendUserActivationCommandHandler_Sends_Email() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            builder.User.ActivationCode.Returns(999999);

            var handler = new SendUserActivationCommandHandler(new UserRepository(builder.Context), mailClient, new ActivationMailTemplate(new AppSettings()));
            var command = new SendUserActivationCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            mailClient.SentMessages.Should().HaveCount(1);
            mailClient.SentMessages[0].Subject.Should().Be("Please activate your account");
            mailClient.SentMessages[0].To.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            builder.User.ActivationCode.Returns(999999);

            var handler = new SendUserActivationCommandHandler(new UserRepository(builder.Context), new FakeMailClient(), new ActivationMailTemplate(new AppSettings()));
            var command = new SendUserActivationCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().GenerateActivationCode();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Active_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new SendUserActivationCommandHandler(new UserRepository(builder.Context), mailClient, new ActivationMailTemplate(new AppSettings()));
            var command = new SendUserActivationCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.DidNotReceiveWithAnyArgs().GenerateActivationCode();
            mailClient.SentMessages.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Inactive_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new SendUserActivationCommandHandler(new UserRepository(builder.Context), mailClient, new ActivationMailTemplate(new AppSettings()));
            var command = new SendUserActivationCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.DidNotReceiveWithAnyArgs().GenerateActivationCode();
            mailClient.SentMessages.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Nonexistent_User() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            var handler = new SendUserActivationCommandHandler(new UserRepository(builder.Context), mailClient, new ActivationMailTemplate(new AppSettings()));
            var command = new SendUserActivationCommand("wrong@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.DidNotReceiveWithAnyArgs().GenerateActivationCode();
            mailClient.SentMessages.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}