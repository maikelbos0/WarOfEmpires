using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        public PlayerRepository _repository;

        public UntrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            int farmers = 0;
            int woodWorkers = 0;
            int stoneMasons = 0;
            int oreMiners = 0;

            if (!string.IsNullOrEmpty(command.Farmers) && !int.TryParse(command.Farmers, out farmers) || farmers < 0) {
                result.AddError(c => c.Farmers, "Farmers must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.WoodWorkers) && !int.TryParse(command.WoodWorkers, out woodWorkers) || woodWorkers < 0) {
                result.AddError(c => c.WoodWorkers, "Wood workers must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.StoneMasons) && !int.TryParse(command.StoneMasons, out stoneMasons) || stoneMasons < 0) {
                result.AddError(c => c.StoneMasons, "Stone masons must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.OreMiners) && !int.TryParse(command.OreMiners, out oreMiners) || oreMiners < 0) {
                result.AddError(c => c.OreMiners, "Ore miners must be a valid number");
            }

            if (farmers > player.Farmers) {
                result.AddError(c => c.Farmers, "You don't have that many farmers to untrain");
            }

            if (woodWorkers > player.WoodWorkers) {
                result.AddError(c => c.WoodWorkers, "You don't have that many wood workers to untrain");
            }

            if (stoneMasons > player.StoneMasons) {
                result.AddError(c => c.StoneMasons, "You don't have that many stone masons to untrain");
            }

            if (oreMiners > player.OreMiners) {
                result.AddError(c => c.OreMiners, "You don't have that many ore miners to untrain");
            }

            if (result.Success) {
                player.UntrainWorkers(farmers, woodWorkers, stoneMasons, oreMiners);
                _repository.Update();
            }

            return result;
        }
    }
}