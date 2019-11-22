using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class WorkerModel {
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Farmers must be a valid number")]
        public string Farmers { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Wood workers must be a valid number")]
        public string WoodWorkers { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Stone masons must be a valid number")]
        public string StoneMasons { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Ore miners must be a valid number")]
        public string OreMiners { get; set; }
        public int CurrentPeasants { get; set; }
        public int CurrentFarmers { get; set; }
        public int CurrentWoodWorkers { get; set; }
        public int CurrentStoneMasons { get; set; }
        public int CurrentOreMiners { get; set; }
        public string Command { get; set; }
    }
}