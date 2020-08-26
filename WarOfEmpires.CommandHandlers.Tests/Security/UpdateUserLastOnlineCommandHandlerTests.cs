using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class UpdateUserLastOnlineCommandHandlerTests {        
        [TestMethod]
        public void UpdateUserLastOnlineCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new UpdateUserLastOnlineCommandHandler(new UserRepository(builder.Context));
            var command = new UpdateUserLastOnlineCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.User.Received().WasOnline();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}