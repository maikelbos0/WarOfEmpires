using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Mail;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class SendUserActivationCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;
        private readonly FakeMailClient _mailClient = new FakeMailClient();
        private readonly FakeAppSettings _appSettings = new FakeAppSettings() {
            Settings = new Dictionary<string, string>() {
                { "Application.BaseUrl", "http://localhost" }
            }
        };

        public SendUserActivationCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Sends_Email() {
            var handler = new SendUserActivationCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new SendUserActivationCommand("test@test.com");

            _context.Users.Add(new User("test@test.com", "test"));

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _mailClient.SentMessages.Should().HaveCount(1);
            _mailClient.SentMessages[0].Subject.Should().Be("Please activate your account");
            _mailClient.SentMessages[0].To.Should().Be("test@test.com");
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Succeeds() {
            var handler = new SendUserActivationCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new SendUserActivationCommand("test@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.ActivationCode.Returns(999999);
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.New);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.Received().GenerateActivationCode();
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Active_User() {
            var handler = new SendUserActivationCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new SendUserActivationCommand("other@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.DidNotReceive().GenerateActivationCode();
            _mailClient.SentMessages.Should().BeEmpty();
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Inactive_User() {
            var handler = new SendUserActivationCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new SendUserActivationCommand("other@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Inactive);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.DidNotReceive().GenerateActivationCode();
            _mailClient.SentMessages.Should().BeEmpty();
        }

        [TestMethod]
        public void SendUserActivationCommandHandler_Does_Nothing_For_Nonexistent_User() {
            var handler = new SendUserActivationCommandHandler(_repository, _mailClient, new ActivationMailTemplate(_appSettings));
            var command = new SendUserActivationCommand("other@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _mailClient.SentMessages.Should().BeEmpty();
        }
    }
}