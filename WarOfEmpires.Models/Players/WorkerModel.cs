using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Players {
    public sealed class WorkerModel {
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Farmers must be a valid number")]
        public string Farmers { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Wood workers must be a valid number")]
        public string WoodWorkers { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Stone masons must be a valid number")]
        public string StoneMasons { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Ore miners must be a valid number")]
        public string OreMiners { get; set; }
        public string Command { get; set; }
    }
}