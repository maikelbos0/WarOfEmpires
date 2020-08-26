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
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class WithdrawInviteCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Alliance _alliance;
        private readonly Player _member;
        private readonly Invite _invite;

        public WithdrawInviteCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _alliance = Substitute.For<Alliance>();

            var memberUser = Substitute.For<User>();
            memberUser.Email.Returns("test@test.com");
            memberUser.Status.Returns(UserStatus.Active);
            _member = Substitute.For<Player>();
            _member.Id.Returns(1);
            _member.User.Returns(memberUser);
            _member.Alliance.Returns(_alliance);

            _invite = Substitute.For<Invite>();
            _invite.Alliance.Returns(_alliance);
            _invite.Id.Returns(3);

            _alliance.Members.Returns(new List<Player>() { _member });
            _alliance.Invites.Returns(new List<Invite>() { _invite });

            _context.Alliances.Add(_alliance);
            _context.Players.Add(_member);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Succeeds() {
            var handler = new WithdrawInviteCommandHandler(_repository, new AllianceRepository(_context));
            var command = new WithdrawInviteCommand("test@test.com", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _alliance.Received().RemoveInvite(_invite);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Sender_Not_Empire_Member() {
            _alliance.Members.Returns(new List<Player>());
            _member.Alliance.Returns((Alliance)null);

            var handler = new WithdrawInviteCommandHandler(_repository, new AllianceRepository(_context));
            var command = new WithdrawInviteCommand("test@test.com", "3");

            Action action = () => handler.Execute(command);

            action.Should().Throw<NullReferenceException>();
            _alliance.Invites.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Alphanumeric_Invite_Id() {
            var handler = new WithdrawInviteCommandHandler(_repository, new AllianceRepository(_context));
            var command = new WithdrawInviteCommand("test@test.com", "A");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            _alliance.Invites.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Nonexistent_Invite_Id() {
            var handler = new WithdrawInviteCommandHandler(_repository, new AllianceRepository(_context));
            var command = new WithdrawInviteCommand("test@test.com", "4");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _alliance.Invites.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void WithdrawInviteCommandHandler_Throws_Exception_For_Invite_Id_Other_Alliance() {
            var alliance = Substitute.For<Alliance>();
            var invite = Substitute.For<Invite>();

            alliance.Members.Returns(new List<Player>());
            alliance.Invites.Returns(new List<Invite>() { invite });
            invite.Alliance.Returns(_alliance);
            invite.Id.Returns(4);

            _context.Alliances.Add(alliance);

            var handler = new WithdrawInviteCommandHandler(_repository, new AllianceRepository(_context));
            var command = new WithdrawInviteCommand("test@test.com", "4");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            alliance.Invites.Should().HaveCount(1);
            _alliance.Invites.Should().HaveCount(1);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}