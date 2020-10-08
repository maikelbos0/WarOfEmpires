using System.Web.Mvc;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Services;
using WarOfEmpires.Attributes;
using System;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
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

        [AllianceAuthorize]
        [HttpGet]
        [Route("Home")]
        public ActionResult Home() {
            // Explicitly name view so it works from Create, PostChatMessage, DeleteChatMessage and ReceivedInviteDetails
            return View("Home", _messageService.Dispatch(new GetAllianceHomeQuery(_authenticationService.Identity)));
        }

        [HttpGet]
        [Route("Details")]
        public ActionResult Details(string id) {
            return View(_messageService.Dispatch(new GetAllianceDetailsQuery(id)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("Invite")]
        public ActionResult Invite(string playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("Invite")]
        public ActionResult Invite(SendInviteModel model) {
            return ValidatedCommandResult(model, new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Subject, model.Body), () => Invites());
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("Invites")]
        public ActionResult Invites() {
            // Explicitly name view so it works from Invite
            return View("Invites", (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [Route("GetInvites")]
        [HttpPost]
        public ActionResult GetInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetInvitesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("InviteDetails")]
        public ActionResult InviteDetails(string id) {
            return View(_messageService.Dispatch(new GetInviteQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("WithdrawInvite")]
        public ActionResult WithdrawInvite(string id) {
            _messageService.Dispatch(new WithdrawInviteCommand(_authenticationService.Identity, id));

            return RedirectToAction("Invites");
        }

        [HttpGet]
        [Route("ReceivedInvites")]
        public ActionResult ReceivedInvites() {
            // Explicitly name view so it works from ReceivedInviteDetails
            return View("ReceivedInvites");
        }

        [HttpPost]
        [Route("GetReceivedInvites")]
        public ActionResult GetReceivedInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedInvitesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet]
        [Route("ReceivedInviteDetails")]
        public ActionResult ReceivedInviteDetails(string id) {
            var model = _messageService.Dispatch(new GetReceivedInviteQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadInviteCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpPost]
        [Route("ReceivedInviteDetails")]
        public ActionResult ReceivedInviteDetails(ReceivedInviteDetailsViewModel model) {
            switch (model.Command) {
                case "accept":
                    return ValidatedCommandResult(model,
                        new AcceptInviteCommand(_authenticationService.Identity, model.Id.ToString()),
                        Home);
                case "reject":
                    return ValidatedCommandResult(model,
                        new RejectInviteCommand(_authenticationService.Identity, model.Id.ToString()),
                        ReceivedInvites);
                default:
                    throw new InvalidOperationException($"Invalid operation '{model.Command}' found");
            }
        }

        [AllianceAuthorize]
        [HttpPost]
        [Route("PostChatMessage")]
        public ActionResult PostChatMessage(AllianceHomeViewModel model) {
            return ValidatedCommandResult(model, new PostChatMessageCommand(_authenticationService.Identity, model.ChatMessage), Home);
        }

        [AllianceAuthorize] // TODO add can delete chat messages
        [HttpPost]
        [Route("DeleteChatMessage")]
        public ActionResult DeleteChatMessage(string id) {
            return ValidatedCommandResult(id, new DeleteChatMessageCommand(_authenticationService.Identity, id), Home);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("Roles")]
        public ActionResult Roles() {
            // Explicitly name view so it works from CreateRole
            return View("Roles", (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("GetRoles")]
        public ActionResult GetRoles(DataGridViewMetaData metaData) {
            return GridJson(new GetRolesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("CreateRole")]
        public ActionResult CreateRole() {
            return View(new CreateRoleModel());
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("CreateRole")]
        public ActionResult CreateRole(CreateRoleModel model) {
            return ValidatedCommandResult(model, new CreateRoleCommand(_authenticationService.Identity, model.Name, model.CanInvite, model.CanManageRoles, model.CanDeleteChatMessages), Roles);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("RoleDetails")]
        public ActionResult RoleDetails(string id) {
            return View(_messageService.Dispatch(new GetRoleDetailsQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("ClearRole")]
        public ActionResult ClearRole(string id, string playerId) {
            _messageService.Dispatch(new ClearRoleCommand(_authenticationService.Identity, playerId));

            return RedirectToAction("RoleDetails", new { id });
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("DeleteRole")]
        public ActionResult DeleteRole(string id) {
            _messageService.Dispatch(new DeleteRoleCommand(_authenticationService.Identity, id));

            return RedirectToAction("Roles");
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("SetRole")]
        public ActionResult SetRole(string id) {
            return View(_messageService.Dispatch(new GetNewRolePlayerQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("SetRole")]
        public ActionResult SetRole(string id, string playerId) {
            _messageService.Dispatch(new SetRoleCommand(_authenticationService.Identity, playerId, id));

            return RedirectToAction("RoleDetails", new { id });
        }
    }
}