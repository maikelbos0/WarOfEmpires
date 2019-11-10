using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Players {
    [TestClass]
    public sealed class PlayerRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestInitialize]
        public void Initialize() {
            var id = 1;

            foreach (var status in new[] { UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();

                user.Status.Returns(status);
                user.Id.Returns(id);
                player.User.Returns(user);
                player.Id.Returns(id++);

                _context.Users.Add(user);
                _context.Players.Add(player);
            }
        }

        [TestMethod]
        public void PlayerRepository_Get_Succeeds() {
            var repository = new PlayerRepository(_context);

            var player = repository.Get(1);

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Get_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.Get(1);

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
    }
}