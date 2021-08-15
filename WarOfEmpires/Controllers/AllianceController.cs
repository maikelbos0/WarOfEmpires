using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Alliance")]
    public class AllianceController : BaseController {
        public AllianceController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            // Explicitly name view so it works from other actions
            return View(nameof(Index), new AllianceSearchModel());
        }

        [HttpPost(nameof(GetAlliances))]
        public JsonResult GetAlliances(DataGridViewMetaData metaData, AllianceSearchModel search) {
            return GridJson(new GetAlliancesQuery(_authenticationService.Identity, search.Code, search.Name), metaData);
        }

        [HttpGet(nameof(Details))]
        public ViewResult Details(int id) {
            return View(_messageService.Dispatch(new GetAllianceDetailsQuery(_authenticationService.Identity, id)));
        }

        [HttpGet(nameof(Create))]
        public ViewResult Create() {
            return View(new CreateAllianceModel());
        }

        [HttpPost(nameof(Create))]
        public ViewResult Create(CreateAllianceModel model) {
            return BuildViewResultFor(new CreateAllianceCommand(_authenticationService.Identity, model.Code, model.Name))
                .OnSuccess(Home)
                .OnFailure(nameof(Create), model);
        }

        [AllianceAuthorize]
        [HttpGet(nameof(Home))]
        public ViewResult Home() {
            _messageService.Dispatch(new ReadNewChatMessagesCommand(_authenticationService.Identity));

            // Explicitly name view so it works from other actions
            return View(nameof(Home), _messageService.Dispatch(new GetAllianceHomeQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet(nameof(Invite))]
        public ViewResult Invite(int playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost(nameof(Invite))]
        public ViewResult Invite(SendInviteModel model) {
            return BuildViewResultFor(new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Subject, model.Body))
                .OnSuccess(Invites)
                .OnFailure(nameof(Invite), model);
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet(nameof(Invites))]
        public ViewResult Invites() {
            // Explicitly name view so it works from other actions
            return View(nameof(Invites), (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost(nameof(GetInvites))]
        public JsonResult GetInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetInvitesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet(nameof(InviteDetails))]
        public ViewResult InviteDetails(int id) {
            return View(_messageService.Dispatch(new GetInviteQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost(nameof(WithdrawInvite))]
        public ViewResult WithdrawInvite(int id) {
            return BuildViewResultFor(new WithdrawInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(Invites)
                .ThrowOnFailure();
        }

        [HttpGet(nameof(ReceivedInvites))]
        public ViewResult ReceivedInvites() {
            // Explicitly name view so it works from other actions
            return View(nameof(ReceivedInvites));
        }

        [HttpPost(nameof(GetReceivedInvites))]
        public JsonResult GetReceivedInvites(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedInvitesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(ReceivedInviteDetails))]
        public ViewResult ReceivedInviteDetails(int id) {
            var model = _messageService.Dispatch(new GetReceivedInviteQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadInviteCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpPost(nameof(AcceptInvite))]
        public ViewResult AcceptInvite(ReceivedInviteDetailsViewModel model) {
            return BuildViewResultFor(new AcceptInviteCommand(_authenticationService.Identity, model.Id))
                .OnSuccess(Home)
                .OnFailure(nameof(ReceivedInviteDetails), model);
        }

        [HttpPost(nameof(RejectInvite))]
        public ViewResult RejectInvite(int id) {
            return BuildViewResultFor(new RejectInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(ReceivedInvites)
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpPost(nameof(PostChatMessage))]
        public ViewResult PostChatMessage(AllianceHomeViewModel model) {
            return BuildViewResultFor(new PostChatMessageCommand(_authenticationService.Identity, model.ChatMessage))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDeleteChatMessages = true)]
        [HttpPost(nameof(DeleteChatMessage))]
        public ViewResult DeleteChatMessage(int id) {
            return BuildViewResultFor(new DeleteChatMessageCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(Roles))]
        public ViewResult Roles() {
            // Explicitly name view so it works from other actions
            return View(nameof(Roles), (object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(GetRoles))]
        public JsonResult GetRoles(DataGridViewMetaData metaData) {
            return GridJson(new GetRolesQuery(_authenticationService.Identity), metaData);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(CreateRole))]
        public ViewResult CreateRole() {
            return View(new CreateRoleModel());
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(CreateRole))]
        public ViewResult CreateRole(CreateRoleModel model) {
            return BuildViewResultFor(new CreateRoleCommand(_authenticationService.Identity, model.Name, model.CanInvite, model.CanManageRoles, model.CanDeleteChatMessages, model.CanKickMembers, model.CanManageNonAggressionPacts, model.CanManageWars))
                .OnSuccess(Roles)
                .OnFailure(nameof(CreateRole), model);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(RoleDetails))]
        public ViewResult RoleDetails(int id) {
            // Explicitly name view so it works from other actions
            return View(nameof(RoleDetails), _messageService.Dispatch(new GetRoleDetailsQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(ClearRole))]
        public ViewResult ClearRole(int id, int playerId) {
            return BuildViewResultFor(new ClearRoleCommand(_authenticationService.Identity, playerId))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(DeleteRole))]
        public ViewResult DeleteRole(int id) {
            return BuildViewResultFor(new DeleteRoleCommand(_authenticationService.Identity, id))
                .OnSuccess(Roles)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(SetRole))]
        public ViewResult SetRole(int id) {
            return View(_messageService.Dispatch(new GetNewRolePlayerQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(SetRole))]
        public ViewResult SetRole(int id, int playerId) {
            return BuildViewResultFor(new SetRoleCommand(_authenticationService.Identity, playerId, id))
                .OnSuccess(() => RoleDetails(id))
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpGet(nameof(Leave))]
        public ViewResult Leave() {
            return View();
        }

        [AllianceAuthorize]
        [HttpPost(nameof(Leave))]
        public ViewResult LeavePost() {
            return BuildViewResultFor(new LeaveAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanKickMembers = true)]
        [HttpPost(nameof(Kick))]
        public ViewResult Kick(string id) {
            return BuildViewResultFor(new KickFromAllianceCommand(_authenticationService.Identity, id))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpGet(nameof(TransferLeadership))]
        public ViewResult TransferLeadership() {
            return View(_messageService.Dispatch(new GetNewLeaderQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpPost(nameof(TransferLeadership))]
        public ViewResult TransferLeadership(int memberId) {
            return BuildViewResultFor(new TransferLeadershipCommand(_authenticationService.Identity, memberId))
                .OnSuccess(Home)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpGet(nameof(Disband))]
        public ViewResult Disband() {
            return View();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpPost(nameof(Disband))]
        public ViewResult DisbandPost() {
            return BuildViewResultFor(new DisbandAllianceCommand(_authenticationService.Identity))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(SendNonAggressionPactRequest))]
        public ViewResult SendNonAggressionPactRequest(int id) {
            return View(_messageService.Dispatch(new GetCreateNonAggressionPactRequestQuery(id)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(SendNonAggressionPactRequest))]
        public ViewResult SendNonAggressionPactRequest(CreateNonAggressionPactRequestModel model) {
            return BuildViewResultFor(new SendNonAggressionPactRequestCommand(_authenticationService.Identity, model.AllianceId))
                .OnSuccess(SentNonAggressionPactRequests)
                .OnFailure(nameof(SendNonAggressionPactRequest), model);
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(SentNonAggressionPactRequests))]
        public ViewResult SentNonAggressionPactRequests() {
            // Explicitly name view so it works from other actions
            return View(nameof(SentNonAggressionPactRequests), _messageService.Dispatch(new GetSentNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(WithdrawNonAggressionPactRequest))]
        public ViewResult WithdrawNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new WithdrawNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(SentNonAggressionPactRequests)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(ReceivedNonAggressionPactRequests))]
        public ViewResult ReceivedNonAggressionPactRequests() {
            // Explicitly name view so it works from other actions
            return View(nameof(ReceivedNonAggressionPactRequests), _messageService.Dispatch(new GetReceivedNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(AcceptNonAggressionPactRequest))]
        public ViewResult AcceptNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new AcceptNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(NonAggressionPacts)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(RejectNonAggressionPactRequest))]
        public ViewResult RejectNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new RejectNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(ReceivedNonAggressionPactRequests)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(NonAggressionPacts))]
        public ViewResult NonAggressionPacts() {
            // Explicitly name view so it works from other actions
            return View(nameof(NonAggressionPacts), _messageService.Dispatch(new GetNonAggressionPactsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(DissolveNonAggressionPact))]
        public ViewResult DissolveNonAggressionPact(int id) {
            return BuildViewResultFor(new DissolveNonAggressionPactCommand(_authenticationService.Identity, id))
                .OnSuccess(NonAggressionPacts)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpGet(nameof(Wars))]
        public ViewResult Wars() {
            // Explicitly name view so it works from other actions
            return View(nameof(Wars), _messageService.Dispatch(new GetWarsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpGet(nameof(DeclareWar))]
        public ViewResult DeclareWar(int id) {
            return View(_messageService.Dispatch(new GetDeclareWarQuery(id)));
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(DeclareWar))]
        public ViewResult DeclareWar(DeclareWarModel model) {
            return BuildViewResultFor(new DeclareWarCommand(_authenticationService.Identity, model.AllianceId))
                .OnSuccess(Wars)
                .OnFailure(nameof(DeclareWar), model);
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(CancelPeaceDeclaration))]
        public ViewResult CancelPeaceDeclaration(int id) {
            return BuildViewResultFor(new CancelPeaceDeclarationCommand(_authenticationService.Identity, id))
                .OnSuccess(Wars)
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(DeclarePeace))]
        public ViewResult DeclarePeace(int id) {
            return BuildViewResultFor(new DeclarePeaceCommand(_authenticationService.Identity, id))
                .OnSuccess(Wars)
                .ThrowOnFailure();
        }
    }
}