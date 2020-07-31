using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetDefenderQueryHandlerTests {
        [TestMethod]
        public void GetDefenderQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(2)
                .WithPopulation();

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("2");

            var result = handler.Execute(query);

            result.DefenderId.Should().Be(2);
            result.DisplayName.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var builder = new FakeBuilder()
                .BuildPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .BuildPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("5");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}