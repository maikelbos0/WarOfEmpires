using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class ConfirmUserEmailChangeCommandHandlerTests {
        [TestMethod]
        public void ConfirmUserEmailChangeCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.NewEmailConfirmationCode.Returns(999999);

            var handler = new ConfirmUserEmailChangeCommandHandler(new UserRepository(builder.Context));
            var command = new ConfirmUserEmailChangeCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().ChangeEmail();
            builder.User.DidNotReceiveWithAnyArgs().ChangeEmailFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ConfirmUserEmailChangeCommandHandler_Fails_For_Nonexistent_User() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new ConfirmUserEmailChangeCommandHandler(new UserRepository(builder.Context));
            var command = new ConfirmUserEmailChangeCommand("wrong@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError("ConfirmationCode", "This confirmation code is invalid");
        }

        [TestMethod]
        public void ConfirmUserEmailChangeCommandHandler_Fails_For_New_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            var handler = new ConfirmUserEmailChangeCommandHandler(new UserRepository(builder.Context));
            var command = new ConfirmUserEmailChangeCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError("ConfirmationCode", "This confirmation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().ChangeEmail();
            builder.User.Received().ChangeEmailFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ConfirmUserEmailChangeCommandHandler_Fails_For_Inactive_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new ConfirmUserEmailChangeCommandHandler(new UserRepository(builder.Context));
            var command = new ConfirmUserEmailChangeCommand("test1@test.com", "999999");

            var result = handler.Execute(command);

            result.Should().HaveError("ConfirmationCode", "This confirmation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().ChangeEmail();
            builder.User.Received().ChangeEmailFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ConfirmUserEmailChangeCommandHandler_Fails_For_Wrong_Code() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.NewEmailConfirmationCode.Returns(999999);

            var handler = new ConfirmUserEmailChangeCommandHandler(new UserRepository(builder.Context));
            var command = new ConfirmUserEmailChangeCommand("test1@test.com", "999987");

            var result = handler.Execute(command);

            result.Should().HaveError("ConfirmationCode", "This confirmation code is invalid");
            builder.User.DidNotReceiveWithAnyArgs().ChangeEmail();
            builder.User.Received().ChangeEmailFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}