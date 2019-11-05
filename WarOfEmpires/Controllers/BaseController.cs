using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;
using System;
using System.Web.Mvc;

namespace WarOfEmpires.Controllers {
    public abstract class BaseController : Controller {
        protected readonly IMessageService _messageService;
        protected readonly IAuthenticationService _authenticationService;

        public BaseController(IMessageService messageService, IAuthenticationService authenticationService) {
            _messageService = messageService;
            _authenticationService = authenticationService;
        }

        protected ActionResult ValidatedCommandResult<TCommand>(object model, TCommand command, string onValidViewName) where TCommand : ICommand {
            return ValidatedCommandResult(model, command, () => View(onValidViewName));
        }

        protected ActionResult ValidatedCommandResult<TCommand>(object model, TCommand command, Func<ActionResult> onValid) where TCommand : ICommand {
            if (ModelState.IsValid) {
                ModelState.Merge(model, _messageService.Dispatch(command));
            }

            if (ModelState.IsValid) {
                return onValid();
            }
            else {
                return View(model);
            }
        }
    }
}