using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetLastExecutedAttackQueryHandlerTests {
        [TestMethod]
        public void GetLastExecutedAttackQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var defender)
                .BuildPlayer(3)
                .WithAttackOn(7, defender, AttackType.Assault, AttackResult.Defended)
                .BuildPlayer(1)
                .WithAttackOn(5, defender, AttackType.Raid, AttackResult.Won, resources: new Resources(1, 2, 3, 4, 5))
                .WithAttackOn(1, defender, AttackType.Raid, AttackResult.Won, resources: new Resources(1, 2, 3, 4, 5))
                .WithAttackOn(2, defender, AttackType.Raid, AttackResult.Won, resources: new Resources(1, 2, 3, 4, 5));

            var handler = new GetLastExecutedAttackQueryHandler(builder.Context);
            var query = new GetLastExecutedAttackQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().Be(5);
        }
    }
}