using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [AdminAuthorize]
    [UserOnline]
    [RoutePrefix("Administration")]
    public class AdministrationController : BaseController {
        public AdministrationController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ViewResult Index() {
            return View();
        }

        [Route("ScheduledTasks")]
        [HttpGet]
        public PartialViewResult ScheduledTasks() {
            // Explicitly name view so it works from other actions
            return PartialView("ScheduledTasks", _messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [Route("UnpauseScheduledTasks")]
        [HttpPost]
        public PartialViewResult UnpauseScheduledTasks() {
            return BuildPartialViewResultFor(new UnpauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .ThrowOnFailure();
        }

        [Route("PauseScheduledTasks")]
        [HttpPost]
        public PartialViewResult PauseScheduledTasks() {
            return BuildPartialViewResultFor(new PauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .ThrowOnFailure();
        }
    }
}