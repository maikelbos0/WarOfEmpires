using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainWorkersCommandHandler : ICommandHandler<TrainWorkersCommand> {
        public PlayerRepository _repository;

        public TrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            var result = new CommandResult<TrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            int farmers = 0;
            int woodWorkers = 0;
            int stoneMasons = 0;
            int oreMiners = 0;

            if (!string.IsNullOrEmpty(command.Farmers) && !int.TryParse(command.Farmers, out farmers) || farmers < 0) {
                result.AddError(c => c.Farmers, "Farmers must be a valid number");
                farmers = 0;
            }

            if (!string.IsNullOrEmpty(command.WoodWorkers) && !int.TryParse(command.WoodWorkers, out woodWorkers) || woodWorkers < 0) {
                result.AddError(c => c.WoodWorkers, "Wood workers must be a valid number");
                woodWorkers = 0;
            }

            if (!string.IsNullOrEmpty(command.StoneMasons) && !int.TryParse(command.StoneMasons, out stoneMasons) || stoneMasons < 0) {
                result.AddError(c => c.StoneMasons, "Stone masons must be a valid number");
                stoneMasons = 0;
            }

            if (!string.IsNullOrEmpty(command.OreMiners) && !int.TryParse(command.OreMiners, out oreMiners) || oreMiners < 0) {
                result.AddError(c => c.OreMiners, "Ore miners must be a valid number");
                oreMiners = 0;
            }

            if (farmers + woodWorkers + stoneMasons + oreMiners > player.Peasants) {
                result.AddError("You don't have that many peasants available to train");
            }

            if (farmers + woodWorkers + stoneMasons + oreMiners > player.GetAvailableHutCapacity()) {
                result.AddError("You don't have enough huts available to train that many workers");
            }

            if (!player.Resources.CanAfford((farmers + woodWorkers + stoneMasons + oreMiners) * Player.WorkerTrainingCost)) {
                result.AddError("You don't have enough gold to train these peasants");
            }

            if (result.Success) {
                player.TrainWorkers(farmers, woodWorkers, stoneMasons, oreMiners);
                _repository.Update();
            }

            return result;
        }
    }
}