using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetDefenderQueryHandlerTests {
        [TestMethod]
        public void GetDefenderQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1)
                .BuildPlayer(2)
                .WithPopulation();

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.DefenderId.Should().Be(2);
            result.DisplayName.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
            result.IsAtWar.Should().BeFalse();
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Returns_IsAtWar_For_War() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2)
                .WithWar(1, builder.Alliance);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.IsAtWar.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(GamePhase.Truce, true, DisplayName = "Truce")]
        [DataRow(GamePhase.Active, false, DisplayName = "Active")]
        [DataRow(GamePhase.Finished, false, DisplayName = "Finished")]
        public void GetDefenderQueryHandler_Returns_Correct_IsTruce(GamePhase phase, bool expectedIsTruce) {
            var builder = new FakeBuilder()
                .WithGameStatus(1, phase: phase)
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.IsTruce.Should().Be(expectedIsTruce);
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}