﻿namespace WarOfEmpires.Models.Empires {
    public sealed class WorkerInfoViewModel {
        public ResourcesViewModel Cost { get; set; }
        public int CurrentWorkers { get; set; }
        public int CurrentProductionPerWorkerPerTurn { get; set; }
        public int CurrentProductionPerTurn { get; set; }
    }
}