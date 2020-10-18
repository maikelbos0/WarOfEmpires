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
        public ViewResult Tax() {
            return View(_messageService.Dispatch(new GetTaxQuery(_authenticationService.Identity)));
        }

        [Route("Tax")]
        [HttpPost]
        public ViewResult Tax(TaxModel model) {
            return BuildViewResultFor(new SetTaxCommand(_authenticationService.Identity, model.Tax))
                .OnSuccess(Tax)
                .OnFailure("Tax", model)
                .Execute();
        }

        [Route("_ResourceHeader")]
        [HttpGet]
        public PartialViewResult _ResourceHeader() {
            return PartialView(_messageService.Dispatch(new GetResourceHeaderQuery(_authenticationService.Identity)));
        }

        [Route("ResourceBuildings")]
        [HttpGet]
        public ViewResult ResourceBuildings() {
            return View();
        }

        [Route("TroopBuildings")]
        [HttpGet]
        public ViewResult TroopBuildings() {
            return View();
        }

        [Route("EmpireBuildings")]
        [HttpGet]
        public ViewResult EmpireBuildings() {
            return View();
        }

        [Route("BankBuildings")]
        [HttpGet]
        public ViewResult BankBuildings() {
            return View();
        }

        [Route("SpecialtyBuildings")]
        [HttpGet]
        public ViewResult SpecialtyBuildings() {
            return View();
        }

        [Route("_Building")]
        [HttpGet]
        public PartialViewResult _Building(string buildingType) {
            return PartialView(_messageService.Dispatch(new GetBuildingQuery(_authenticationService.Identity, buildingType)));
        }

        [Route("_Building")]
        [HttpPost]
        public PartialViewResult _Building(BuildingModel model) {
            return BuildPartialViewResultFor(new UpgradeBuildingCommand(_authenticationService.Identity, model.BuildingType))
                .OnSuccess(() => _Building(model.BuildingType))
                .OnFailure("_Building", model)
                .Execute();
        }

        [Route("_BuildingTotals")]
        public PartialViewResult _BuildingTotals() {
            return PartialView(_messageService.Dispatch(new GetBuildingTotalsQuery(_authenticationService.Identity)));
        }

        [Route("_HousingTotals")]
        public PartialViewResult _HousingTotals() {
            return PartialView(_messageService.Dispatch(new GetHousingTotalsQuery(_authenticationService.Identity)));
        }

        [Route("BuildingUpgrades")]
        [HttpGet]
        public ViewResult BuildingUpgrades(string buildingType) {
            return View(_messageService.Dispatch(new GetBuildingUpgradesQuery(_authenticationService.Identity, buildingType)));
        }

        [Route("Banking")]
        [HttpGet]
        public ViewResult Banking() {
            return View(_messageService.Dispatch(new GetBankedResourcesQuery(_authenticationService.Identity)));
        }

        [Route("Banking")]
        [HttpPost]
        public ViewResult Banking(BankedResourcesViewModel model) {
            return BuildViewResultFor(new BankCommand(_authenticationService.Identity))
                .OnSuccess(Banking)
                .OnFailure("Banking", model)
                .Execute();
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