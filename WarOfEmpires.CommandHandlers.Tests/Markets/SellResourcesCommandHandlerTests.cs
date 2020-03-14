using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class SellResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public SellResourcesCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.Merchants, 10) });
            player.GetBuildingBonus(BuildingType.Market).Returns(6);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Allows_Empty_Values() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Few_Available_Merchants() {
            // Test with caravans on the market already to make sure those are taken into account
            throw new System.NotImplementedException();
        }

        // TODO duplicate these tests for wood, stone, ore
        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Food() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Food() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Too_Little_Available_Food() {
            throw new System.NotImplementedException();
        }


        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_FoodPrice() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_FoodPrice() {
            throw new System.NotImplementedException();
        }

    }
}