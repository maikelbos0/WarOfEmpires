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
        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ChangeUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ChangeUserPasswordCommand("test1@test.com", "test", "test2");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().ChangePassword("test2");
            builder.User.DidNotReceive().ChangePasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new ChangeUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ChangeUserPasswordCommand("test1@test.com", "test", "test2");
            
            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceive().ChangePassword(Arg.Any<string>());
            builder.User.DidNotReceive().ChangePasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void ChangeUserPasswordCommandHandler_Fails_For_Wrong_Password() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ChangeUserPasswordCommandHandler(new UserRepository(builder.Context));
            var command = new ChangeUserPasswordCommand("test1@test.com", "wrong", "test2");

            var result = handler.Execute(command);

            result.Should().HaveError("CurrentPassword", "Invalid password");
            builder.User.DidNotReceive().ChangePassword(Arg.Any<string>());
            builder.User.Received().ChangePasswordFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}