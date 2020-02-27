using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public static class WorkerDefinitionFactory {
        private readonly static Dictionary<WorkerType, WorkerDefinition> _workers = new Dictionary<WorkerType, WorkerDefinition>();

        static WorkerDefinitionFactory() {
            foreach (var definition in new[] { GenerateFarmers(), GenerateWoodWorkers(), GenerateStoneMasons(), GenerateOreMiners(), GenerateSiegeEngineers() }) {
                _workers.Add(definition.Type, definition);
            }
        }

        private static WorkerDefinition GenerateFarmers() {
            return new WorkerDefinition(WorkerType.Farmer, BuildingType.Farm, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateWoodWorkers() {
            return new WorkerDefinition(WorkerType.WoodWorker, BuildingType.Lumberyard, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateStoneMasons() {
            return new WorkerDefinition(WorkerType.StoneMason, BuildingType.Quarry, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateOreMiners() {
            return new WorkerDefinition(WorkerType.OreMiner, BuildingType.Mine, new Resources(gold: 250), true);
        }

        private static WorkerDefinition GenerateSiegeEngineers() {
            return new WorkerDefinition(WorkerType.SiegeEngineer, BuildingType.SiegeFactory, new Resources(gold: 2500, wood: 250, ore: 500), false);
        }

        public static WorkerDefinition Get(WorkerType type) {
            return _workers[type];
        }
    }
}
