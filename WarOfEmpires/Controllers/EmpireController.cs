using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Empire")]
    public class EmpireController : BaseController {
        public EmpireController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet(nameof(Workers))]
        public ViewResult Workers() {
            return View(_messageService.Dispatch(new GetWorkersQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Workers))]
        public ViewResult Workers(WorkersModel model) {
            switch (model.Command) {
                case "train":
                    return BuildViewResultFor(new TrainWorkersCommand(_authenticationService.Identity, model.Workers.Select(w => new WorkerInfo(w.Type, w.Count))))
                        .OnSuccess(Workers)
                        .OnFailure(nameof(Workers), model);
                case "untrain":
                    return BuildViewResultFor(new UntrainWorkersCommand(_authenticationService.Identity, model.Workers.Select(w => new WorkerInfo(w.Type, w.Count))))
                        .OnSuccess(Workers)
                        .OnFailure(nameof(Workers), model);
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }

        [HttpGet(nameof(Troops))]
        public ViewResult Troops() {
            return View(_messageService.Dispatch(new GetTroopsQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Troops))]
        public ViewResult Troops(TroopsModel model) {
            switch (model.Command) {
                case "train":
                    return BuildViewResultFor(new TrainTroopsCommand(_authenticationService.Identity, model.Troops.Select(t => new TroopInfo(t.Type, t.Soldiers, t.Mercenaries))))
                        .OnSuccess(Troops)
                        .OnFailure(nameof(Troops), model);
                case "untrain":
                    return BuildViewResultFor(new UntrainTroopsCommand(_authenticationService.Identity, model.Troops.Select(t => new TroopInfo(t.Type, t.Soldiers, t.Mercenaries))))
                        .OnSuccess(Troops)
                        .OnFailure(nameof(Troops), model);
                case "heal":
                    return BuildViewResultFor(new HealTroopsCommand(_authenticationService.Identity, model.StaminaToHeal))
                        .OnSuccess(Troops)
                        .OnFailure(nameof(Troops), model);
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }

        [HttpGet(nameof(Tax))]
        public ViewResult Tax() {
            return View(_messageService.Dispatch(new GetTaxQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Tax))]
        public ViewResult Tax(TaxModel model) {
            return BuildViewResultFor(new SetTaxCommand(_authenticationService.Identity, model.Tax))
                .OnSuccess(Tax)
                .OnFailure(nameof(Tax), model);
        }

        [HttpGet(nameof(_ResourceHeader))]
        public PartialViewResult _ResourceHeader() {
            return PartialView(_messageService.Dispatch(new GetResourceHeaderQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(ResourceBuildings))]
        public ViewResult ResourceBuildings() {
            return View();
        }

        [HttpGet(nameof(TroopBuildings))]
        public ViewResult TroopBuildings() {
            return View();
        }

        [HttpGet(nameof(EmpireBuildings))]
        public ViewResult EmpireBuildings() {
            return View();
        }

        [HttpGet(nameof(BankBuildings))]
        public ViewResult BankBuildings() {
            return View();
        }

        [HttpGet(nameof(SpecialtyBuildings))]
        public ViewResult SpecialtyBuildings() {
            return View();
        }

        [HttpGet(nameof(_Building))]
        public PartialViewResult _Building(string buildingType) {
            return PartialView(_messageService.Dispatch(new GetBuildingQuery(_authenticationService.Identity, buildingType)));
        }

        [HttpPost(nameof(_Building))]
        public PartialViewResult _Building(BuildingModel model) {
            return BuildPartialViewResultFor(new UpgradeBuildingCommand(_authenticationService.Identity, model.BuildingType))
                .OnSuccess(() => _Building(model.BuildingType))
                .OnFailure(nameof(_Building), model);
        }

        [HttpGet(nameof(_BuildingTotals))]
        public PartialViewResult _BuildingTotals() {
            return PartialView(_messageService.Dispatch(new GetBuildingTotalsQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(_HousingTotals))]
        public PartialViewResult _HousingTotals() {
            return PartialView(_messageService.Dispatch(new GetHousingTotalsQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(BuildingUpgrades))]
        public ViewResult BuildingUpgrades(string buildingType) {
            return View(_messageService.Dispatch(new GetBuildingUpgradesQuery(_authenticationService.Identity, buildingType)));
        }

        [HttpGet(nameof(Banking))]
        public ViewResult Banking() {
            return View(_messageService.Dispatch(new GetBankedResourcesQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Banking))]
        public ViewResult Banking(BankedResourcesViewModel model) {
            return BuildViewResultFor(new BankCommand(_authenticationService.Identity))
                .OnSuccess(Banking)
                .OnFailure(nameof(Banking), model);
        }

        [HttpGet(nameof(Siege))]
        public ViewResult Siege() {
            return View(_messageService.Dispatch(new GetSiegeQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Siege))]
        public ViewResult Siege(SiegeModel model) {
            switch (model.Command) {
                case "build":
                    return BuildViewResultFor(new BuildSiegeCommand(_authenticationService.Identity, model.SiegeWeapons.Select(d => new SiegeWeaponInfo(d.Type, d.Count))))
                        .OnSuccess(Siege)
                        .OnFailure(nameof(Siege), model);
                case "discard":
                    return BuildViewResultFor(new DiscardSiegeCommand(_authenticationService.Identity, model.SiegeWeapons.Select(d => new SiegeWeaponInfo(d.Type, d.Count))))
                        .OnSuccess(Siege)
                        .OnFailure(nameof(Siege), model);
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }
    }
}