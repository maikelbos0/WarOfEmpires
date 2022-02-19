using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<RecruitCommand>))]
    public sealed class RecruitCommandHandler : ICommandHandler<RecruitCommand> {
        private readonly IPlayerRepository _repository;

        public RecruitCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<RecruitCommand> Execute(RecruitCommand command) {
            var result = new CommandResult<RecruitCommand>();

            foreach (var player in _repository.GetAll()) {
                player.Recruit();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}