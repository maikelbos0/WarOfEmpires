using System.Collections.Generic;

namespace WarOfEmpires.Models.Empires {
    public sealed class WorkersModel {
        public List<WorkerModel> Workers { get; set; }
        public int CurrentPeasants { get; set; }
        public int CurrentGoldPerWorkerPerTurn { get; set; }
        public int CurrentGoldPerTurn { get; set; }
        public ResourcesViewModel WorkerCost { get; set; }
        public ResourcesViewModel UpkeepPerTurn { get; set; }
        public int RecruitsPerDay { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public string Command { get; set; }
    }
}