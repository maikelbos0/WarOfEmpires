using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _context.Players.Add(new Player(0, "Test"));
        }

        [TestMethod]
        public void PlayerRepository_Get_Succeeds() {
            var repository = new PlayerRepository(_context);

            var player = repository.Get(0);

            player.Should().NotBeNull();
            player.DisplayName.Should().Be("Test");
        }

        [TestMethod]
        public void PlayerRepository_Get_Does_Not_Save() {
            var repository = new PlayerRepository(_context);

            repository.Get(0);

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