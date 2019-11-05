using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Queries;

namespace WarOfEmpires.Services {
    public interface IMessageService {
        CommandResult<TCommand> Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
        TReturnValue Dispatch<TReturnValue>(IQuery<TReturnValue> query);
    }
}