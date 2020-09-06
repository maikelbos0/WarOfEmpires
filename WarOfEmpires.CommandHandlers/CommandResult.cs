using WarOfEmpires.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WarOfEmpires.CommandHandlers {
    public sealed class CommandResult<TCommand> where TCommand : ICommand {
        public List<CommandError<TCommand>> Errors { get; set; } = new List<CommandError<TCommand>>();
        public List<string> Warnings { get; set; } = new List<string>();

        public bool Success {
            get {
                return !Errors.Any();
            }
        }

        public bool HasWarnings {
            get {
                return Warnings.Any();
            }
        }

        public void AddError(string message) {
            AddError(null, message);
        }

        public void AddError(Expression<Func<TCommand, object>> expression, string message) {
            Errors.Add(new CommandError<TCommand>() {
                Expression = expression,
                Message = message
            });
        }

        public void AddWarning(string message) {
            Warnings.Add(message);
        }
    }
}