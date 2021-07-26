using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class UpdateUserDetailsCommandHandlerTests {
        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "new@test.com")
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new UpdateUserDetailsCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new UpdateUserDetailsCommand(1, "new@test.com", "display", "code", "name", "Active", true);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Alliance.Received().Update("code", "name");
            builder.Player.Received().Update("display");
            builder.User.Received().Update("new@test.com", UserStatus.Active, true);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [DataTestMethod]
        [DataRow("New", UserStatus.New, DisplayName = "New")]
        [DataRow("Active", UserStatus.Active, DisplayName = "Active")]
        [DataRow("Inactive", UserStatus.Inactive, DisplayName = "Inactive")]
        public void UpdateUserDetailsCommandHandler_Resolves_Status_To_Correct_Status(string status, UserStatus expectedStatus) {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "new@test.com")
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new UpdateUserDetailsCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new UpdateUserDetailsCommand(1, "new@test.com", "display", "code", "name", status, false);

            handler.Execute(command);

            builder.User.Received().Update("new@test.com", expectedStatus, false);
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Fails_For_Existing_Email() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "new@test.com")
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new UpdateUserDetailsCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new UpdateUserDetailsCommand(1, "new@test.com", "display", "code", "name", "Active", false);

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Email, "Email address already exists");
            builder.Alliance.DidNotReceiveWithAnyArgs().Update(default, default);
            builder.Player.DidNotReceiveWithAnyArgs().Update(default);
            builder.User.DidNotReceiveWithAnyArgs().Update(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new UpdateUserDetailsCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new UpdateUserDetailsCommand(2, "new@test.com", "display", "code", "name", "Wrong", false);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Update(default, default);
            builder.Player.DidNotReceiveWithAnyArgs().Update(default);
            builder.User.DidNotReceiveWithAnyArgs().Update(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Throws_Exception_For_Nonexistent_Status() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new UpdateUserDetailsCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new UpdateUserDetailsCommand(1, "new@test.com", "display", "code", "name", "Wrong", false);

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Alliance.DidNotReceiveWithAnyArgs().Update(default, default);
            builder.Player.DidNotReceiveWithAnyArgs().Update(default);
            builder.User.DidNotReceiveWithAnyArgs().Update(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
