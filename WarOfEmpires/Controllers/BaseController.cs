using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;
using System;
using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;

namespace WarOfEmpires.Controllers {
    public abstract class BaseController : Controller {
        protected readonly IMessageService _messageService;
        protected readonly IAuthenticationService _authenticationService;

        public BaseController(IMessageService messageService, IAuthenticationService authenticationService) {
            _messageService = messageService;
            _authenticationService = authenticationService;
        }

        protected ActionResult ValidatedCommandResult<TCommand>(object model, TCommand command, string onValidViewName) where TCommand : ICommand {
            return ValidatedCommandResult(model, command, (id) => View(onValidViewName));
        }

        protected ActionResult ValidatedCommandResult<TCommand>(object model, TCommand command, Func<ActionResult> onValid) where TCommand : ICommand {
            return ValidatedCommandResult(model, command, (id) => onValid());
        }

        protected ActionResult ValidatedCommandResult<TCommand>(object model, TCommand command, Func<int?, ActionResult> onValid) where TCommand : ICommand {
            CommandResult<TCommand> result = null;

            if (ModelState.IsValid) {
                result = _messageService.Dispatch(command);
                ModelState.Merge(model, result);
            }

            if (ModelState.IsValid) {
                // We're done so the current model is no longer relevant
                ModelState.Clear();

                if (result.HasWarnings) {
                    Response?.AddHeader("X-Warnings", string.Join("|", result.Warnings));
                }
                else {
                    // Let the client know explicitly that everything was valid
                    Response?.AddHeader("X-IsValid", "true");
                }

                return onValid(result?.ResultId);
            }
            else {
                return View(model);
            }
        }
    }
}