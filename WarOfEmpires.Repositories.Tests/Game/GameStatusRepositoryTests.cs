using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Repositories.Game;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Game {
    [TestClass]
    public sealed class GameStatusRepositoryTests {
        [TestMethod]
        public void GameRepository_Get_Succeeds() {
            var builder = new FakeBuilder()
                .WithGameStatus(1);

            var repository = new GameStatusRepository(builder.Context);

            var gameStatus = repository.Get();

            gameStatus.Should().NotBeNull();
            gameStatus.Id.Should().Be(1);
        }

        [TestMethod]
        public void GameRepository_Get_Throws_Exception_For_None() {
            var builder = new FakeBuilder();

            var repository = new GameStatusRepository(builder.Context);

            Action action = () => repository.Get();
        
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GameRepository_Throws_Exception_For_Multiple() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithGameStatus(2);

            var repository = new GameStatusRepository(builder.Context);

            Action action = () => repository.Get();

            action.Should().Throw<InvalidOperationException>();
        }
    }
}