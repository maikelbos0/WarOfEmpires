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
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;
        private readonly FakeMailClient _mailClient = new FakeMailClient();
        private readonly FakeAppSettings _appSettings = new FakeAppSettings();

        public RegisterUserCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void RegisterUserCommandHandler_Succeeds() {
            var handler = new RegisterUserCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new RegisterUserCommand("test@test.com", "test");

            var result = handler.Execute(command);
            var user = _context.Users.SingleOrDefault();

            result.Success.Should().BeTrue();
            user.Should().NotBeNull();
            user.Status.Should().Be(UserStatus.New);
            user.UserEvents.Should().HaveCount(1);
            user.UserEvents.Single().Type.Should().Be(UserEventType.Registered);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void RegisterUserCommandHandler_Sends_Email() {
            var handler = new RegisterUserCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new RegisterUserCommand("test@test.com", "test");

            handler.Execute(command);

            _mailClient.SentMessages.Should().HaveCount(1);
            _mailClient.SentMessages[0].Subject.Should().Be("Please activate your account");
            _mailClient.SentMessages[0].To.Should().Be("test@test.com");
        }

        [TestMethod]
        public void RegisterUserCommandHandler_Fails_For_Existing_Email() {
            var handler = new RegisterUserCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new RegisterUserCommand("test@test.com", "test");

            _context.Users.Add(new User("test@test.com", "test"));

            var result = handler.Execute(command);

            result.Should().HaveError("Email", "Email address already exists");
            _mailClient.SentMessages.Should().BeEmpty();
        }
    }
}