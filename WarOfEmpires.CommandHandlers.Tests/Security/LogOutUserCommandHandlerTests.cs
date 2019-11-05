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
    public sealed class LogOutUserCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;

        public LogOutUserCommandHandlerTests() {
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public void LogOutUserCommandHandler_Succeeds() {
            var handler = new LogOutUserCommandHandler(_repository);
            var command = new LogOutUserCommand("test@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            user.Received().LogOut();
        }

        [TestMethod]
        public void LogOutUserCommandHandler_Throws_Exception_For_Invalid_User() {
            var handler = new LogOutUserCommandHandler(_repository);
            var command = new LogOutUserCommand("test@test.com");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Inactive);

            Action commandAction = () => {
                var result = handler.Execute(command);
            };

            _context.Users.Add(user);

            commandAction.Should().Throw<InvalidOperationException>();
            user.DidNotReceive().LogOut();
        }
    }
}