using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [AdminAuthorize]
    [RoutePrefix("Administration")]
    public class AdministrationController : BaseController {
        public AdministrationController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [Route("ScheduledTasks")]
        [HttpGet]
        public ActionResult ScheduledTasks() {
            return PartialView(_messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [Route("UnpauseScheduledTasks")]
        [HttpPost]
        public ActionResult UnpauseScheduledTasks() {
            _messageService.Dispatch(new UnpauseScheduledTasksCommand());

            return PartialView("ScheduledTasks", _messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }

        [Route("PauseScheduledTasks")]
        [HttpPost]
        public ActionResult PauseScheduledTasks() {
            _messageService.Dispatch(new PauseScheduledTasksCommand());

            return PartialView("ScheduledTasks", _messageService.Dispatch(new GetScheduledTasksPausedQuery()));
        }
    }
}