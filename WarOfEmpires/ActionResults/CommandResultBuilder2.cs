﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder2<TCommand, TActionResult> where TCommand : ICommand where TActionResult : ActionResult, new() {
        private readonly IMessageService _messageService;
        private readonly Func<object, TActionResult> _createView;
        private readonly ModelStateDictionary _modelState;
        private readonly IUrlHelper _urlHelper;
        private readonly bool _isAjaxRequest;
        private readonly TCommand _command;
        private Func<ActionResult> _onFailure;
        private string _onSuccess;

        public CommandResultBuilder2(IMessageService messageService, Func<object, TActionResult> createView, ModelStateDictionary modelState, IUrlHelper urlHelper, bool isAjaxRequest, TCommand command) {
            _messageService = messageService;
            _createView = createView;
            _modelState = modelState;
            _urlHelper = urlHelper;
            _isAjaxRequest = isAjaxRequest;
            _command = command;
        }
        
        public CommandResultBuilder2<TCommand, TActionResult> OnSuccess(string actionName, string controllerName, object routeValues) {
            _onSuccess = _urlHelper.Action(actionName, controllerName, routeValues);

            return this;
        }

        public CommandResultBuilder2<TCommand, TActionResult> OnFailure(object model) {
            _onFailure = () => _createView(model);

            return this;
        }

        public CommandResultBuilder2<TCommand, TActionResult> ThrowOnFailure() {
            _onFailure = () => throw new InvalidOperationException($"Unexpected error executing {typeof(TCommand).FullName}: {JsonConvert.SerializeObject(_command)}");

            return this;
        }

        public ActionResult Execute() {
            if (_onFailure == null) {
                throw new InvalidOperationException("Missing on failure result handler");
            }

            if (_onSuccess == null) {
                throw new InvalidOperationException("Missing on success result handler");
            }

            CommandResult<TCommand> result = null;

            if (_modelState.IsValid) {
                result = _messageService.Dispatch(_command);
                _modelState.Merge(result);
            }

            if (_modelState.IsValid) {
                if (_isAjaxRequest) {
                    return new JsonResult(new {
                        Success = true,
                        result.Warnings,
                        RedirectUrl = _onSuccess
                    });
                }
                else {
                    return new RedirectResult(_onSuccess);
                }
            }
            else {
                return _onFailure();
            }
        }

        public static implicit operator ActionResult(CommandResultBuilder2<TCommand, TActionResult> builder) {
            return builder.Execute();
        }
    }
}