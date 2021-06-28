using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Commands.Game;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [AdminAuthorize]
    [UserOnline]
    [Route("Administration")]
    public class AdministrationController : BaseController {
        public AdministrationController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [HttpGet("Index")]
        public ViewResult Index() {
            return View();
        }

        [HttpGet("_ScheduledTasks")]
        public PartialViewResult _ScheduledTasks() {
            // Explicitly name view so it works from other actions
            return PartialView("_ScheduledTasks", _messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [HttpPost("_UnpauseScheduledTasks")]
        public PartialViewResult _UnpauseScheduledTasks() {
            return BuildPartialViewResultFor(new UnpauseScheduledTasksCommand())
                .OnSuccess(_ScheduledTasks)
                .ThrowOnFailure();
        }

        [HttpPost("_PauseScheduledTasks")]
        public PartialViewResult _PauseScheduledTasks() {
            return BuildPartialViewResultFor(new PauseScheduledTasksCommand())
                .OnSuccess(_ScheduledTasks)
                .ThrowOnFailure();
        }

        [HttpGet("_GamePhase")]
        public PartialViewResult _GamePhase() {
            return PartialView(_messageService.Dispatch(new GetGamePhaseQuery()));
        }

        [HttpPost("_GamePhase")]
        public PartialViewResult _GamePhase(GamePhaseModel model) {
            return BuildPartialViewResultFor(new SetGamePhaseCommand(model.Phase))
                .OnSuccess(_GamePhase)
                .ThrowOnFailure();
        }
    }
}