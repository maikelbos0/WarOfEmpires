﻿using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.Services;

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
        public ViewResultBase UnpauseScheduledTasks() {
            return GetCommandResultBuilder(new UnpauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .OnFailure(ScheduledTasks)
                .Resolve();
        }

        [Route("PauseScheduledTasks")]
        [HttpPost]
        public ViewResultBase PauseScheduledTasks() {
            return GetCommandResultBuilder(new PauseScheduledTasksCommand())
                .OnSuccess(ScheduledTasks)
                .OnFailure(ScheduledTasks)
                .Resolve();
        }
    }
}