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
    public sealed class ClearRoleCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Alliance _alliance;

        public ClearRoleCommandHandlerTests() {
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

            _alliance.Members.Returns(new List<Player>() { _player });

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void ClearRoleCommandHandler_Succeeds() {
            var handler = new ClearRoleCommandHandler(_repository);
            var command = new ClearRoleCommand("test@test.com", "2");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Received().ClearRole(_player);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ClearRoleCommandHandler_Throws_Exception_For_Player_Not_In_Alliance() {
            _player.Alliance.Returns((Alliance)null);

            var handler = new ClearRoleCommandHandler(_repository);
            var command = new ClearRoleCommand("test@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            _alliance.DidNotReceiveWithAnyArgs().ClearRole(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void ClearRoleCommandHandler_Throws_Exception_For_Member_Of_Different_Alliance() {
            _alliance.Members.Remove(_player);

            var handler = new ClearRoleCommandHandler(_repository);
            var command = new ClearRoleCommand("test@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.DidNotReceiveWithAnyArgs().ClearRole(default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}