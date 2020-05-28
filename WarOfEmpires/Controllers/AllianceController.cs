using System.Web.Mvc;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Alliance")]
    public class AllianceController : BaseController {
        public AllianceController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [Route]
        [Route("Index")]
        public ActionResult Index() {
            return View(new AllianceSearchModel());
        }

        [Route("GetAlliances")]
        [HttpPost]
        public ActionResult GetAlliances(DataGridViewMetaData metaData, AllianceSearchModel search) {
            return GridJson(new GetAlliancesQuery(search.Code, search.Name), metaData);
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
            return View("Home", _messageService.Dispatch(new GetAllianceHomeQuery(_authenticationService.Identity)));
        }

        [HttpGet]
        [Route("Details")]
        public ActionResult Details(string id) {
            return View(_messageService.Dispatch(new GetAllianceDetailsQuery(id)));
        }

        [HttpGet]
        [Route("Invite")]
        public ActionResult Invite(string playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [HttpPost]
        [Route("Invite")]
        public ActionResult Invite(SendInviteModel model) {
            return ValidatedCommandResult(model, new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Message), () => Invites());
        }

        [HttpGet]
        [Route("Invites")]
        public ActionResult Invites() {
            // Explicitly name view so it works from Invite
            return View("Invites", _messageService.Dispatch(new GetInvitesQuery(_authenticationService.Identity)));
        }

        [HttpPost]
        [Route("WithdrawInvite")]
        public ActionResult WithdrawInvite(string id) {
            _messageService.Dispatch(new WithdrawInviteCommand(_authenticationService.Identity, id));

            return RedirectToAction("Invites");
        }

        [HttpGet]
        [Route("ReceivedInvites")]
        public ActionResult ReceivedInvites() {
            _messageService.Dispatch(new ReadInvitesCommand(_authenticationService.Identity));

            return View(_messageService.Dispatch(new GetReceivedInvitesQuery(_authenticationService.Identity)));
        }

        [HttpPost]
        [Route("AcceptInvite")]
        public ActionResult AcceptInvite(string id) {
            _messageService.Dispatch(new AcceptInviteCommand(_authenticationService.Identity, id));

            return RedirectToAction("Home");
        }

        [HttpPost]
        [Route("RejectInvite")]
        public ActionResult RejectInvite(string id) {
            _messageService.Dispatch(new WithdrawInviteCommand(_authenticationService.Identity, id));

            return RedirectToAction("Invites");
        }
    }
}