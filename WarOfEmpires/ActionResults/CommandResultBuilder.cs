using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Controllers;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder<TCommand, TViewResult> where TCommand : ICommand where TViewResult : ViewResultBase, new() {
        private readonly IMessageService _messageService;
        private readonly IBaseController _controller;
        private readonly TCommand _command;
        private Func<TViewResult> _onFailure;
        private Func<TViewResult> _onSuccess;

        public CommandResultBuilder(IMessageService messageService, IBaseController controller, TCommand command) {
            _messageService = messageService;
            _controller = controller;
            _command = command;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnSuccess(Func<TViewResult> onSuccess) {
            _onSuccess = onSuccess;

            return this;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnSuccess(string onSuccessView) {
            return OnSuccess(() => View(viewName: onSuccessView));
        }

        public CommandResultBuilder<TCommand, TViewResult> OnFailure(Func<TViewResult> onFailure) {
            _onFailure = onFailure;

            return this;
        }

        public CommandResultBuilder<TCommand, TViewResult> OnFailure(object model) {
            return OnFailure(() => View(model: model));
        }

        public CommandResultBuilder<TCommand, TViewResult> OnFailure(string onFailureView, object model) {
            return OnFailure(() => View(viewName: onFailureView, model: model));
        }

        // TODO determine if needed
        public CommandResultBuilder<TCommand, TViewResult> OnFailure(string onFailureView) {
            return OnFailure(() => View(viewName: onFailureView));
        }

        public CommandResultBuilder<TCommand, TViewResult> ThrowOnFailure() {            
            return OnFailure(() => throw new InvalidOperationException($"Unexpected error executing {typeof(TCommand).FullName}: {JsonConvert.SerializeObject(_command)}"));
        }

        private TViewResult View(string viewName = null, object model = null) {
            if (model != null) {
                _controller.ViewData.Model = model;
            }

            // TODO figure out why ViewData and ViewEngineCollection are used
            return new TViewResult {
                ViewName = viewName,
                ViewData = _controller.ViewData,
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

            if (_controller.IsModelStateValid()) {
                result = _messageService.Dispatch(_command);
                _controller.MergeModelState(result);
            }

            if (_controller.IsModelStateValid()) {
                // We're done so the current model is no longer relevant
                _controller.ClearModelState();

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