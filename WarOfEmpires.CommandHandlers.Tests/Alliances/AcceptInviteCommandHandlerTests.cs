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
    public sealed class AcceptInviteCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Invite _invite;
        private readonly Alliance _alliance;

        public AcceptInviteCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _invite = Substitute.For<Invite>();
            _invite.Id.Returns(1);

            _alliance = Substitute.For<Alliance>();
            _alliance.Invites.Returns(new List<Invite>() { _invite });

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _player = Substitute.For<Player>();
            _player.Invites.Returns(new List<Invite>() { _invite });
            _player.Alliance.Returns((Alliance)null);
            _player.User.Returns(user);

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(_player);

            _invite.Alliance.Returns(_alliance);

            var wrongUser = Substitute.For<User>();
            wrongUser.Email.Returns("wrong@test.com");
            wrongUser.Status.Returns(UserStatus.Active);

            var wrongPlayer = Substitute.For<Player>();
            wrongPlayer.Invites.Returns(new List<Invite>());
            wrongPlayer.User.Returns(wrongUser);

            _context.Users.Add(wrongUser);
            _context.Players.Add(wrongPlayer);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Succeeds() {
            var handler = new AcceptInviteCommandHandler(_repository);
            var command = new AcceptInviteCommand("test@test.com", "1");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Invites.Should().HaveCount(0);
            _alliance.Received().AddMember(_player);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Fails_For_Player_In_Empire() {
            _player.Alliance.Returns(_alliance);

            var handler = new AcceptInviteCommandHandler(_repository);
            var command = new AcceptInviteCommand("test@test.com", "1");

            var result = handler.Execute(command);

            result.Should().HaveError("You are already in an alliance; leave your current alliance before accepting an invite");
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_For_Alphanumeric_InviteId() {
            var handler = new AcceptInviteCommandHandler(_repository);
            var command = new AcceptInviteCommand("test@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_For_Nonexistent_InviteId() {
            var handler = new AcceptInviteCommandHandler(_repository);
            var command = new AcceptInviteCommand("test@test.com", "2");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void AcceptInviteCommandHandler_Throws_Exception_InviteId_For_Different_Player() {
            var handler = new AcceptInviteCommandHandler(_repository);
            var command = new AcceptInviteCommand("wrong@test.com", "1");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}