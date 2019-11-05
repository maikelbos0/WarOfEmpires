using WarOfEmpires.Commands;
using System;
using System.Linq.Expressions;

namespace WarOfEmpires.CommandHandlers {
    public sealed class CommandError<TCommand> where TCommand : ICommand {
        public Expression<Func<TCommand, object>> Expression { get; set; }
        public string Message { get; set; }
    }
}