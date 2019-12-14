using System.Web.Mvc;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Attack")]
    public class AttackController : BaseController {
        public AttackController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View(_messageService.Dispatch(new GetExecutedAttacksQuery(_authenticationService.Identity)));
        }

        [Route("ReceivedIndex")]
        [HttpGet]
        public ActionResult ReceivedIndex() {
            return View(_messageService.Dispatch(new GetReceivedAttacksQuery(_authenticationService.Identity)));
        }

        [Route("Details")]
        [HttpGet]
        public ActionResult Details(string id) {
            return View(_messageService.Dispatch(new GetAttackDetailsQuery(_authenticationService.Identity, id)));
        }

        [Route("Execute")]
        [HttpGet]
        public ActionResult Execute(string defenderId) {
            return View(_messageService.Dispatch(new GetDefenderQuery(defenderId)));
        }

        [Route("Execute")]
        [HttpPost]
        public ActionResult Execute(ExecuteAttackModel model) {
            return ValidatedCommandResult(model, new AttackCommand(_authenticationService.Identity, model.DefenderId.ToString(), model.Turns), (id) => Details(id.ToString()));
        }
    }
}