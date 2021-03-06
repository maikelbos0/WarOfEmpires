﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Controllers;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder<TCommand, TActionResult> where TCommand : ICommand where TActionResult : IActionResult, new() {
        private readonly IMessageService _messageService;
        private readonly IBaseController _controller;
        private readonly Func<string, object, TActionResult> _createView;
        private readonly ModelStateDictionary _modelState;
        private readonly TCommand _command;
        private Func<TActionResult> _onFailure;
        private Func<TActionResult> _onSuccess;

        public CommandResultBuilder(IMessageService messageService, IBaseController controller, Func<string, object, TActionResult> createView, ModelStateDictionary modelState, TCommand command) {
            _messageService = messageService;
            _controller = controller;
            _createView = createView;
            _modelState = modelState;
            _command = command;
        }

        public CommandResultBuilder<TCommand, TActionResult> OnSuccess(string viewName) {
            return OnSuccess(() => _createView(viewName, null));
        }

        public CommandResultBuilder<TCommand, TActionResult> OnSuccess(Func<TActionResult> onSuccess) {
            _onSuccess = onSuccess;

            return this;
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(string viewName) {
            return OnFailure(viewName, null);
        }

        public CommandResultBuilder<TCommand, TActionResult> OnFailure(string viewName, object model) {
            _onFailure = () => _createView(viewName, model);

            return this;
        }

        public CommandResultBuilder<TCommand, TActionResult> ThrowOnFailure() {
            _onFailure = () => throw new InvalidOperationException($"Unexpected error executing {typeof(TCommand).FullName}: {JsonConvert.SerializeObject(_command)}");

            return this;
        }

        public TActionResult Execute() {
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
                // We're done so the current model is no longer relevant
                _modelState.Clear();

                if (result.HasWarnings) {
                    _controller.AddResponseHeader("X-Warnings", string.Join("|", result.Warnings));
                }
                else {
                    // Let the client know explicitly that everything was valid
                    _controller.AddResponseHeader("X-IsValid", "true");
                }

                return _onSuccess();
            }
            else {
                return _onFailure();
            }
        }

        public static implicit operator TActionResult (CommandResultBuilder<TCommand, TActionResult> builder) {
            return builder.Execute();
        }
    }
}