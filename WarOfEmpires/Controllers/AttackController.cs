using System.Collections.Generic;
using System.Web.Mvc;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Attack")]
    public class AttackController : BaseController {
        private readonly IDataGridViewService _dataGridViewService;

        public AttackController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) : base(messageService, authenticationService) {
            _dataGridViewService = dataGridViewService;
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
            IEnumerable<ReceivedAttackViewModel> data = _messageService.Dispatch(new GetReceivedAttacksQuery(_authenticationService.Identity));

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }

        [Route("ExecutedIndex")]
        [HttpGet]
        public ActionResult ExecutedIndex() {
            return View();
        }
        
        [Route("GetExecutedAttacks")]
        [HttpPost]
        public ActionResult GetExecutedAttacks(DataGridViewMetaData metaData) {
            IEnumerable<ExecutedAttackViewModel> data = _messageService.Dispatch(new GetExecutedAttacksQuery(_authenticationService.Identity));

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
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
            return ValidatedCommandResult(model, new AttackCommand(_authenticationService.Identity, model.DefenderId.ToString(), model.Turns), (id) => Details(id.ToString()));
        }
    }
}