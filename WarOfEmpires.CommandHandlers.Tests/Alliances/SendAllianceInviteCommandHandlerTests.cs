using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class SendAllianceInviteCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Alliance _alliance;
        private readonly Player _member;
        private readonly Player _player;

        public SendAllianceInviteCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _alliance = Substitute.For<Alliance>();

            var memberUser = Substitute.For<User>();
            memberUser.Id.Returns(1);
            memberUser.Email.Returns("test@test.com");
            memberUser.Status.Returns(UserStatus.Active);
            _member = Substitute.For<Player>();
            _member.User.Returns(memberUser);
            _member.Alliance.Returns(_alliance);
            _alliance.Members.Returns(new List<Player>() { _member });

            var user = Substitute.For<User>();
            user.Id.Returns(2);
            user.Email.Returns("invite@test.com");
            user.Status.Returns(UserStatus.Active);
            _player = Substitute.For<Player>();
            _player.User.Returns(user);
            _player.Alliance.Returns((Alliance)null);

            _context.Alliances.Add(_alliance);
            _context.Players.Add(_member);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void SendAllianceInviteCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendAllianceInviteCommandHandler_Fails_For_Player_Already_Invited() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendAllianceInviteCommandHandler_Throws_Exception_For_Member_Not_In_Empire() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendAllianceInviteCommandHandler_Throws_Exception_For_Alphanumeric_Player_Id() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendAllianceInviteCommandHandler_Throws_Exception_For_Nonexistent_Player_Id() {
            throw new System.NotImplementedException();
        }
    }
}