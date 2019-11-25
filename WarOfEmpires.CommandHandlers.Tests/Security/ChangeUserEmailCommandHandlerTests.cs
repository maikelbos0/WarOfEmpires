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
using System.Collections.Generic;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ChangeUserEmailCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;
        private readonly FakeMailClient _mailClient = new FakeMailClient();
        private readonly FakeAppSettings _appSettings = new FakeAppSettings() {
            Settings = new Dictionary<string, string>() {
                { "Application.BaseUrl", "http://localhost" }
            }
        };

        public ChangeUserEmailCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Succeeds() {
            var handler = new ChangeUserEmailCommandHandler(_repository, _mailClient, new ConfirmEmailMailTemplate(_appSettings));
            var command = new ChangeUserEmailCommand("test@test.com", "test", "new@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.NewEmailConfirmationCode.Returns(999999);
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            handler.Execute(command);

            user.Received().RequestEmailChange("new@test.com");
            user.DidNotReceive().RequestEmailChangeFailed();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Sends_Email() {
            var handler = new ChangeUserEmailCommandHandler(_repository, _mailClient, new ConfirmEmailMailTemplate(_appSettings));
            var command = new ChangeUserEmailCommand("test@test.com", "test", "new@test.com");
            var user = new User("test@test.com", "test");

            user.Activate();
            _context.Users.Add(user);

            handler.Execute(command);

            _mailClient.SentMessages.Should().HaveCount(1);
            _mailClient.SentMessages[0].Subject.Should().Be("Please confirm your email address");
            _mailClient.SentMessages[0].To.Should().Be("new@test.com");
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Fails_For_Wrong_Password() {
            var handler = new ChangeUserEmailCommandHandler(_repository, _mailClient, new ConfirmEmailMailTemplate(_appSettings));
            var command = new ChangeUserEmailCommand("test@test.com", "wrong", "new@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Password");
            result.Errors[0].Message.Should().Be("Invalid password");
            user.DidNotReceive().RequestEmailChange(Arg.Any<string>());
            user.Received().RequestEmailChangeFailed();
            _mailClient.SentMessages.Should().BeEmpty();
        }

        [TestMethod]
        public void ChangeUserEmailCommandHandler_Fails_For_Existing_New_Email() {
            var handler = new ChangeUserEmailCommandHandler(_repository, _mailClient, new ConfirmEmailMailTemplate(_appSettings));
            var command = new ChangeUserEmailCommand("test@test.com", "test", "new@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var user2 = Substitute.For<User>();
            user2.Email.Returns("new@test.com");

            _context.Users.Add(user2);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.NewEmail");
            result.Errors[0].Message.Should().Be("Email address already exists");
            user.DidNotReceive().RequestEmailChange(Arg.Any<string>());
            user.Received().RequestEmailChangeFailed();
            _mailClient.SentMessages.Should().BeEmpty();
        }
        
        [TestMethod]
        public void ChangeUserEmailCommandHandler_Throws_Exception_For_Invalid_User() {
            var handler = new ChangeUserEmailCommandHandler(_repository, _mailClient, new ConfirmEmailMailTemplate(_appSettings));
            var command = new ChangeUserEmailCommand("test@test.com", "test", "new@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Inactive);

            _context.Users.Add(user);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();

            _mailClient.SentMessages.Should().BeEmpty();
        }
    }
}