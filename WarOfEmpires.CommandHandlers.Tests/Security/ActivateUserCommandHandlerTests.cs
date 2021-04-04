using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ActivateUserCommandHandlerTests {
        [TestMethod]
        public void ActivateUserCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            builder.User.ActivationCode.Returns(999999);

            var handler = new ActivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new ActivateUserCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().Activate();
            builder.User.DidNotReceiveWithAnyArgs().ActivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ActivateUserCommandHandler_Fails_For_Nonexistent_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            builder.User.ActivationCode.Returns(999999);

            var handler = new ActivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new ActivateUserCommand("wrong@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.ActivationCode, "This activation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().Activate();
            builder.User.DidNotReceiveWithAnyArgs().ActivationFailed();
        }

        [TestMethod]
        public void ActivateUserCommandHandler_Fails_For_Active_User() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.ActivationCode.Returns(999999);

            var handler = new ActivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new ActivateUserCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.ActivationCode, "This activation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().Activate();
            builder.User.Received().ActivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ActivateUserCommandHandler_Fails_For_Inactive_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            builder.User.ActivationCode.Returns(999999);

            var handler = new ActivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new ActivateUserCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.ActivationCode, "This activation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().Activate();
            builder.User.Received().ActivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ActivateUserCommandHandler_Fails_For_Wrong_Code() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            builder.User.ActivationCode.Returns(999999);

            var handler = new ActivateUserCommandHandler(new UserRepository(builder.Context));
            var command = new ActivateUserCommand("test1@test.com", "999987");

            var result = handler.Execute(command);
            
            result.Should().HaveError(c => c.ActivationCode, "This activation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().Activate();
            builder.User.Received().ActivationFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}