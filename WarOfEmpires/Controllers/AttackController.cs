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
        public ViewResult Index() {
            return View();
        }

        [Route("GetReceivedAttacks")]
        [HttpPost]
        public JsonResult GetReceivedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [Route("ExecutedIndex")]
        [HttpGet]
        public ViewResult ExecutedIndex() {
            return View();
        }
        
        [Route("GetExecutedAttacks")]
        [HttpPost]
        public JsonResult GetExecutedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetExecutedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [Route("Details")]
        [HttpGet]
        public ViewResult Details(int id) {
            var model = _messageService.Dispatch(new GetAttackDetailsQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadAttackCommand(_authenticationService.Identity, id));
            }

            // Explicitly name view so it works from other actions
            return View("Details", model);
        }

        [Route("LastExecutedAttackDetails")]
        [HttpGet]
        public ViewResult LastExecutedAttackDetails() {
            var id = _messageService.Dispatch(new GetLastExecutedAttackQuery(_authenticationService.Identity));

            return Details(id);
        }

        [Route("Execute")]
        [HttpGet]
        public ViewResult Execute(int defenderId) {
            return View(_messageService.Dispatch(new GetDefenderQuery(defenderId)));
        }

        [Route("Execute")]
        [HttpPost]
        public ViewResult Execute(ExecuteAttackModel model) {
            return BuildViewResultFor(new AttackCommand(model.AttackType, _authenticationService.Identity, model.DefenderId, model.Turns))
                .OnSuccess(LastExecutedAttackDetails)
                .OnFailure("Execute", model);
        }
    }
}