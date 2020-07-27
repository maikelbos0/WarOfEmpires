using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class CreateRoleCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Alliance _alliance;

        public CreateRoleCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _alliance = Substitute.For<Alliance>();

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _player = Substitute.For<Player>();
            _player.Alliance.Returns(_alliance);
            _player.User.Returns(user);

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void CreateRoleCommandHandler_Succeeds() {
            var handler = new CreateRoleCommandHandler(_repository);
            var command = new CreateRoleCommand("test@test.com", "Diva");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Received().CreateRole("Diva");
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void CreateRoleCommandHandler_Throws_Exception_For_Nonexistent_Player() {
            var handler = new CreateRoleCommandHandler(_repository);
            var command = new CreateRoleCommand("wrong@test.com", "Diva");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.DidNotReceiveWithAnyArgs().CreateRole(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CreateRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            _player.Alliance.Returns((Alliance)null);

            var handler = new CreateRoleCommandHandler(_repository);
            var command = new CreateRoleCommand("test@test.com", "Diva");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            _alliance.DidNotReceiveWithAnyArgs().CreateRole(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}