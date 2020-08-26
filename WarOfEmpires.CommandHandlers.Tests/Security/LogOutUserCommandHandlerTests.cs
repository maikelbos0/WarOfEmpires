using WarOfEmpires.CommandHandlers.Security;
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
        [TestMethod]
        public void LogOutUserCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new LogOutUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogOutUserCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().LogOut();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void LogOutUserCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new LogOutUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogOutUserCommand("wrong@test.com");

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().LogOut();
        }
    }
}