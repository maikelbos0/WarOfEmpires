using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class WorkerModel {
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public string Count { get; set; }
        public string Type { get; set; }
        public bool IsProducer { get; set; }
        public string Name { get; set; }
        public ResourcesViewModel Cost { get; set; }
        public int CurrentWorkers { get; set; }
        public int CurrentProductionPerWorkerPerTurn { get; set; }
        public int CurrentProductionPerTurn { get; set; }
    }
}