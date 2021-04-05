using WarOfEmpires.Commands;

namespace WarOfEmpires.CommandHandlers.Tests {
    public static class FluentAssertionsExtensions {
        public static CommandResultAssertions<TCommand> Should<TCommand>(this CommandResult<TCommand> instance) where TCommand : ICommand {
            return new CommandResultAssertions<TCommand>(instance);
        }
    }
}