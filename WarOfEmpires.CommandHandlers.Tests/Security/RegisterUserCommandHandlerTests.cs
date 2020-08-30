using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Mail;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class RegisterUserCommandHandlerTests {
        [TestMethod]
        public void RegisterUserCommandHandler_Succeeds() {
            var context = new FakeWarContext();

            var handler = new RegisterUserCommandHandler(new UserRepository(context), new FakeMailClient(), new ActivationMailTemplate(new FakeAppSettings()));
            var command = new RegisterUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);
            var user = context.Users.SingleOrDefault();

            result.Success.Should().BeTrue();
            user.Should().NotBeNull();
            user.Status.Should().Be(UserStatus.New);
            user.UserEvents.Should().HaveCount(1);
            user.UserEvents.Single().Type.Should().Be(UserEventType.Registered);
            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void RegisterUserCommandHandler_Sends_Email() {
            var mailClient = new FakeMailClient();
            
            var handler = new RegisterUserCommandHandler(new UserRepository(new FakeWarContext()), mailClient, new ActivationMailTemplate(new FakeAppSettings()));
            var command = new RegisterUserCommand("test1@test.com", "test");

            handler.Execute(command);

            mailClient.SentMessages.Should().HaveCount(1);
            mailClient.SentMessages[0].Subject.Should().Be("Please activate your account");
            mailClient.SentMessages[0].To.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void RegisterUserCommandHandler_Fails_For_Existing_Email() {
            var mailClient = new FakeMailClient();
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new RegisterUserCommandHandler(new UserRepository(builder.Context), mailClient, new ActivationMailTemplate(new FakeAppSettings()));
            var command = new RegisterUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Should().HaveError("Email", "Email address already exists");
            builder.Context.Users.Should().HaveCount(1);
            mailClient.SentMessages.Should().BeEmpty();
        }
    }
}