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

        [Route("Execute")]
        [HttpGet]
        public ActionResult Execute(string id) {
            return View(_messageService.Dispatch(new GetDefenderQuery(id)));
        }

        [Route("Execute")]
        [HttpPost]
        public ActionResult Execute(ExecuteAttackModel model) {
            return ValidatedCommandResult(model, new AttackCommand(_authenticationService.Identity, model.DefenderId.ToString(), model.Turns), (id) => Content(id.ToString()));
        }
    }
}