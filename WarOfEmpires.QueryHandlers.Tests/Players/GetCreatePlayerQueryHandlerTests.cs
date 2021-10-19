using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetCreatePlayerQueryHandlerTests {
        [TestMethod]
        public void GetCreatePlayerQueryHandler_Returns_Correct_Information() {
            var races = Enum.GetValues(typeof(Race)).Cast<Race>().ToList();
            var enumFormatter = new EnumFormatter();
            
            var handler = new GetCreatePlayerQueryHandler(enumFormatter);
            var query = new GetCreatePlayerQuery();

            var result = handler.Execute(query);

            result.Races.Should().HaveSameCount(races);

            foreach (var race in races) {
                var raceModel = result.Races.Should().ContainSingle(r => r.Race == race.ToString()).Subject;
                var raceDefinition = RaceDefinitionFactory.Get(race);

                raceModel.Name.Should().Be(enumFormatter.ToString(race));
                raceModel.Description.Should().Be(raceDefinition.Description);
                raceModel.FarmerModifier.Should().Be(raceDefinition.GetWorkerModifier(WorkerType.Farmers));
                raceModel.WoodWorkerModifier.Should().Be(raceDefinition.GetWorkerModifier(WorkerType.WoodWorkers));
                raceModel.StoneMasonModifier.Should().Be(raceDefinition.GetWorkerModifier(WorkerType.StoneMasons));
                raceModel.OreMinerModifier.Should().Be(raceDefinition.GetWorkerModifier(WorkerType.OreMiners));
                raceModel.ArcherModifier.Should().Be(raceDefinition.GetTroopModifier(TroopType.Archers));
                raceModel.CavalryModifier.Should().Be(raceDefinition.GetTroopModifier(TroopType.Cavalry));
                raceModel.FootmenModifier.Should().Be(raceDefinition.GetTroopModifier(TroopType.Footmen));
            }
        }
    }
}
