using WarOfEmpires.Commands;
using System.Linq.Expressions;
using System;

namespace WarOfEmpires.CommandHandlers {
    public sealed class CommandError<TCommand> where TCommand : ICommand {
        public Expression<Func<TCommand, object>> Expression { get; set; }
        public string Message { get; set; }
    }
}