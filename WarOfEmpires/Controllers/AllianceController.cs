using System.Web.Mvc;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Alliance")]
    public class AllianceController : BaseController {
        public AllianceController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [Route("Create")]
        public ActionResult Create() {
            return View(new CreateAllianceModel());
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create(CreateAllianceModel model) {
            return ValidatedCommandResult(model, new CreateAllianceCommand(_authenticationService.Identity, model.Code, model.Name), () => Home());
        }

        [HttpGet]
        [Route("Home")]
        public ActionResult Home() {
            // Explicitly name view so it works from Create
            return View("Home");
        }
    }
}