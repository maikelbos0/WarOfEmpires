using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class PostChatMessageCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly Alliance _alliance;

        public PostChatMessageCommandHandlerTests() {
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
        public void PostChatMessageCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void PostChatMessageCommandHandler_Fails_For_No_Message() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void PostChatMessageCommandHandler__Throws_Exception_For_Player_Not_Empire_Member() {
            _player.Alliance.Returns((Alliance)null);

            throw new System.NotImplementedException();
        }
    }
}