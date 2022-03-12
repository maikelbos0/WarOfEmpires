using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [Route(Route)]
    public class AllianceController : BaseController {
        public const string Route = "Alliance";

        public AllianceController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View(new AllianceSearchModel());
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
        public ActionResult Create(CreateAllianceModel model) {
            return BuildViewResultFor(new CreateAllianceCommand(_authenticationService.Identity, model.Code, model.Name))
                .OnSuccess(nameof(Home))
                .OnFailure(model);
        }

        [AllianceAuthorize]
        [HttpGet(nameof(Home))]
        public ViewResult Home() {
            _messageService.Dispatch(new ReadNewChatMessagesCommand(_authenticationService.Identity));

            return View(_messageService.Dispatch(new GetAllianceHomeQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet(nameof(Invite))]
        public ViewResult Invite(int playerId) {
            return View(_messageService.Dispatch(new GetInvitePlayerQuery(playerId)));
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpPost(nameof(Invite))]
        public ActionResult Invite(SendInviteModel model) {
            return BuildViewResultFor(new SendInviteCommand(_authenticationService.Identity, model.PlayerId, model.Subject, model.Body))
                .OnSuccess(nameof(Invites))
                .OnFailure(model);
        }

        [AllianceAuthorize(CanInvite = true)]
        [HttpGet(nameof(Invites))]
        public ViewResult Invites() {
            return View((object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
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
        public ActionResult WithdrawInvite(int id) {
            return BuildViewResultFor(new WithdrawInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Invites))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(ReceivedInvites))]
        public ViewResult ReceivedInvites() {
            return View();
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
        public ActionResult AcceptInvite(ReceivedInviteDetailsViewModel model) {
            return BuildViewResultFor(new AcceptInviteCommand(_authenticationService.Identity, model.Id))
                .OnSuccess(nameof(Home))
                .OnFailure(nameof(ReceivedInviteDetails), model);
        }

        [HttpPost(nameof(RejectInvite))]
        public ActionResult RejectInvite(int id) {
            return BuildViewResultFor(new RejectInviteCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(ReceivedInvites))
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpPost(nameof(PostChatMessage))]
        public ActionResult PostChatMessage(AllianceHomeModel model) {
            return BuildViewResultFor(new PostChatMessageCommand(_authenticationService.Identity, model.ChatMessage))
                .OnSuccess(nameof(Home))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDeleteChatMessages = true)]
        [HttpPost(nameof(DeleteChatMessage))]
        public ActionResult DeleteChatMessage(int id) {
            return BuildViewResultFor(new DeleteChatMessageCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Home))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(Roles))]
        public ViewResult Roles() {
            return View((object)_messageService.Dispatch(new GetAllianceNameQuery(_authenticationService.Identity)));
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
        public ActionResult CreateRole(CreateRoleModel model) {
            return BuildViewResultFor(new CreateRoleCommand(_authenticationService.Identity, model.Name, model.CanInvite, model.CanManageRoles, model.CanDeleteChatMessages, model.CanKickMembers, model.CanManageNonAggressionPacts, model.CanManageWars, model.CanBank))
                .OnSuccess(nameof(Roles))
                .OnFailure(model);
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(RoleDetails))]
        public ViewResult RoleDetails(int id) {
            return View(_messageService.Dispatch(new GetRoleDetailsQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(ClearRole))]
        public ActionResult ClearRole(int id, int playerId) {
            return BuildViewResultFor(new ClearRoleCommand(_authenticationService.Identity, playerId))
                .OnSuccess(nameof(RoleDetails), new { id })
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(DeleteRole))]
        public ActionResult DeleteRole(int id) {
            return BuildViewResultFor(new DeleteRoleCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Roles))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpGet(nameof(SetRole))]
        public ViewResult SetRole(int id) {
            return View(_messageService.Dispatch(new GetNewRolePlayerQuery(_authenticationService.Identity, id)));
        }

        [AllianceAuthorize(CanManageRoles = true)]
        [HttpPost(nameof(SetRole))]
        public ActionResult SetRole(int id, int playerId) {
            return BuildViewResultFor(new SetRoleCommand(_authenticationService.Identity, playerId, id))
                .OnSuccess(nameof(RoleDetails), new { id })
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpGet(nameof(Leave))]
        public ViewResult Leave() {
            return View();
        }

        [AllianceAuthorize]
        [HttpPost(nameof(Leave))]
        public ActionResult LeavePost() {
            return BuildViewResultFor(new LeaveAllianceCommand(_authenticationService.Identity))
                .OnSuccess(nameof(Index))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanKickMembers = true)]
        [HttpPost(nameof(Kick))]
        public ActionResult Kick(string id) {
            return BuildViewResultFor(new KickFromAllianceCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Home))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpGet(nameof(TransferLeadership))]
        public ViewResult TransferLeadership() {
            return View(_messageService.Dispatch(new GetNewLeaderQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanTransferLeadership = true)]
        [HttpPost(nameof(TransferLeadership))]
        public ActionResult TransferLeadership(int memberId) {
            return BuildViewResultFor(new TransferLeadershipCommand(_authenticationService.Identity, memberId))
                .OnSuccess(nameof(Home))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpGet(nameof(Disband))]
        public ViewResult Disband() {
            return View();
        }

        [AllianceAuthorize(CanDisbandAlliance = true)]
        [HttpPost(nameof(Disband))]
        public ActionResult DisbandPost() {
            return BuildViewResultFor(new DisbandAllianceCommand(_authenticationService.Identity))
                .OnSuccess(nameof(Index))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(SendNonAggressionPactRequest))]
        public ViewResult SendNonAggressionPactRequest(int id) {
            return View(_messageService.Dispatch(new GetCreateNonAggressionPactRequestQuery(id)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(SendNonAggressionPactRequest))]
        public ActionResult SendNonAggressionPactRequest(CreateNonAggressionPactRequestModel model) {
            return BuildViewResultFor(new SendNonAggressionPactRequestCommand(_authenticationService.Identity, model.AllianceId))
                .OnSuccess(nameof(SentNonAggressionPactRequests))
                .OnFailure(model);
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(SentNonAggressionPactRequests))]
        public ViewResult SentNonAggressionPactRequests() {
            return View(_messageService.Dispatch(new GetSentNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(WithdrawNonAggressionPactRequest))]
        public ActionResult WithdrawNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new WithdrawNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(SentNonAggressionPactRequests))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(ReceivedNonAggressionPactRequests))]
        public ViewResult ReceivedNonAggressionPactRequests() {
            return View(_messageService.Dispatch(new GetReceivedNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(AcceptNonAggressionPactRequest))]
        public ActionResult AcceptNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new AcceptNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(NonAggressionPacts))
                .OnFailure(nameof(ReceivedNonAggressionPactRequests), () => _messageService.Dispatch(new GetReceivedNonAggressionPactRequestsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(RejectNonAggressionPactRequest))]
        public ActionResult RejectNonAggressionPactRequest(int id) {
            return BuildViewResultFor(new RejectNonAggressionPactRequestCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(ReceivedNonAggressionPactRequests))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpGet(nameof(NonAggressionPacts))]
        public ViewResult NonAggressionPacts() {
            return View(_messageService.Dispatch(new GetNonAggressionPactsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageNonAggressionPacts = true)]
        [HttpPost(nameof(DissolveNonAggressionPact))]
        public ActionResult DissolveNonAggressionPact(int id) {
            return BuildViewResultFor(new DissolveNonAggressionPactCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(NonAggressionPacts))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpGet(nameof(Wars))]
        public ViewResult Wars() {
            return View(_messageService.Dispatch(new GetWarsQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpGet(nameof(DeclareWar))]
        public ViewResult DeclareWar(int id) {
            return View(_messageService.Dispatch(new GetDeclareWarQuery(id)));
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(DeclareWar))]
        public ActionResult DeclareWar(DeclareWarModel model) {
            return BuildViewResultFor(new DeclareWarCommand(_authenticationService.Identity, model.AllianceId))
                .OnSuccess(nameof(Wars))
                .OnFailure(model);
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(CancelPeaceDeclaration))]
        public ActionResult CancelPeaceDeclaration(int id) {
            return BuildViewResultFor(new CancelPeaceDeclarationCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Wars))
                .ThrowOnFailure();
        }

        [AllianceAuthorize(CanManageWars = true)]
        [HttpPost(nameof(DeclarePeace))]
        public ActionResult DeclarePeace(int id) {
            return BuildViewResultFor(new DeclarePeaceCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Wars))
                .ThrowOnFailure();
        }

        [AllianceAuthorize]
        [HttpGet(nameof(TransferResources))]
        public ViewResult TransferResources() {
            return View(_messageService.Dispatch(new GetTransferResourcesQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize]
        [HttpPost(nameof(TransferResources))]
        public ActionResult TransferResources(TransferResourcesModel model) {
            return BuildViewResultFor(new TransferResourcesCommand(_authenticationService.Identity, model.RecipientId.Value, model.Gold, model.Food, model.Wood, model.Stone, model.Ore))
                .OnSuccess(nameof(TransferResources))
                .OnFailure(model);
        }

        [AllianceAuthorize(CanBank = true)]
        [HttpGet(nameof(Banking))]
        public ViewResult Banking() {
            return View(_messageService.Dispatch(new GetBankedResourcesQuery(_authenticationService.Identity)));
        }

        [AllianceAuthorize(CanBank = true)]
        [HttpPost(nameof(Banking))]
        public ActionResult Banking(BankedResourcesModel model) {
            return model.Command switch {
                "deposit" => BuildViewResultFor(new DepositCommand(_authenticationService.Identity, model.Gold, model.Food, model.Wood, model.Stone, model.Ore))
                    .OnSuccess(nameof(Banking))
                    .OnFailure(model),
                "withdraw" => BuildViewResultFor(new WithdrawCommand(_authenticationService.Identity, model.Gold, model.Food, model.Wood, model.Stone, model.Ore))
                    .OnSuccess(nameof(Banking))
                    .OnFailure(model),
                _ => throw new InvalidOperationException($"Invalid operation '{model.Command}' found"),
            };
        }
    }
}
