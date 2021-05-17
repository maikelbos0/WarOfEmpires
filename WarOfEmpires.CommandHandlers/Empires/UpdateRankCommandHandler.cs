using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateRankCommand>))]
    public sealed class UpdateRankCommandHandler : ICommandHandler<UpdateRankCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IRankService _rankService;

        public UpdateRankCommandHandler(IPlayerRepository repository, IRankService rankService) {
            _repository = repository;
            _rankService = rankService;
        }

        [Audit]
        public CommandResult<UpdateRankCommand> Execute(UpdateRankCommand command) {
            var result = new CommandResult<UpdateRankCommand>();

            _rankService.Update(_repository.GetAll());
            _repository.SaveChanges();

            return result;
        }
    }
}