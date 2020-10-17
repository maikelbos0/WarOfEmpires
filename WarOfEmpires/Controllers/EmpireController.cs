using System;
using System.Linq;
using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [RoutePrefix("Empire")]
    public class EmpireController : BaseController {
        public EmpireController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("Workers")]
        [HttpGet]
        public ViewResult Workers() {
            return View(_messageService.Dispatch(new GetWorkersQuery(_authenticationService.Identity)));
        }

        [Route("Workers")]
        [HttpPost]
        public ViewResult Workers(WorkersModel model) {
            switch (model.Command) {
                case "train":
                    return BuildViewResultFor(new TrainWorkersCommand(_authenticationService.Identity, model.Workers.Select(w => new WorkerInfo(w.Type, w.Count))))
                        .OnSuccess(Workers)
                        .OnFailure("Workers", model)
                        .Execute();
                case "untrain":
                    return BuildViewResultFor(new UntrainWorkersCommand(_authenticationService.Identity, model.Workers.Select(w => new WorkerInfo(w.Type, w.Count))))
                        .OnSuccess(Workers)
                        .OnFailure("Workers", model)
                        .Execute();
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }

        [Route("Troops")]
        [HttpGet]
        public ViewResult Troops() {
            return View(_messageService.Dispatch(new GetTroopsQuery(_authenticationService.Identity)));
        }

        [Route("Troops")]
        [HttpPost]
        public ViewResult Troops(TroopsModel model) {
            switch (model.Command) {
                case "train":
                    return BuildViewResultFor(new TrainTroopsCommand(_authenticationService.Identity, model.Troops.Select(t => new TroopInfo(t.Type, t.Soldiers, t.Mercenaries))))
                        .OnSuccess(Troops)
                        .OnFailure("Troops", model)
                        .Execute();
                case "untrain":
                    return BuildViewResultFor(new UntrainTroopsCommand(_authenticationService.Identity, model.Troops.Select(t => new TroopInfo(t.Type, t.Soldiers, t.Mercenaries))))
                        .OnSuccess(Troops)
                        .OnFailure("Troops", model)
                        .Execute();
                case "heal":
                    return BuildViewResultFor(new HealTroopsCommand(_authenticationService.Identity, model.StaminaToHeal))
                        .OnSuccess(Troops)
                        .OnFailure("Troops", model)
                        .Execute();
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