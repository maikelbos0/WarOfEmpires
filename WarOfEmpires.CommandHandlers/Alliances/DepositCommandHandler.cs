using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<DepositCommand>))]
    public sealed class DepositCommandHandler : ICommandHandler<DepositCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IRankService _rankService;

        public DepositCommandHandler(IPlayerRepository repository, IRankService rankService) {
            _repository = repository;
            _rankService = rankService;
        }

        [Audit]
        public CommandResult<DepositCommand> Execute(DepositCommand command) {
            var result = new CommandResult<DepositCommand>();
            var player = _repository.Get(command.Email);
            var alliance = player.Alliance;
            var resources = new Resources(command.Gold ?? 0, command.Food ?? 0, command.Wood ?? 0, command.Stone ?? 0, command.Ore ?? 0);

            if (!player.CanAfford(resources)) {
                result.AddError("You don't have enough resources available to deposit that much");
            }

            if (alliance.BankTurns <= 0) {
                result.AddError("Your alliance doesn't have any bank turns available");
            }

            if (result.Success) {
                var highestRankedPlayer = alliance.Members.OrderBy(p => p.Rank).First();

                alliance.Deposit(player, _rankService.GetRatio(player, highestRankedPlayer), resources);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
