using System;
using System.Web.Mvc;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Models.Empires;
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
            return View(new WorkerModel());
        }

        [Route("Workers")]
        [HttpPost]
        public ActionResult Workers(WorkerModel model) {
            switch (model.Command) {
                case "train":
                    return ValidatedCommandResult(model,
                        new TrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners),
                        "Workers");
                case "untrain":
                    return ValidatedCommandResult(model,
                        new UntrainWorkersCommand(_authenticationService.Identity, model.Farmers, model.WoodWorkers, model.StoneMasons, model.OreMiners),
                        "Workers");
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");

            }
        }
    }
}