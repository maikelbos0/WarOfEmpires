using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Markets {
    [TestClass]
    public sealed class CaravanRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestInitialize]
        public void Initialize() {
            var caravanId = 1;

            for (var playerId = 1; playerId < 3; playerId++) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();
                var caravans = new List<Caravan>();

                user.Status.Returns(UserStatus.Active);
                user.Email.Returns($"test_{playerId}@test.com");
                user.Id.Returns(playerId);
                player.User.Returns(user);
                player.Id.Returns(playerId);
                player.Caravans.Returns(caravans);

                _context.Users.Add(user);
                _context.Players.Add(player);

                for (var price = 5; price > 3; price--) {
                    foreach (var type in new[] { MerchandiseType.Wood, MerchandiseType.Stone }) {
                        var caravan = Substitute.For<Caravan>();

                        caravan.Id.Returns(caravanId++);
                        caravan.Merchandise.Returns(new List<Merchandise>() {
                            new Merchandise(type, 10000, price)
                        });
                        caravans.Add(caravan);
                    }
                }
            }
        }

        [TestMethod]
        public void CaravanRepository_GetForMerchandiseType_Succeeds() {
            var repository = new CaravanRepository(_context);

            var result = repository.GetForMerchandiseType(MerchandiseType.Wood).ToList();

            result.Count.Should().Be(4);
            result[0].Id.Should().Be(3);
            result[0].Merchandise.Single().Price.Should().Be(4);
            result[3].Id.Should().Be(5);
            result[3].Merchandise.Single().Price.Should().Be(5);
        }

        [TestMethod]
        public void CaravanRepository_GetForMerchandiseType_Does_Not_Save() {
            var repository = new CaravanRepository(_context);

            repository.GetForMerchandiseType(MerchandiseType.Wood);

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CaravanRepository_Update_Saves() {
            var repository = new CaravanRepository(_context);

            repository.Update();

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}