﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder<TCommand, TActionResult> where TCommand : ICommand where TActionResult : ActionResult, new() {
        private readonly IMessageService _messageService;
        private readonly Func<string, object, TActionResult> _createView;
        private readonly ModelStateDictionary _modelState;
        private readonly IUrlHelper _urlHelper;
        private readonly bool _isAjaxRequest;
        private readonly TCommand _command;
        private Func<ActionResult> _onFailure;
        private Func<string> _onSuccess;

        public CommandResultBuilder(IMessageService messageService, Func<string, object, TActionResult> createView, ModelStateDictionary modelState, IUrlHelper urlHelper, bool isAjaxRequest, TCommand command) {
            _messageService = messageService;
            _createView = createView;
            _modelState = modelState;
            _urlHelper = urlHelper;
            _isAjaxRequest = isAjaxRequest;
            _command = command;
        }

        public CommandResultBuilder<TCommand, TActionResult> OnSuccess(string actionName) {
            return OnSuccess(actionName, () => null);
        }

        public CommandResultBuilder<TCommand, TActionResult> OnSuccess(string actionName, object routeValues) {
            return OnSuccess(actionName, () => routeValues);
        }
        public CommandResultBuilder<TCommand, TActionResult> OnSuccess(string actionName, Func<object> routeValuesFunc) {
            _onSuccess = () => _urlHelper.Action(actionName, null, routeValuesFunc());

            return this;
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(string viewName) {
            return OnFailure(viewName, () => null);
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(object model) {
            return OnFailure(null, () => model);
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(string viewName, object model) {
            return OnFailure(viewName, () => model);
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(string viewName, Func<object> modelFunc) {
            _onFailure = () => _createView(viewName, modelFunc());

            return this;
        }

        public CommandResultBuilder<TCommand, TActionResult> ThrowOnFailure() {
            _onFailure = () => throw new InvalidOperationException($"Unexpected error executing {typeof(TCommand).FullName}");

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
                        RedirectUrl = _onSuccess()
                    });
                }
                else {
                    return new RedirectResult(_onSuccess());
                }
            }
            else {
                return _onFailure();
            }
        }

        public static implicit operator ActionResult(CommandResultBuilder<TCommand, TActionResult> builder) {
            return builder.Execute();
        }
    }
}