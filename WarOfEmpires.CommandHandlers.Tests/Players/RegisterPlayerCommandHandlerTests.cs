using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.CommandHandlers.Players;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class RegisterPlayerCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _userRepository;
        private readonly PlayerRepository _repository;

        public RegisterPlayerCommandHandlerTests() {
            _userRepository = new UserRepository(_context);
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void RegisterPlayerCommandHandler_Succeeds() {
            var handler = new RegisterPlayerCommandHandler(_userRepository, _repository);
            var command = new RegisterPlayerCommand("test@test.com", "My name");
            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Password.Returns(new Password("test"));
            user.Status.Returns(UserStatus.Active);

            _context.Users.Add(user);

            var result = handler.Execute(command);
            var player = _context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.DisplayName.Should().Be("My name");
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}