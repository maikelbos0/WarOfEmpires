using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WarOfEmpires.ActionResults;
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

        protected CommandResultBuilder<TCommand, ViewResult> BuildViewResultFor<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder<TCommand, ViewResult>(_messageService, View, ModelState, Url, Request.IsAjaxRequest(), command);
        }

        protected CommandResultBuilder<TCommand, PartialViewResult> BuildPartialViewResultFor<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder<TCommand, PartialViewResult>(_messageService, PartialView, ModelState, Url, Request.IsAjaxRequest(), command);
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
