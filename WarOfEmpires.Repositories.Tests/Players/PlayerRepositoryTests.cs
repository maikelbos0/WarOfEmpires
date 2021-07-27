using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Players {
    [TestClass]
    public sealed class PlayerRepositoryTests {
        [TestMethod]
        public void PlayerRepository_Get_By_Id_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            var player = repository.Get(1);

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Get_By_Id_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            Action action = () => repository.Get(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_By_Id_Throws_Exception_For_Wrong_Status() {
            var builder = new FakeBuilder()
                .WithPlayer(1, status: UserStatus.Inactive);

            var repository = new PlayerRepository(builder.Context);

            Action action = () => repository.Get(1);

            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow(UserStatus.New, DisplayName = "New")]
        [DataRow(UserStatus.Active, DisplayName = "Active")]
        [DataRow(UserStatus.Inactive, DisplayName = "Inactive")]
        public void PlayerRepository_GetIgnoringStatus_Succeeds(UserStatus status) {
            var builder = new FakeBuilder()
                .WithPlayer(1, status: status);

            var repository = new PlayerRepository(builder.Context);

            var player = repository.GetIgnoringStatus(1);

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_GetIgnoringStatus_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            Action action = () => repository.GetIgnoringStatus(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            var player = repository.Get("test1@test.com");

            player.Should().NotBeNull();
            player.Id.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_Get_Throws_Exception_For_Nonexistent_Email() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            Action action = () => repository.Get("wrong@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Throws_Exception_For_Wrong_Status() {
            var builder = new FakeBuilder()
                .WithPlayer(1, status: UserStatus.Inactive);

            var repository = new PlayerRepository(builder.Context);

            Action action = () => repository.Get("test1@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void PlayerRepository_Get_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            repository.Get("test1@test.com");

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_GetAll_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2)
                .WithPlayer(3, status: UserStatus.Inactive)
                .WithPlayer(4, status: UserStatus.New);

            var repository = new PlayerRepository(builder.Context);

            var players = repository.GetAll();

            players.Should().NotBeNull();
            players.Should().HaveCount(2);
        }

        [TestMethod]
        public void PlayerRepository_GetAll_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var repository = new PlayerRepository(builder.Context);

            repository.GetAll();

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void PlayerRepository_Add_Succeeds() {
            var context = new FakeWarContext();

            var repository = new PlayerRepository(context);
            var player = new Player(0, "New");

            repository.Add(player);

            context.Players.Should().Contain(player);
        }

        [TestMethod]
        public void PlayerRepository_Add_Saves() {
            var context = new FakeWarContext();

            var repository = new PlayerRepository(context);

            repository.Add(new Player(0, "New"));

            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PlayerRepository_GetCaravans_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithCaravan(1, new Merchandise(MerchandiseType.Wood, 10000, 5))
                .WithCaravan(2, new Merchandise(MerchandiseType.Stone, 10000, 8))
                .BuildPlayer(2)
                .WithCaravan(3, new Merchandise(MerchandiseType.Wood, 10000, 4))
                .WithCaravan(4, new Merchandise(MerchandiseType.Stone, 10000, 7));

            var repository = new PlayerRepository(builder.Context);

            var result = repository.GetCaravans(MerchandiseType.Wood);

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Id == 3 && c.Merchandise.Any(m => m.Type == MerchandiseType.Wood && m.Price == 4));
            result.Should().Contain(c => c.Id == 1 && c.Merchandise.Any(m => m.Type == MerchandiseType.Wood && m.Price == 5));
        }

        [TestMethod]
        public void PlayerRepository_GetCaravans_Does_Not_Save() {
            var context = new FakeWarContext();

            var repository = new PlayerRepository(context);

            repository.GetCaravans(MerchandiseType.Wood);

            context.CallsToSaveChanges.Should().Be(0);
        }
    }
}