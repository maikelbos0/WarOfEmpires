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
    public sealed class ChangeUserPasswordCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;

        public ChangeUserPasswordCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Succeeds() {
            var handler = new ChangeUserPasswordCommandHandler(_repository);
            var command = new ChangeUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.Received().ChangePassword("test2");
            user.DidNotReceive().ChangePasswordFailed();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Throws_Exception_For_Invalid_User() {
            var handler = new ChangeUserPasswordCommandHandler(_repository);
            var command = new ChangeUserPasswordCommand("test@test.com", "test", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Inactive);

            _context.Users.Add(user);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            commandAction.Should().Throw<InvalidOperationException>();
            user.DidNotReceive().ChangePassword(Arg.Any<string>());
            user.DidNotReceive().ChangePasswordFailed();
        }

        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Fails_For_Wrong_Password() {
            var handler = new ChangeUserPasswordCommandHandler(_repository);
            var command = new ChangeUserPasswordCommand("test@test.com", "wrong", "test2");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Should().HaveError("CurrentPassword", "Invalid password");
            user.DidNotReceive().ChangePassword(Arg.Any<string>());
            user.Received().ChangePasswordFailed();
        }
    }
}