using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.CommandHandlers.Players;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class RegisterPlayerCommandHandlerTests {
        [TestMethod]
        public void RegisterPlayerCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new RegisterPlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context));
            var command = new RegisterPlayerCommand("test1@test.com", "My name");
            
            var result = handler.Execute(command);
            var player = builder.Context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.DisplayName.Should().Be("My name");
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}