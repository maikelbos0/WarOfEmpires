﻿using System.ComponentModel.DataAnnotations;

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
        public string Command { get; set; }
    }
}