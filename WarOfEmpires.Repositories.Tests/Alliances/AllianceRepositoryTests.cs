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
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Alliances {
    [TestClass]
    public sealed class AllianceRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public AllianceRepositoryTests() {
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
        public void AllianceRepository_Get_Succeeds() {
            var repository = new AllianceRepository(_context);

            var alliance = repository.Get(1);

            alliance.Should().NotBeNull();
            alliance.Id.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Get_Throws_Exception_For_Nonexistent_Id() {
            var repository = new AllianceRepository(_context);

            Action action = () => repository.Get(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void AllianceRepository_Get_Does_Not_Save() {
            var repository = new AllianceRepository(_context);

            repository.Get(1);

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Succeeds() {
            var repository = new AllianceRepository(_context);

            var alliances = repository.GetAll();

            alliances.Should().NotBeNull();
            alliances.Should().HaveCount(2);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Does_Not_Save() {
            var repository = new AllianceRepository(_context);

            repository.GetAll();

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_Add_Succeeds() {
            var repository = new AllianceRepository(_context);
            var alliance = new Alliance(null, "ALLY", "The Alliance");

            repository.Add(alliance);

            _context.Alliances.Should().Contain(alliance);
        }

        [TestMethod]
        public void AllianceRepository_Add_Saves() {
            var repository = new AllianceRepository(_context);

            repository.Add(new Alliance(null, "ALLY", "The Alliance"));

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_RemoveInvite_Succeeds() {
            var repository = new AllianceRepository(_context);
            var previousInviteCount = _context.Alliances.Sum(a => a.Invites.Count());

            repository.RemoveInvite(_context.Alliances.First().Invites.First());

            _context.Alliances.Sum(a => a.Invites.Count()).Should().Be(previousInviteCount - 1);
        }

        [TestMethod]
        public void AllianceRepository_RemoveInvite_Saves() {
            var repository = new AllianceRepository(_context);

            repository.RemoveInvite(_context.Alliances.First().Invites.First());

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Update_Saves() {
            var repository = new AllianceRepository(_context);

            repository.Update();

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}