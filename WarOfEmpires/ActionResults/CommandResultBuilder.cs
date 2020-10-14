using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Controllers;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder<TCommand, TViewResult> where TCommand : ICommand where TViewResult : ViewResultBase, new() {
        private readonly IMessageService _messageService;
        private readonly IBaseController _controller;
        private readonly ModelStateDictionary _modelState;
        private readonly TCommand _command;
        private Func<TViewResult> _onFailure;
        private Func<TViewResult> _onSuccess;

        public CommandResultBuilder(IMessageService messageService, IBaseController controller, ModelStateDictionary modelState, TCommand command) {
            _messageService = messageService;
            _controller = controller;
            _command = command;
            _modelState = modelState;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnSuccess(Func<TViewResult> onSuccess) {
            _onSuccess = onSuccess;

            return this;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnSuccess(string viewName) {
            return OnSuccess(() => View(viewName));
        }

        public CommandResultBuilder<TCommand, TViewResult> OnFailure(Func<TViewResult> onFailure) {
            _onFailure = onFailure;

            return this;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnFailure(string viewName, object model) {
            return OnFailure(() => View(viewName, model));
        }

        public CommandResultBuilder<TCommand, TViewResult> ThrowOnFailure() {            
            return OnFailure(() => throw new InvalidOperationException($"Unexpected error executing {typeof(TCommand).FullName}: {JsonConvert.SerializeObject(_command)}"));
        }

        private TViewResult View(string viewName, object model = null) {            
            // TODO figure out why ViewEngineCollection is used
            return new TViewResult {
                ViewName = viewName,
                ViewData = new ViewDataDictionary(model),
                ViewEngineCollection = _controller.ViewEngineCollection
            };
        }

        public TViewResult Execute() {
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
    }
}