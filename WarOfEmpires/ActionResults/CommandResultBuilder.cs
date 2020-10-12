using System;
using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Controllers;
using WarOfEmpires.Extensions;
using WarOfEmpires.Services;

namespace WarOfEmpires.ActionResults {
    public class CommandResultBuilder<TCommand> where TCommand : ICommand {
        private readonly IMessageService _messageService;
        private readonly BaseController _controller;
        private readonly TCommand _command;
        private Func<ViewResultBase> _onFailure;
        private Func<ViewResultBase> _onSuccess;

        public CommandResultBuilder(IMessageService messageService, BaseController controller, TCommand command) {
            _messageService = messageService;
            _controller = controller;
            _command = command;
        }

        public CommandResultBuilder<TCommand> OnSuccess(Func<ViewResultBase> onSuccess) {
            _onSuccess = onSuccess;

            return this;
        }

        public CommandResultBuilder<TCommand> OnSuccess(string onSuccessView) {
            return OnSuccess(() => View(viewName: onSuccessView));
        }

        public CommandResultBuilder<TCommand> OnFailure(Func<ViewResultBase> onFailure) {
            _onFailure = onFailure;

            return this;
        }

        public CommandResultBuilder<TCommand> OnFailure(object model) {
            return OnFailure(() => View(model: model));
        }

        public CommandResultBuilder<TCommand> OnFailure(string onFailureView, object model) {
            return OnFailure(() => View(viewName: onFailureView, model: model));
        }

        public CommandResultBuilder<TCommand> OnFailure(string onFailureView) {
            return OnFailure(() => View(viewName: onFailureView));
        }

        private ViewResult View(string viewName = null, object model = null) {
            if (model != null) {
                _controller.ViewData.Model = model;
            }

            return new ViewResult {
                ViewName = viewName,
                ViewData = _controller.ViewData,
                TempData = _controller.TempData,
                ViewEngineCollection = _controller.ViewEngineCollection
            };
        }

        public ViewResultBase Resolve() {
            if (_onFailure == null) {
                throw new InvalidOperationException("Missing on failure result handler");
            }

            if (_onSuccess == null) {
                throw new InvalidOperationException("Missing on success result handler");
            }

            CommandResult<TCommand> result = null;

            if (_controller.ModelState.IsValid) {
                result = _messageService.Dispatch(_command);
                _controller.ModelState.Merge(result);
            }

            if (_controller.ModelState.IsValid) {
                // We're done so the current model is no longer relevant
                _controller.ModelState.Clear();

                if (result.HasWarnings) {
                    _controller.Response?.AddHeader("X-Warnings", string.Join("|", result.Warnings));
                }
                else {
                    // Let the client know explicitly that everything was valid
                    _controller.Response?.AddHeader("X-IsValid", "true");
                }

                return _onSuccess();
            }
            else {
                return _onFailure();
            }
        }
    }
}