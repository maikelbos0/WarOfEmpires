using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class WithdrawCaravanCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly CaravanRepository _caravanRepository;
        private readonly Player _player;

        public WithdrawCaravanCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
            _caravanRepository = new CaravanRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            _player = Substitute.For<Player>();
            _player.User.Returns(user);
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Succeeds() {
            var handler = new WithdrawCaravanCommandHandler(_repository, _caravanRepository);

            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Destroys_Resources() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void WithdrawCaravanCommandHandler_Throws_Exception_For_Caravan_Of_Different_Player() {
            throw new System.NotImplementedException();
        }
    }
}