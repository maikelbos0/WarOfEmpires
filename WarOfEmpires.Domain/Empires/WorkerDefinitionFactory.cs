using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public static class WorkerDefinitionFactory {
        private readonly static Dictionary<WorkerType, WorkerDefinition> _workers = new();

        static WorkerDefinitionFactory() {
            foreach (var definition in new[] {
                GenerateFarmers(), GenerateWoodWorkers(), GenerateStoneMasons(), GenerateOreMiners(), 
                GenerateSiegeEngineers(), GenerateMerchants(), GenerateScientists()
            }) {
                _workers.Add(definition.Type, definition);
            }
        }

        private static WorkerDefinition GenerateFarmers() {
            return new WorkerDefinition(WorkerType.Farmers, BuildingType.Farm, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateWoodWorkers() {
            return new WorkerDefinition(WorkerType.WoodWorkers, BuildingType.Lumberyard, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateStoneMasons() {
            return new WorkerDefinition(WorkerType.StoneMasons, BuildingType.Quarry, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateOreMiners() {
            return new WorkerDefinition(WorkerType.OreMiners, BuildingType.Mine, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateSiegeEngineers() {
            return new WorkerDefinition(WorkerType.SiegeEngineers, BuildingType.SiegeFactory, new Resources(gold: 2500, wood: 250, ore: 500), false);
        }

        private static WorkerDefinition GenerateMerchants() {
            return new WorkerDefinition(WorkerType.Merchants, BuildingType.Market, new Resources(gold: 2500, wood: 500, ore: 250), false);
        }

        private static WorkerDefinition GenerateScientists() {
            return new WorkerDefinition(WorkerType.Scientists, BuildingType.University, new Resources(gold: 2500, wood: 250, stone: 250, ore: 250), false);
        }

        public static WorkerDefinition Get(WorkerType type) {
            return _workers[type];
        }
    }
}
