using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Players {
    [TestClass]
    public sealed class PlayerRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public PlayerRepositoryTests() {
            var id = 1;
            var caravanId = 1;

            foreach (var status in new[] { UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();
                var caravans = new List<Caravan>();

                user.Status.Returns(status);
                user.Email.Returns($"test_{id}@test.com");
                user.Id.Returns(id);
                player.User.Returns(user);
                player.Id.Returns(id++);
                player.Caravans.Returns(caravans);

                _context.Users.Add(user);
                _context.Players.Add(player);

                for (var price = 5; price > 3; price--) {
                    foreach (var type in new[] { MerchandiseType.Wood, MerchandiseType.Stone }) {
                        var caravan = Substitute.For<Caravan>();

                        caravan.Id.Returns(caravanId++);
                        caravan.Player.Returns(player);
                        caravan.Merchandise.Returns(new List<Merchandise>() {
                            new Merchandise(type, 10000, price)
                        });

                        caravans.Add(caravan);

                        var emptyCaravan = Substitute.For<Caravan>();

                        emptyCaravan.Id.Returns(caravanId++);
                        emptyCaravan.Player.Returns(player);
                        emptyCaravan.Merchandise.Returns(new List<Merchandise>() {
                            new Merchandise(type, 0, price)
                        });

                        caravans.Add(emptyCaravan);
                    }
                }
            }

            id = 1;

            foreach (var name in new[] { "Alliance", "The Enemy" }) {
                var alliance = Substitute.For<Alliance>();
                var invite = Substitute.For<Invite>();

                invite.Alliance.Returns(alliance);

                alliance.Id.Returns(id++);
                alliance.Name.Returns(name);
                alliance.Invites.Returns(new List<Invite>(){ invite });

                _context.Alliances.Add(alliance);
            }            
        }

        [TestMethod]
        public void PlayerRepository_Get_By_Id_Succeeds() {
            var repository = new PlayerRepository(_context);

            var player = repository.Get(1);

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Get_By_Id_Throws_Exception_For_Nonexistent_Id() {
            var repository = new PlayerRepository(_context);

            Action action = () => repository.Get(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_By_Id_Throws_Exception_For_Wrong_Status() {
            var repository = new PlayerRepository(_context);

            Action action = () => repository.Get(2);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Succeeds() {
            var repository = new PlayerRepository(_context);

            var player = repository.Get("test_1@test.com");

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Get_Throws_Exception_For_Nonexistent_Email() {
            var repository = new PlayerRepository(_context);

            Action action = () => repository.Get("nobody@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Throws_Exception_For_Wrong_Status() {
            var repository = new PlayerRepository(_context);

            Action action = () => repository.Get("test_2@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.Get("test_1@test.com");

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_GetAll_Succeeds() {
            var repository = new PlayerRepository(_context);

            var players = repository.GetAll();

            players.Should().NotBeNull();
            players.Should().HaveCount(1);
        }

        [TestMethod]
        public void PlayerRepository_GetAll_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.GetAll();

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_Add_Succeeds() {
            var repository = new PlayerRepository(_context);
            var player = new Player(0, "New");

            repository.Add(player);

            _context.Players.Should().Contain(player);
        }

        [TestMethod]
        public void PlayerRepository_Add_Saves() {
            var repository = new PlayerRepository(_context);

            repository.Add(new Player(0, "New"));

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Update_Saves() {
            var repository = new PlayerRepository(_context);

            repository.Update();

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_GetCaravans_Succeeds() {
            var repository = new PlayerRepository(_context);

            var result = repository.GetCaravans(MerchandiseType.Wood);

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Id == 5 && c.Merchandise.Any(m => m.Type == MerchandiseType.Wood && m.Price == 4));
            result.Should().Contain(c => c.Id == 1 && c.Merchandise.Any(m => m.Type == MerchandiseType.Wood && m.Price == 5));
        }

        [TestMethod]
        public void PlayerRepository_GetCaravans_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.GetCaravans(MerchandiseType.Wood);

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_RemoveCaravan_Succeeds() {
            var repository = new PlayerRepository(_context);
            var previousCaravanCount = _context.Players.Sum(p => p.Caravans.Count());

            repository.RemoveCaravan(_context.Players.First().Caravans.First());

            _context.Players.Sum(p => p.Caravans.Count()).Should().Be(previousCaravanCount - 1);
        }

        [TestMethod]
        public void PlayerRepository_RemoveCaravan_Saves() {
            var repository = new PlayerRepository(_context);

            repository.RemoveCaravan(_context.Players.First().Caravans.First());

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_GetAlliance_Succeeds() {
            var repository = new PlayerRepository(_context);

            var alliance = repository.GetAlliance(1);

            alliance.Should().NotBeNull();
            alliance.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_GetAlliance_Throws_Exception_For_Nonexistent_Id() {
            var repository = new PlayerRepository(_context);

            Action action = () => repository.GetAlliance(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_GetAlliance_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.GetAlliance(1);

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_GetAllAlliances_Succeeds() {
            var repository = new PlayerRepository(_context);

            var alliances = repository.GetAllAlliances();

            alliances.Should().NotBeNull();
            alliances.Should().HaveCount(2);
        }

        [TestMethod]
        public void PlayerRepository_GetAllAlliances_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.GetAllAlliances();

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_AddAlliance_Succeeds() {
            var repository = new PlayerRepository(_context);
            var alliance = new Alliance(null, "ALLY", "The Alliance");

            repository.AddAlliance(alliance);

            _context.Alliances.Should().Contain(alliance);
        }

        [TestMethod]
        public void PlayerRepository_AddAlliance_Saves() {
            var repository = new PlayerRepository(_context);

            repository.AddAlliance(new Alliance(null, "ALLY", "The Alliance"));

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_RemoveInvite_Succeeds() {
            var repository = new PlayerRepository(_context);
            var previousInviteCount = _context.Alliances.Sum(a => a.Invites.Count());

            repository.RemoveInvite(_context.Alliances.First().Invites.First());

            _context.Alliances.Sum(a => a.Invites.Count()).Should().Be(previousInviteCount - 1);
        }

        [TestMethod]
        public void PlayerRepository_RemoveInvite_Saves() {
            var repository = new PlayerRepository(_context);

            repository.RemoveInvite(_context.Alliances.First().Invites.First());

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}