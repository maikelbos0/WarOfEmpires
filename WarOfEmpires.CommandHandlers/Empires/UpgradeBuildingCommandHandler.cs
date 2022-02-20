using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class UpgradeBuildingCommandHandler : ICommandHandler<UpgradeBuildingCommand> {
        private readonly IPlayerRepository _repository;

        public UpgradeBuildingCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<UpgradeBuildingCommand> Execute(UpgradeBuildingCommand command) {
            var result = new CommandResult<UpgradeBuildingCommand>();
            var player = _repository.Get(command.Email);
            var buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), command.BuildingType);
            var buildingDefinition = BuildingDefinitionFactory.Get(buildingType);
            var buildingLevel = player.Buildings.SingleOrDefault(b => b.Type == buildingType)?.Level ?? 0;

            if (!player.CanAfford(buildingDefinition.GetNextLevelCost(buildingLevel))) {
                result.AddError($"You don't have enough resources to upgrade your {buildingDefinition.GetName(buildingLevel)}");
            }

            if (result.Success) {
                player.UpgradeBuilding(buildingType);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}