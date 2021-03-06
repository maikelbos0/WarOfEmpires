﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TaxModel {
        [DisplayName("Tax rate")]
        [Range(0, 100, ErrorMessage = "Tax must be a valid number")]
        public int Tax { get; set; }
        public int BaseGoldPerTurn { get; set; }
        public int CurrentGoldPerWorkerPerTurn { get; set; }
        public int BaseFoodPerTurn { get; set; }
        public int CurrentFoodPerWorkerPerTurn { get; set; }
        public int BaseWoodPerTurn { get; set; }
        public int CurrentWoodPerWorkerPerTurn { get; set; }
        public int BaseStonePerTurn { get; set; }
        public int CurrentStonePerWorkerPerTurn { get; set; }
        public int BaseOrePerTurn { get; set; }
        public int CurrentOrePerWorkerPerTurn { get; set; }
    }
}