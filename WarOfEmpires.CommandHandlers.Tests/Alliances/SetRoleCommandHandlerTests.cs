using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class SetRoleCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Alliance _alliance;
        private readonly Role _role;

        public SetRoleCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _alliance = Substitute.For<Alliance>();

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            user.Id.Returns(2);

            _player = Substitute.For<Player>();
            _player.Alliance.Returns(_alliance);
            _player.User.Returns(user);
            _player.Id.Returns(2);

            _role = Substitute.For<Role>();
            _role.Alliance.Returns(_alliance);
            _role.Id.Returns(3);
            _role.Players.Returns(new List<Player>() { });

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Succeeds() {
            var handler = new SetRoleCommandHandler(_repository);
            var command = new SetRoleCommand("test@test.com", "2", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Received().SetRole(_player, _role);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            _player.Alliance.Returns((Alliance)null);

            var handler = new SetRoleCommandHandler(_repository);
            var command = new SetRoleCommand("test@test.com", "2", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            _alliance.Roles.Remove(_role);

            var handler = new SetRoleCommandHandler(_repository);
            var command = new SetRoleCommand("test@test.com", "2", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SetRoleCommandHandler_Throws_Exception_For_Member_Of_Different_Alliance() {
            _alliance.Members.Remove(_player);

            var handler = new SetRoleCommandHandler(_repository);
            var command = new SetRoleCommand("test@test.com", "2", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.DidNotReceiveWithAnyArgs().SetRole(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}