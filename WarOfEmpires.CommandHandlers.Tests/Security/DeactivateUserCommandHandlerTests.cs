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
        [TestMethod]
        public void DeactivateUserCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().Deactivate();
            builder.User.DidNotReceiveWithAnyArgs().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Fails_For_Wrong_Password() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "wrong");

            var result = handler.Execute(command);

            result.Should().HaveError("Password", "Invalid password");
            builder.User.DidNotReceiveWithAnyArgs().Deactivate();
            builder.User.Received().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "test");

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().Deactivate();
            builder.User.DidNotReceiveWithAnyArgs().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}