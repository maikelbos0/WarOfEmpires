using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Repositories.Security;
using System.Linq;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class RotateUserRefreshTokenCommandHandlerTests {
        [TestMethod]
        public void RotateUserRefreshTokenCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1)
                .WithRefreshTokenFamily(1, Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            builder.User.RotateRefreshToken(Arg.Any<Guid>(), Arg.Any<byte[]>()).Returns(true);

            var handler = new RotateUserRefreshTokenCommandHandler(new UserRepository(builder.Context));
            var command = new RotateUserRefreshTokenCommand("test1@test.com", Guid.NewGuid(), Convert.ToBase64String(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }));

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().RotateRefreshToken(command.RequestId, Arg.Is<byte[]>(t => t.SequenceEqual(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 })));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void RotateUserRefreshTokenCommandHandler_Fails_For_Invalid_Token() {
            var builder = new FakeBuilder()
                .BuildUser(1)
                .WithRefreshTokenFamily(1, Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            builder.User.RotateRefreshToken(Arg.Any<Guid>(), Arg.Any<byte[]>()).Returns(false);

            var handler = new RotateUserRefreshTokenCommandHandler(new UserRepository(builder.Context));
            var command = new RotateUserRefreshTokenCommand("test1@test.com", Guid.NewGuid(), Convert.ToBase64String(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }));

            var result = handler.Execute(command);

            result.Should().HaveError("Invalid refresh token");
            builder.User.Received().RotateRefreshToken(command.RequestId, Arg.Is<byte[]>(t => t.SequenceEqual(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 })));
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void RotateUserRefreshTokenCommandHandler_Throws_Exception_For_Invalid_User() {
            var builder = new FakeBuilder()
                .BuildUser(1)
                .WithRefreshTokenFamily(1, Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            var handler = new RotateUserRefreshTokenCommandHandler(new UserRepository(builder.Context));
            var command = new RotateUserRefreshTokenCommand("wrong@test.com", Guid.NewGuid(), Convert.ToBase64String(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }));

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.User.DidNotReceiveWithAnyArgs().RotateRefreshToken(default, default);
        }
    }
}