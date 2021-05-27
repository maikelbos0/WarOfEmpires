using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("ScheduledTasks")]
        public PartialViewResult ScheduledTasks() {
            // Explicitly name view so it works from other actions
            return PartialView("ScheduledTasks", _messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [HttpPost("UnpauseScheduledTasks")]
        public PartialViewResult UnpauseScheduledTasks() {
            return BuildPartialViewResultFor(new UnpauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .ThrowOnFailure();
        }

        [HttpPost("PauseScheduledTasks")]
        public PartialViewResult PauseScheduledTasks() {
            return BuildPartialViewResultFor(new PauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .ThrowOnFailure();
        }
    }
}