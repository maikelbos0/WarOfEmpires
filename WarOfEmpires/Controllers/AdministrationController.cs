using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Commands.Game;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [AdminAuthorize]
    [UserOnline]
    [Route(Route)]
    public class AdministrationController : BaseController {
        public const string Route = "Administration";

        public AdministrationController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View();
        }

        [HttpGet(nameof(_ScheduledTasks))]
        public PartialViewResult _ScheduledTasks() {
            return PartialView(_messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [HttpPost(nameof(_UnpauseScheduledTasks))]
        public ActionResult _UnpauseScheduledTasks() {
            return BuildPartialViewResultFor(new UnpauseScheduledTasksCommand())
                .OnSuccess(nameof(_ScheduledTasks))
                .ThrowOnFailure();
        }

        [HttpPost(nameof(_PauseScheduledTasks))]
        public ActionResult _PauseScheduledTasks() {
            return BuildPartialViewResultFor(new PauseScheduledTasksCommand())
                .OnSuccess(nameof(_ScheduledTasks))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(_GamePhase))]
        public PartialViewResult _GamePhase() {
            return PartialView(_messageService.Dispatch(new GetGamePhaseQuery()));
        }

        [HttpPost(nameof(_GamePhase))]
        public ActionResult _GamePhase(GamePhaseModel model) {
            return BuildPartialViewResultFor(new SetGamePhaseCommand(model.Phase))
                .OnSuccess(nameof(_GamePhase))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(Users))]
        public ViewResult Users() {
            return View(new UserSearchModel());
        }


        [HttpPost(nameof(GetUsers))]
        public JsonResult GetUsers(DataGridViewMetaData metaData, UserSearchModel search) {
            return GridJson(new GetUsersQuery(search.DisplayName), metaData);
        }

        [HttpGet(nameof(UserDetails))]
        public ViewResult UserDetails(int id) {
            return View(_messageService.Dispatch(new GetUserDetailsQuery(id)));
        }

        [HttpPost(nameof(UserDetails))]
        public ActionResult UserDetails(UserDetailsModel model) {
            return BuildViewResultFor(new UpdateUserDetailsCommand(model.Id, model.Email, model.DisplayName, model.AllianceCode, model.AllianceName, model.Status, model.IsAdmin))
                .OnSuccess(nameof(Users))
                .OnFailure(model);
        }

        [HttpGet(nameof(_ResetPlayers))]
        public PartialViewResult _ResetPlayers() {
            return PartialView();
        }


        [HttpPost(nameof(_ResetPlayers))]
        public ActionResult _ResetPlayersPost() {
            return BuildPartialViewResultFor(new ResetPlayersCommand())
                .OnSuccess(nameof(_ResetPlayers))
                .ThrowOnFailure();
        }
    }
}