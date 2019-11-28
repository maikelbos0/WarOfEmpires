﻿using System;
using System.Web.Mvc;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Empire")]
    public class EmpireController : BaseController {
        public EmpireController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route("Workers")]
        [HttpGet]
        public ActionResult Workers() {
            return View(_messageService.Dispatch(new GetWorkersQuery(_authenticationService.Identity)));
        }

        [Route("Workers")]
        [HttpPost]
        public ActionResult Workers(WorkerModel model) {
            switch (model.Command) {
                case "train":
                    return ValidatedCommandResult(model,
                        new TrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners),
                        () => Workers());
                case "untrain":
                    return ValidatedCommandResult(model,
                        new UntrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners),
                        () => Workers());
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

        [Route("_Resources")]
        [HttpGet]
        public ActionResult _Resources() {
            return PartialView(_messageService.Dispatch(new GetResourcesQuery(_authenticationService.Identity)));
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
        public ActionResult _Building(BuildingViewModel model) {
            return ValidatedCommandResult(model,
                new UpgradeBuildingCommand(_authenticationService.Identity, model.BuildingType),
                () => _Building(model.BuildingType));
        }
    }
}