using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class DeactivateUserCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;

        public DeactivateUserCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Succeeds() {
            var handler = new DeactivateUserCommandHandler(_repository);
            var command = new DeactivateUserCommand("test@test.com", "test");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Errors.Should().BeEmpty();
            user.Received().Deactivate();
            user.DidNotReceive().DeactivationFailed();
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Fails_For_Wrong_Password() {
            var handler = new DeactivateUserCommandHandler(_repository);
            var command = new DeactivateUserCommand("test@test.com", "wrong");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("p => p.Password");
            result.Errors[0].Message.Should().Be("Invalid password");
            user.DidNotReceive().Deactivate();
            user.Received().DeactivationFailed();
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Throws_Exception_For_Invalid_User() {
            var handler = new DeactivateUserCommandHandler(_repository);
            var command = new DeactivateUserCommand("test@test.com", "test");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Inactive);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            _context.Users.Add(user);

            commandAction.Should().Throw<InvalidOperationException>();
            user.DidNotReceive().Deactivate();
            user.DidNotReceive().DeactivationFailed();
        }
    }
}