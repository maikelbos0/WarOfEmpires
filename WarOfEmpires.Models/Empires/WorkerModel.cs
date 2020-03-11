using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class WorkerModel {
        [DisplayName("Farmers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Farmers must be a valid number")]
        public string Farmers { get; set; }
        [DisplayName("Wood workers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Wood workers must be a valid number")]
        public string WoodWorkers { get; set; }
        [DisplayName("Stone masons")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Stone masons must be a valid number")]
        public string StoneMasons { get; set; }
        [DisplayName("Ore miners")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Ore miners must be a valid number")]
        public string OreMiners { get; set; }
        [DisplayName("Siege engineers")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Siege engineers must be a valid number")]
        public string SiegeEngineers { get; set; }
        [DisplayName("Merchants")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Merchants must be a valid number")]
        public string Merchants { get; set; }
        public int CurrentPeasants { get; set; }
        public int CurrentGoldPerWorkerPerTurn { get; set; }
        public int CurrentGoldPerTurn { get; set; }
        public WorkerInfoViewModel FarmerInfo { get; set; }
        public WorkerInfoViewModel WoodWorkerInfo { get; set; }
        public WorkerInfoViewModel StoneMasonInfo { get; set; }
        public WorkerInfoViewModel OreMinerInfo { get; set; }
        public WorkerInfoViewModel SiegeEngineerInfo { get; set; }
        public WorkerInfoViewModel MerchantInfo { get; set; }
        public ResourcesViewModel UpkeepPerTurn { get; set; }
        public int RecruitsPerDay { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public string Command { get; set; }
    }
}