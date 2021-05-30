using WarOfEmpires.Commands;
using System.Linq.Expressions;

namespace WarOfEmpires.CommandHandlers {
    public sealed class CommandError<TCommand> where TCommand : ICommand {
        public LambdaExpression Expression { get; set; }
        public string Message { get; set; }
    }
}