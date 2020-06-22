using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class SendInviteCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Alliance _alliance;
        private readonly Player _member;
        private readonly Player _player;

        public SendInviteCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _alliance = Substitute.For<Alliance>();

            var memberUser = Substitute.For<User>();
            memberUser.Email.Returns("test@test.com");
            memberUser.Status.Returns(UserStatus.Active);
            _member = Substitute.For<Player>();
            _member.Id.Returns(1);
            _member.User.Returns(memberUser);
            _member.Alliance.Returns(_alliance);

            _alliance.Members.Returns(new List<Player>() { _member });
            _alliance.Invites.Returns(new List<Invite>());

            var user = Substitute.For<User>();
            user.Email.Returns("invite@test.com");
            user.Status.Returns(UserStatus.Active);
            _player = Substitute.For<Player>();
            _player.Id.Returns(2);
            _player.User.Returns(user);
            _player.Alliance.Returns((Alliance)null);

            _context.Alliances.Add(_alliance);
            _context.Players.Add(_member);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Succeeds() {
            var handler = new SendInviteCommandHandler(_repository);
            var command = new SendInviteCommand("test@test.com", "2", "Test message", "Message body");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Invites.Should().HaveCount(1);
            _alliance.Invites.Single().Subject.Should().Be("Test message");
            _alliance.Invites.Single().Body.Should().Be("Message body");
            _alliance.Invites.Single().Player.Should().Be(_player);
            _alliance.Invites.Single().Alliance.Should().Be(_alliance);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Fails_For_Player_Already_Invited() {
            _alliance.Invites.Returns(new List<Invite>() { new Invite(_alliance, _player, null, null) });

            var handler = new SendInviteCommandHandler(_repository);
            var command = new SendInviteCommand("test@test.com", "2", "Test message", "Message body");

            var result = handler.Execute(command);

            result.Should().HaveError("This player already has an open invite and can not be invited again");
            _alliance.Invites.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Sender_Not_Empire_Member() {
            var handler = new SendInviteCommandHandler(_repository);
            var command = new SendInviteCommand("invite@test.com", "2", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            _alliance.Invites.Should().HaveCount(0);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Alphanumeric_Player_Id() {
            var handler = new SendInviteCommandHandler(_repository);
            var command = new SendInviteCommand("test@test.com", "A", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            _alliance.Invites.Should().HaveCount(0);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void SendInviteCommandHandler_Throws_Exception_For_Nonexistent_Player_Id() {
            var handler = new SendInviteCommandHandler(_repository);
            var command = new SendInviteCommand("test@test.com", "3", "Test message", "Message body");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.Invites.Should().HaveCount(0);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}