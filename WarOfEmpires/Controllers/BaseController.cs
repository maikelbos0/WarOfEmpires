using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WarOfEmpires.ActionResults;
using WarOfEmpires.Commands;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    public abstract class BaseController : Controller, IBaseController {
        protected readonly IMessageService _messageService;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly IDataGridViewService _dataGridViewService;

        public BaseController(IMessageService messageService, IAuthenticationService authenticationService, IDataGridViewService dataGridViewService) {
            _messageService = messageService;
            _authenticationService = authenticationService;
            _dataGridViewService = dataGridViewService;
        }

        protected CommandResultBuilder2<TCommand, ViewResult> BuildViewResultFor2<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder2<TCommand, ViewResult>(_messageService, View, ModelState, Url, Request?.Headers["X-Requested-With"].ToString() == "XMLHttpRequest", command);
        }

        protected CommandResultBuilder2<TCommand, PartialViewResult> BuildPartialViewResultFor2<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder2<TCommand, PartialViewResult>(_messageService, PartialView, ModelState, Url, Request?.Headers["X-Requested-With"].ToString() == "XMLHttpRequest", command);
        }

        [Obsolete]
        protected CommandResultBuilder<TCommand, ViewResult> BuildViewResultFor<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder<TCommand, ViewResult>(_messageService, this, View, ModelState, command);
        }

        [Obsolete]
        protected CommandResultBuilder<TCommand, PartialViewResult> BuildPartialViewResultFor<TCommand>(TCommand command) where TCommand : ICommand {
            return new CommandResultBuilder<TCommand, PartialViewResult>(_messageService, this, PartialView, ModelState, command);
        }

        protected JsonResult GridJson<TReturnValue>(IQuery<IEnumerable<TReturnValue>> query, DataGridViewMetaData metaData) where TReturnValue : EntityViewModel {
            IEnumerable<TReturnValue> data = _messageService.Dispatch(query);

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }

        [NonAction]
        public void AddResponseHeader(string name, string value) {
            Response?.Headers.Add(name, value);
        }
    }
}