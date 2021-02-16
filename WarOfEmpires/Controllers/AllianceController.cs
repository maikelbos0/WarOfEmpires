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
            // Explicitly name view so it works from other actions
            return View("Index", new AllianceSearchModel());
        }

        [Route("GetAlliances")]
        [HttpPost]
        public JsonResult GetAlliances(DataGridViewMetaData metaData, AllianceSearchModel search) {
            return GridJson(new GetAlliancesQuery(search.Code, search.Name), metaData);
        }

        [HttpGet]
        [Route("Details")]
        public ViewResult Details(int id) {
            return View(_messageService.Dispatch(new GetAllianceDetailsQuery(_authenticationService.Identity, id)));
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
                .OnFailure("Create", model);
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
        public ViewResult Invite(int playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("Invite")]
        public ViewResult Invite(SendInviteModel model) {
            return BuildViewResultFor(new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Subject, model.Body))
                .OnSuccess(Invites)
                .OnFailure("Invite", model);
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
        public ViewResult InviteDetails(int id) {
            return View(_messageService.Dispatch(new GetInviteQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost]
        [Route("WithdrawInvite")]
        public ViewResult WithdrawInvite(int id) {
            return BuildViewResultFor(new WithdrawInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(Invites)
                .ThrowOnFailure();
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
        public ViewResult ReceivedInviteDetails(int id) {
            var model = _messageService.Dispatch(new GetReceivedInviteQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadInviteCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpPost]
        [Route("AcceptInvite")]
        public ViewResult AcceptInvite(ReceivedInviteDetailsViewModel model) {
            return BuildViewResultFor(new AcceptInviteCommand(_authenticationService.Identity, model.Id))
                .OnSuccess(Home)
                .OnFailure("ReceivedInviteDetails", model);
        }

        [HttpPost]
        [Route("RejectInvite")]
        public ViewResult RejectInvite(int id) {
            return BuildViewResultFor(new RejectInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(ReceivedInvites)
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpPost]
        [Route("PostChatMessage")]
        public ViewResult PostChatMessage(AllianceHomeViewModel model) {
            return BuildViewResultFor(new PostChatMessageCommand(_authenticationService.Identity, model.ChatMessage))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDeleteChatMessages = true)]
        [HttpPost]
        [Route("DeleteChatMessage")]
        public ViewResult DeleteChatMessage(int id) {
            return BuildViewResultFor(new DeleteChatMessageCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure();
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
            return BuildViewResultFor(new CreateRoleCommand(_authenticationService.Identity, model.Name, model.CanInvite, model.CanManageRoles, model.CanDeleteChatMessages, model.CanKickMembers, model.CanManageNonAggressionPacts))
                .OnSuccess(Roles)
                .OnFailure("CreateRole", model);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("RoleDetails")]
        public ViewResult RoleDetails(int id) {
            // Explicitly name view so it works from other actions
            return View("RoleDetails", _messageService.Dispatch(new GetRoleDetailsQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("ClearRole")]
        public ViewResult ClearRole(int id, int playerId) {
            return BuildViewResultFor(new ClearRoleCommand(_authenticationService.Identity, playerId))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("DeleteRole")]
        public ViewResult DeleteRole(int id) {
            return BuildViewResultFor(new DeleteRoleCommand(_authenticationService.Identity, id))
                .OnSuccess(Roles)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet]
        [Route("SetRole")]
        public ViewResult SetRole(int id) {
            return View(_messageService.Dispatch(new GetNewRolePlayerQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost]
        [Route("SetRole")]
        public ViewResult SetRole(int id, int playerId) {
            return BuildViewResultFor(new SetRoleCommand(_authenticationService.Identity, playerId, id))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpPost]
        [Route("LeaveAlliance")]
        public ViewResult LeaveAlliance() {
            return BuildViewResultFor(new LeaveAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanKickMembers = true)]
        [HttpPost]
        [Route("KickFromAlliance")]
        public ViewResult KickFromAlliance(string id) {
            return BuildViewResultFor(new KickFromAllianceCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpGet]
        [Route("TransferLeadership")]
        public ViewResult TransferLeadership() {
            return View(_messageService.Dispatch(new GetNewLeaderQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpPost]
        [Route("TransferLeadership")]
        public ViewResult TransferLeadership(int memberId) {
            return BuildViewResultFor(new TransferLeadershipCommand(_authenticationService.Identity, memberId))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpPost]
        [Route("Disband")]
        public ViewResult Disband() {
            return BuildViewResultFor(new DisbandAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpGet]
        [Route("SendNonAggressionPactRequest")]
        public ViewResult SendNonAggressionPactRequest(int id) {
            return View(_messageService.Dispatch(new GetCreateNonAggressionPactRequestQuery(id)));
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpPost]
        [Route("SendNonAggressionPactRequest")]
        public ViewResult SendNonAggressionPactRequest(CreateNonAggressionPactRequestModel model) {
            return BuildViewResultFor(new SendNonAggressionPactRequestCommand(_authenticationService.Identity, model.AllianceId))
                .OnSuccess(SentNonAggressionPactRequests)
                .OnFailure("SendNonAggressionPactRequest", model);
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpGet]
        [Route("SentNonAggressionPactRequests")]
        public ViewResult SentNonAggressionPactRequests() {
            // Explicitly name view so it works from other actions
            return View("SentNonAggressionPactRequests", _messageService.Dispatch(new GetSentNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpPost]
        [Route("WithdrawNonAggressionPactRequest")]
        public ViewResult WithdrawNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new WithdrawNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(SentNonAggressionPactRequests)
                .ThrowOnFailure();
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpGet]
        [Route("ReceivedNonAggressionPactRequests")]
        public ViewResult ReceivedNonAggressionPactRequests() {
            // Explicitly name view so it works from other actions
            return View("ReceivedNonAggressionPactRequests", _messageService.Dispatch(new GetReceivedNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpPost]
        [Route("AcceptNonAggressionPactRequest")]
        public ViewResult AcceptNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new AcceptNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(NonAggressionPacts)
                .ThrowOnFailure();
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpPost]
        [Route("RejectNonAggressionPactRequest")]
        public ViewResult RejectNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new RejectNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(ReceivedNonAggressionPactRequests)
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpGet]
        [Route("NonAggressionPacts")]
        public ViewResult NonAggressionPacts() {
            // Explicitly name view so it works from other actions
            return View("NonAggressionPacts", _messageService.Dispatch(new GetNonAggressionPactsQuery(_authenticationService.Identity)));
        }

        // TODO additional authorization
        [AllianceAuthorize]
        [HttpPost]
        [Route("DissolveNonAggressionPact")]
        public ViewResult DissolveNonAggressionPact(int id) {
            return BuildViewResultFor(new DissolveNonAggressionPactCommand(_authenticationService.Identity, id))
                .OnSuccess(NonAggressionPacts)
                .ThrowOnFailure();
        }
    }
}