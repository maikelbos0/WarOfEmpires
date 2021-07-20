using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class DeactivateUserCommandHandlerTests {
        [TestMethod]
        public void DeactivateUserCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
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
                .BuildPlayer(1);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "wrong");

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Password, "Invalid password");
            builder.User.DidNotReceiveWithAnyArgs().Deactivate();
            builder.User.Received().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Fails_For_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Should().HaveError("You can't deactivate your account while you are leading your alliance; transfer leadership or dissolve your alliance");
            builder.User.DidNotReceiveWithAnyArgs().Deactivate();
            builder.User.Received().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DeactivateUserCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, status: UserStatus.Inactive);

            var handler = new DeactivateUserCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new DeactivateUserCommand("test1@test.com", "test");

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().Deactivate();
            builder.User.DidNotReceiveWithAnyArgs().DeactivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}