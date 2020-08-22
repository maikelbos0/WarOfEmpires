using System.Web.Mvc;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Services;
using WarOfEmpires.Attributes;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [RoutePrefix("Attack")]
    public class AttackController : BaseController {
        public AttackController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [Route("GetReceivedAttacks")]
        [HttpPost]
        public ActionResult GetReceivedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [Route("ExecutedIndex")]
        [HttpGet]
        public ActionResult ExecutedIndex() {
            return View();
        }
        
        [Route("GetExecutedAttacks")]
        [HttpPost]
        public ActionResult GetExecutedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetExecutedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [Route("Details")]
        [HttpGet]
        public ActionResult Details(string id) {
            var model = _messageService.Dispatch(new GetAttackDetailsQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadAttackCommand(_authenticationService.Identity, id));
            }

            // Explicitly name view so it works from Execute
            return View("Details", model);
        }

        [Route("Execute")]
        [HttpGet]
        public ActionResult Execute(string defenderId) {
            return View(_messageService.Dispatch(new GetDefenderQuery(defenderId)));
        }

        [Route("Execute")]
        [HttpPost]
        public ActionResult Execute(ExecuteAttackModel model) {
            return ValidatedCommandResult2(model, 
                new AttackCommand(model.AttackType, _authenticationService.Identity, model.DefenderId.ToString(), model.Turns),
                "Details");
        }
    }
}