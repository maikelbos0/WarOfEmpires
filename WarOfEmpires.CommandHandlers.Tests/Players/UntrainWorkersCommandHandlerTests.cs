using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class UntrainWorkersCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;

        public UntrainWorkersCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Farmers.Returns(10);
            player.WoodWorkers.Returns(10);
            player.StoneMasons.Returns(10);
            player.OreMiners.Returns(10);
            player.Gold.Returns(10000);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }
        
        [TestMethod]
        public void UntrainWorkersCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_WorkerCounts() {
            throw new System.NotImplementedException();
        }

        [DataTestMethod()]
        [DataRow(11, 0, 0, 0, DisplayName = "Farmers")]
        [DataRow(0, 11, 0, 0, DisplayName = "WoodWorkers")]
        [DataRow(0, 0, 11, 0, DisplayName = "StoneMasons")]
        [DataRow(0, 0, 0, 11, DisplayName = "OreMiners")]
        [DataRow(3, 3, 3, 3, DisplayName = "All")]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_WorkerCounts(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            throw new System.NotImplementedException();
        }

        [DataTestMethod()]
        [DataRow(11, 0, 0, 0, DisplayName = "Farmers")]
        [DataRow(0, 11, 0, 0, DisplayName = "WoodWorkers")]
        [DataRow(0, 0, 11, 0, DisplayName = "StoneMasons")]
        [DataRow(0, 0, 0, 11, DisplayName = "OreMiners")]
        [DataRow(3, 3, 3, 3, DisplayName = "All")]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_WorkerCounts(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            throw new System.NotImplementedException();
        }
    }
}