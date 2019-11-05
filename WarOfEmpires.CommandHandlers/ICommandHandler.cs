using WarOfEmpires.Commands;

namespace WarOfEmpires.CommandHandlers {
    public interface ICommandHandler<TCommand> where TCommand : ICommand {
        CommandResult<TCommand> Execute(TCommand command);
    }
}