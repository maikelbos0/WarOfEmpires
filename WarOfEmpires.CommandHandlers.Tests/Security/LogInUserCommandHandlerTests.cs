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
    public sealed class LogInUserCommandHandlerTests {
        [TestMethod]
        public void LogInUserCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new LogInUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogInUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().LogIn();
            builder.User.DidNotReceiveWithAnyArgs().LogInFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void LogInUserCommandHandler_Fails_For_Nonexistent_User() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new LogInUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogInUserCommand("wrong@test.com", "test");

            var result = handler.Execute(command);

            result.Should().HaveError("Invalid email or password");
            builder.User.DidNotReceiveWithAnyArgs().LogIn();
            builder.User.DidNotReceiveWithAnyArgs().LogInFailed();
        }

        [TestMethod]
        public void LogInUserCommandHandler_Fails_For_Inactive_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.Inactive);

            var handler = new LogInUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogInUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Should().HaveError("Invalid email or password");
            builder.User.DidNotReceiveWithAnyArgs().LogIn();
            builder.User.Received().LogInFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void LogInUserCommandHandler_Fails_For_New_User() {
            var builder = new FakeBuilder()
                .BuildUser(1, status: UserStatus.New);

            var handler = new LogInUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogInUserCommand("test1@test.com", "test");

            var result = handler.Execute(command);

            result.Should().HaveError("Invalid email or password");
            builder.User.DidNotReceiveWithAnyArgs().LogIn();
            builder.User.Received().LogInFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void LogInUserCommandHandler_Fails_For_Invalid_Password() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new LogInUserCommandHandler(new UserRepository(builder.Context));
            var command = new LogInUserCommand("test1@test.com", "wrong");
            
            var result = handler.Execute(command);

            result.Should().HaveError("Invalid email or password");
            builder.User.DidNotReceiveWithAnyArgs().LogIn();
            builder.User.Received().LogInFailed();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}