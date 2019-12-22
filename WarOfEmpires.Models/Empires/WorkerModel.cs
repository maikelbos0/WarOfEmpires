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
        public int CurrentPeasants { get; set; }
        public int CurrentGoldPerWorkerPerTurn { get; set; }
        public int CurrentGoldPerTurn { get; set; }
        public int CurrentFarmers { get; set; }
        public int CurrentFoodPerWorkerPerTurn { get; set; }
        public int CurrentFoodPerTurn { get; set; }
        public int CurrentWoodWorkers { get; set; }
        public int CurrentWoodPerWorkerPerTurn { get; set; }
        public int CurrentWoodPerTurn { get; set; }
        public int CurrentStoneMasons { get; set; }
        public int CurrentStonePerWorkerPerTurn { get; set; }
        public int CurrentStonePerTurn { get; set; }
        public int CurrentOreMiners { get; set; }
        public int CurrentOrePerWorkerPerTurn { get; set; }
        public int CurrentOrePerTurn { get; set; }
        public int GoldUpkeepPerTurn { get; set; }
        public int FoodUpkeepPerTurn { get; set; }
        public int RecruitsPerDay { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public string Command { get; set; }
    }
}