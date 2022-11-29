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
    public sealed class GenerateUserRefreshTokenCommandHandlerTests {
        [TestMethod]
        public void GenerateUserRefreshTokenCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new GenerateUserRefreshTokenCommandHandler(new UserRepository(builder.Context));
            var command = new GenerateUserRefreshTokenCommand("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().GenerateRefreshToken(command.RequestId);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void GenerateUserRefreshTokenCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new GenerateUserRefreshTokenCommandHandler(new UserRepository(builder.Context));
            var command = new GenerateUserRefreshTokenCommand("wrong@test.com", Guid.NewGuid());

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().GenerateRefreshToken(default);
        }
    }
}