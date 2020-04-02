﻿using System;
using System.Linq;
using System.Web.Mvc;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Empire")]
    public class EmpireController : BaseController {
        public EmpireController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("Workers")]
        [HttpGet]
        public ActionResult Workers() {
            return View(_messageService.Dispatch(new GetWorkersQuery(_authenticationService.Identity)));
        }

        [Route("Workers")]
        [HttpPost]
        public ActionResult Workers(WorkersModel model) {
            switch (model.Command) {
                case "train":
                    return ValidatedCommandResult(model,
                        new TrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners, model.SiegeEngineers, model.Merchants),
                        () => Workers());
                case "untrain":
                    return ValidatedCommandResult(model,
                        new UntrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners, model.SiegeEngineers, model.Merchants),
                        () => Workers());
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }

        [Route("Troops")]
        [HttpGet]
        public ActionResult Troops() {
            return View(_messageService.Dispatch(new GetTroopsQuery(_authenticationService.Identity)));
        }

        [Route("Troops")]
        [HttpPost]
        public ActionResult Troops(TroopsModel model) {
            switch (model.Command) {
                case "train":
                    return ValidatedCommandResult(model,
                        new TrainTroopsCommand(_authenticationService.Identity, model.Archers, model.MercenaryArchers, model.Cavalry, model.MercenaryCavalry, model.Footmen, model.MercenaryFootmen),
                        () => Troops());
                case "untrain":
                    return ValidatedCommandResult(model,
                        new UntrainTroopsCommand(_authenticationService.Identity, model.Archers, model.MercenaryArchers, model.Cavalry, model.MercenaryCavalry, model.Footmen, model.MercenaryFootmen),
                        () => Troops());
                case "heal":
                    return ValidatedCommandResult(model,
                        new HealTroopsCommand(_authenticationService.Identity, model.StaminaToHeal),
                        () => Troops());
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");

            }
        }

        [Route("Tax")]
        public ActionResult Tax() {
            return View(_messageService.Dispatch(new GetTaxQuery(_authenticationService.Identity)));
        }

        [Route("Tax")]
        [HttpPost]
        public ActionResult Tax(TaxModel model) {
            return ValidatedCommandResult(model,
                new SetTaxCommand(_authenticationService.Identity, model.Tax),
                () => Tax());
        }

        [Route("_ResourceHeader")]
        [HttpGet]
        public ActionResult _ResourceHeader() {
            return PartialView(_messageService.Dispatch(new GetResourceHeaderQuery(_authenticationService.Identity)));
        }

        [Route("ResourceBuildings")]
        [HttpGet]
        public ActionResult ResourceBuildings() {
            return View();
        }

        [Route("_Building")]
        [HttpGet]
        public ActionResult _Building(string buildingType) {
            return PartialView(_messageService.Dispatch(new GetBuildingQuery(_authenticationService.Identity, buildingType)));
        }

        [Route("_Building")]
        [HttpPost]
        public ActionResult _Building(BuildingModel model) {
            return ValidatedCommandResult(model,
                new UpgradeBuildingCommand(_authenticationService.Identity, model.BuildingType),
                () => _Building(model.BuildingType));
        }

        [Route("_BuildingTotals")]
        public ActionResult _BuildingTotals() {
            return PartialView(_messageService.Dispatch(new GetBuildingTotalsQuery(_authenticationService.Identity)));
        }

        [Route("TroopBuildings")]
        [HttpGet]
        public ActionResult TroopBuildings() {
            return View();
        }

        [Route("EmpireBuildings")]
        [HttpGet]
        public ActionResult EmpireBuildings() {
            return View();
        }

        [Route("_HousingTotals")]
        public ActionResult _HousingTotals() {
            return PartialView(_messageService.Dispatch(new GetHousingTotalsQuery(_authenticationService.Identity)));
        }

        [Route("BuildingUpgrades")]
        [HttpGet]
        public ActionResult BuildingUpgrades(string buildingType) {
            return View(_messageService.Dispatch(new GetBuildingUpgradesQuery(_authenticationService.Identity, buildingType)));
        }

        [Route("BankBuildings")]
        [HttpGet]
        public ActionResult BankBuildings() {
            return View();
        }

        [Route("Banking")]
        [HttpGet]
        public ActionResult Banking() {
            return View(_messageService.Dispatch(new GetBankedResourcesQuery(_authenticationService.Identity)));
        }

        [Route("Banking")]
        [HttpPost]
        public ActionResult Banking(BankedResourcesViewModel model) {
            return ValidatedCommandResult(model,
                new BankCommand(_authenticationService.Identity),
                () => Banking());
        }

        [Route("SpecialtyBuildings")]
        [HttpGet]
        public ActionResult SpecialtyBuildings() {
            return View();
        }

        [Route("Siege")]
        [HttpGet]
        public ActionResult Siege() {
            return View(_messageService.Dispatch(new GetSiegeQuery(_authenticationService.Identity)));
        }

        [Route("Siege")]
        [HttpPost]
        public ActionResult Siege(SiegeModel model) {
            switch (model.Command) {
                case "build":
                    return ValidatedCommandResult(model,
                        new BuildSiegeCommand(_authenticationService.Identity, model.SiegeWeapons.Select(d => new SiegeWeaponInfo(d.Type, d.Count))),
                        () => Siege());
                case "discard":
                    return ValidatedCommandResult(model,
                        new DiscardSiegeCommand(_authenticationService.Identity, model.SiegeWeapons.Select(d => new SiegeWeaponInfo(d.Type, d.Count))),
                        () => Siege());
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }
    }
}