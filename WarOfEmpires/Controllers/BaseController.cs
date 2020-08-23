using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    public abstract class BaseController : Controller {
        protected readonly IMessageService _messageService;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly IDataGridViewService _dataGridViewService;

        public BaseController(IMessageService messageService, IAuthenticationService authenticationService, IDataGridViewService dataGridViewService) {
            _messageService = messageService;
            _authenticationService = authenticationService;
            _dataGridViewService = dataGridViewService;
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
                ModelState.Merge(result);
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

        protected JsonResult GridJson<TReturnValue>(IQuery<IEnumerable<TReturnValue>> query, DataGridViewMetaData metaData) where TReturnValue : EntityViewModel {
            IEnumerable<TReturnValue> data = _messageService.Dispatch(query);

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }
    }
}