using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
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
            int farmers;
            int woodWorkers;
            int stoneMasons;
            int oreMiners;

            if (!int.TryParse(command.Farmers, out farmers) || farmers < 0) {
                result.AddError(c => c.Farmers, "Farmers must be a valid number");
            }

            if (!int.TryParse(command.WoodWorkers, out woodWorkers) || woodWorkers < 0) {
                result.AddError(c => c.WoodWorkers, "Wood workers must be a valid number");
            }

            if (!int.TryParse(command.StoneMasons, out stoneMasons) || stoneMasons < 0) {
                result.AddError(c => c.StoneMasons, "Stone masons must be a valid number");
            }

            if (!int.TryParse(command.OreMiners, out oreMiners) || oreMiners < 0) {
                result.AddError(c => c.OreMiners, "Ore miners must be a valid number");
            }

            return result;
        }
    }
}