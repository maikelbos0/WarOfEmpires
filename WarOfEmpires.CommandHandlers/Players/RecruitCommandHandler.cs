using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class RecruitCommandHandler : ICommandHandler<RecruitCommand> {
        public PlayerRepository _repository;

        public RecruitCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<RecruitCommand> Execute(RecruitCommand command) {
            var result = new CommandResult<RecruitCommand>();

            foreach (var player in _repository.GetAll()) {
                player.Recruit();
            }

            _repository.Update();

            return result;
        }
    }
}