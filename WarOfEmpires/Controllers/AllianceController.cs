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
        public ViewResult Index() {
            return View("Index", new AllianceSearchModel());
        }

        [Route("GetAlliances")]
        [HttpPost]
        public JsonResult GetAlliances(DataGridViewMetaData metaData, AllianceSearchModel search) {
            return GridJson(new GetAlliancesQuery(search.Code, search.Name), metaData);
        }

        [HttpGet]
        [Route("Details")]
        public ViewResult Details(string id) {
            return View(_messageService.Dispatch(new GetAllianceDetailsQuery(id)));
        }

        [HttpGet]
        [Route("Create")]
        public ViewResult Create() {
            return View(new CreateAllianceModel());
        }

        [HttpPost]
        [Route("Create")]
        public ViewResult Create(CreateAllianceModel model) {
            return BuildViewResultFor(new CreateAllianceCommand(_authenticationService.Identity, model.Code, model.Name))
                .OnSuccess(Home)
                .OnFailure("Create", model)
                .Execute();
        }

        [AllianceAuthorize]
        [HttpGet]
        [Route("Home")]
        public ViewResult Home() {
            // Explicitly name view so it works from other actions
            return View("Home", _messageService.Dispatch(new GetAllianceHomeQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("Invite")]
        public ViewResult Invite(string playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("Invite")]
        public ViewResult Invite(SendInviteModel model) {
            return BuildViewResultFor(new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Subject, model.Body))
                .OnSuccess(Invites)
                .OnFailure("Invite", model)
                .Execute();
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("Invites")]
        public ViewResult Invites() {
            // Explicitly name view so it works from other actions
            return View("Invites", (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [Route("GetInvites")]
        [HttpPost]
        public JsonResult GetInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetInvitesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet]
        [Route("InviteDetails")]
        public ViewResult InviteDetails(string id) {
            return View(_messageService.Dispatch(new GetInviteQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("WithdrawInvite")]
        public ViewResult WithdrawInvite(string id) {
            return BuildViewResultFor(new WithdrawInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(Invites)
                .ThrowOnFailure()
                .Execute();
        }

        [HttpGet]
        [Route("ReceivedInvites")]
        public ViewResult ReceivedInvites() {
            // Explicitly name view so it works from other actions
            return View("ReceivedInvites");
        }

        [HttpPost]
        [Route("GetReceivedInvites")]
        public JsonResult GetReceivedInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedInvitesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet]
        [Route("ReceivedInviteDetails")]
        public ViewResult ReceivedInviteDetails(string id) {
            var model = _messageService.Dispatch(new GetReceivedInviteQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadInviteCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpPost]
        [Route("AcceptInvite")]
        public ViewResult AcceptInvite(ReceivedInviteDetailsViewModel model) {
            return BuildViewResultFor(new AcceptInviteCommand(_authenticationService.Identity, model.Id.ToString()))
                .OnSuccess(Home)
                .OnFailure("ReceivedInviteDetails", model)
                .Execute();
        }

        [HttpPost]
        [Route("RejectInvite")]
        public ViewResult RejectInvite(string id) {
            return BuildViewResultFor(new RejectInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(ReceivedInvites)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize]
        [HttpPost]
        [Route("PostChatMessage")]
        public ViewResult PostChatMessage(AllianceHomeViewModel model) {
            return BuildViewResultFor(new PostChatMessageCommand(_authenticationService.Identity, model.ChatMessage))
                .OnSuccess(Home)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanDeleteChatMessages = true)]
        [HttpPost]
        [Route("DeleteChatMessage")]
        public ViewResult DeleteChatMessage(string id) {
            return BuildViewResultFor(new DeleteChatMessageCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("Roles")]
        public ViewResult Roles() {
            // Explicitly name view so it works from other actions
            return View("Roles", (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("GetRoles")]
        public JsonResult GetRoles(DataGridViewMetaData metaData) {
            return GridJson(new GetRolesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("CreateRole")]
        public ViewResult CreateRole() {
            return View(new CreateRoleModel());
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("CreateRole")]
        public ViewResult CreateRole(CreateRoleModel model) {
            return BuildViewResultFor(new CreateRoleCommand(_authenticationService.Identity, model.Name, model.CanInvite, model.CanManageRoles, model.CanDeleteChatMessages, model.CanKickMembers))
                .OnSuccess(Roles)
                .OnFailure("CreateRole", model)
                .Execute();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("RoleDetails")]
        public ViewResult RoleDetails(string id) {
            // Explicitly name view so it works from other actions
            return View("RoleDetails", _messageService.Dispatch(new GetRoleDetailsQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("ClearRole")]
        public ViewResult ClearRole(string id, string playerId) {
            return BuildViewResultFor(new ClearRoleCommand(_authenticationService.Identity, playerId))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("DeleteRole")]
        public ViewResult DeleteRole(string id) {
            return BuildViewResultFor(new DeleteRoleCommand(_authenticationService.Identity, id))
                .OnSuccess(Roles)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("SetRole")]
        public ViewResult SetRole(string id) {
            return View(_messageService.Dispatch(new GetNewRolePlayerQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("SetRole")]
        public ViewResult SetRole(string id, string playerId) {
            return BuildViewResultFor(new SetRoleCommand(_authenticationService.Identity, playerId, id))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize]
        [HttpPost]
        [Route("LeaveAlliance")]
        public ViewResult LeaveAlliance() {
            return BuildViewResultFor(new LeaveAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanKickMembers = true)]
        [HttpPost]
        [Route("KickFromAlliance")]
        public ViewResult KickFromAlliance(string id) {
            return BuildViewResultFor(new KickFromAllianceCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure()
                .Execute();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpPost]
        [Route("Disband")]
        public ViewResult Disband() {
            return BuildViewResultFor(new DisbandAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure()
                .Execute();
        }
    }
}