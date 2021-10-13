using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public sealed class RaceDefinition {
        private Dictionary<WorkerType, decimal> _workerModifiers = new Dictionary<WorkerType, decimal>();
        private Dictionary<TroopType, decimal> _troopModifiers = new Dictionary<TroopType, decimal>();

        public Race Race { get; }
        public string Description { get; }

        public RaceDefinition(
            Race race, 
            string description,
            decimal farmerModifier = 1M,
            decimal woodWorkerModifier = 1M, 
            decimal stoneMasonModifier = 1M, 
            decimal oreMinerModifier = 1M,
            decimal archerModifier = 1M, 
            decimal cavalryModifier = 1M,
            decimal footmenModifier = 1M
        ) {
            Race = race;
            Description = description;
            _workerModifiers.Add(WorkerType.Farmers, farmerModifier);
            _workerModifiers.Add(WorkerType.WoodWorkers, woodWorkerModifier);
            _workerModifiers.Add(WorkerType.StoneMasons, stoneMasonModifier);
            _workerModifiers.Add(WorkerType.OreMiners, oreMinerModifier);
            _troopModifiers.Add(TroopType.Archers, archerModifier);
            _troopModifiers.Add(TroopType.Cavalry, cavalryModifier);
            _troopModifiers.Add(TroopType.Footmen, footmenModifier);
        }

        public decimal GetWorkerModifier(WorkerType workerType) {
            return _workerModifiers[workerType];
        }

        public decimal GetTroopModifier(TroopType troopType) {
            return _troopModifiers[troopType];
        }
    }
}
