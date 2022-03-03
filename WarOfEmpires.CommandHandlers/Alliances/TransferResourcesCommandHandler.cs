using System;
using System.Linq;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {

    public sealed class TransferResourcesCommandHandler : ICommandHandler<TransferResourcesCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IRankService _rankService;

        public TransferResourcesCommandHandler(IPlayerRepository repository, IRankService rankService) {
            _repository = repository;
            _rankService = rankService;
        }

        public CommandResult<TransferResourcesCommand> Execute(TransferResourcesCommand command) {
            var result = new CommandResult<TransferResourcesCommand>();
            var currentPlayer = _repository.Get(command.Email);
            var recipient = currentPlayer.Alliance.Members.Single(m => m.Id == command.RecipientId);
            var resources = new Resources(command.Gold ?? 0, command.Food ?? 0, command.Wood ?? 0, command.Stone ?? 0, command.Ore ?? 0);

            if (currentPlayer == recipient) {
                throw new InvalidOperationException("You can't transfer resources to yourself");
            }

            if (!currentPlayer.CanAfford(resources)) {
                result.AddError("You don't have enough resources available to transfer that much");
            }

            if (result.Success) {
                currentPlayer.TransferResources(_rankService.GetRatio(currentPlayer, recipient), recipient, resources);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
